using System.Formats.Tar;
using System.Globalization;
using System.IO.Compression;

if (args.Length < 1)
{
    return PrintUsage();
}

if (args[0] == "--compress" && args.Length == 3)
{
    var destination = args[1];
    var sourceDir = args[2];
    CreateTar(destination, sourceDir);
}
else if (args[0] == "--extract" && args.Length == 3)
{
    var sourceTar = args[1];
    var destinationDir = args[2];
    ExtractTar(sourceTar, destinationDir);
}
else if (args[0] == "--single" && args.Length == 4)
{
    var sourceTar = args[1];
    var pathInTar = args[2];
    var output = args[3];
    ExtractSingle(sourceTar, pathInTar, output);
}
else if (args[0] == "--list" && args.Length == 2)
{
    var sourceTar = args[1];
    List(sourceTar);
}
else
{
    return PrintUsage();
}

return 0;

void CreateTar(string dest, string src)
{
    Console.WriteLine($"Creating Tar file: '{dest}' from path '{src}");

    // Throws if the destination file exists
    using FileStream fs = new(dest, FileMode.CreateNew, FileAccess.Write);
    using GZipStream gz = new(fs, CompressionMode.Compress, leaveOpen: true);

    TarFile.CreateFromDirectory(src, gz, includeBaseDirectory: false);

    Console.WriteLine("Done");
}

void ExtractTar(string sourceTar, string dest)
{
    Console.WriteLine($"Expanding Tar file: '{sourceTar}' to directory '{dest}");

    // Throws if the file does not exist
    using FileStream fs = new(sourceTar, FileMode.Open, FileAccess.Read);
    using GZipStream gz = new(fs, CompressionMode.Decompress, leaveOpen: true);

    TarFile.ExtractToDirectory(gz, dest, overwriteFiles: false);

    Console.WriteLine("Done");
}

void ExtractSingle(string sourceTar, string pathInTar, string destination)
{
    Console.WriteLine($"Looking for '{pathInTar}' in file: '{sourceTar}'");

    // read the tar and extract a single file
    using FileStream fs = new(sourceTar, FileMode.Open, FileAccess.Read);
    using GZipStream gz = new(fs, CompressionMode.Decompress, leaveOpen: true);
    using var reader = new TarReader(gz, leaveOpen: true);

    while (reader.GetNextEntry() is { } entry)
    {
        if (entry.Name == pathInTar)
        {
            Console.WriteLine($"Found '{pathInTar}', extracting to '{destination}");
            entry.ExtractToFile(destination, overwrite: false);
            return;
        }
    }

    Console.WriteLine("Could not extract path - not found");
}

void List(string sourceTar)
{
    Console.WriteLine($"Reading '{sourceTar}'");

    // read the tar and loop through the entries
    using FileStream fs = new(sourceTar, FileMode.Open, FileAccess.Read);
    using GZipStream gz = new(fs, CompressionMode.Decompress, leaveOpen: true);
    using var reader = new TarReader(gz, leaveOpen: true);

    while (reader.GetNextEntry() is { } entry)
    {
        // Get the file descriptor
        char type = entry.EntryType switch
        {
            TarEntryType.Directory => 'd',
            TarEntryType.HardLink => 'h',
            TarEntryType.SymbolicLink => 'l',
            _ => '-',
        };

        // Construct the permissions e.g. rwxr-xr-x
        // Moved to a separate function just because it's a bit verbose
        string permissions = GetPermissions(entry);

        // Display the owner info. 0 is special (root) but .NET doesn't
        // expose the mappings for these IDs natively, so ignoring for now 
        string ownerUser = entry.Uid == 0 ? "root" : entry.Uid.ToString(CultureInfo.InvariantCulture);
        string ownerGroup = entry.Gid == 0 ? "root" : entry.Gid.ToString(CultureInfo.InvariantCulture);

        // The length of the data and the modification date in bytes
        long size = entry.Length;
        DateTimeOffset date = entry.ModificationTime.UtcDateTime;

        // Match the display format used by tar -tv 
        string path = entry.EntryType switch
        {
            TarEntryType.HardLink => $"{entry.Name} link to {entry.LinkName}",
            TarEntryType.SymbolicLink => $"{entry.Name} -> {entry.LinkName}",
            _ => entry.Name,
        };

        // Write the entry!
        Console.WriteLine($"{type}{permissions} {ownerUser}/{ownerGroup} {size,9} {date:yyyy-MM-dd HH:mm} {path}");
    }

    // Construct the permissions
    static string GetPermissions(TarEntry entry)
    {
        var userRead = entry.Mode.HasFlag(UnixFileMode.UserRead) ? 'r' : '-';
        var userWrite = entry.Mode.HasFlag(UnixFileMode.UserWrite) ? 'w' : '-';
        var userExecute = entry.Mode.HasFlag(UnixFileMode.UserExecute) ? 'x' : '-';
        var groupRead = entry.Mode.HasFlag(UnixFileMode.GroupRead) ? 'r' : '-';
        var groupWrite = entry.Mode.HasFlag(UnixFileMode.GroupWrite) ? 'w' : '-';
        var groupExecute = entry.Mode.HasFlag(UnixFileMode.GroupExecute) ? 'x' : '-';
        var otherRead = entry.Mode.HasFlag(UnixFileMode.OtherRead) ? 'r' : '-';
        var otherWrite = entry.Mode.HasFlag(UnixFileMode.OtherWrite) ? 'w' : '-';
        var otherExecute = entry.Mode.HasFlag(UnixFileMode.OtherExecute) ? 'x' : '-';
        
        return $"{userRead}{userWrite}{userExecute}{groupRead}{groupWrite}{groupExecute}{otherRead}{otherWrite}{otherExecute}";
    }
}

int PrintUsage()
{
    Console.WriteLine("USAGE: dotnetar --create <DESTINATIONTAR> <SOURCE>");
    Console.WriteLine("USAGE: dotnetar --extract <SOURCETAR> <DESTINATION>");
    Console.WriteLine("USAGE: dotnetar --single <SOURCETAR> <PATH_IN_TAR> <OUTPUT_PATH>");
    Console.WriteLine("USAGE: dotnetar --list <SOURCETAR>");
    return 1;
}