using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<LoggerBenchmark>();

[MemoryDiagnoser]
public partial class LoggerBenchmark
{
    private const string Message = "Testing a message by {Person} and a {Time}";
    private readonly ILogger _logger;
    private readonly Person _person;
    private readonly DateTime _startTime;

    private readonly Action<ILogger, Person, DateTime, Exception?> _logEnabled =
        LoggerMessage.Define<Person, DateTime>(
            logLevel: LogLevel.Information,
            eventId: 1,
            formatString: Message);

    private readonly Action<ILogger, Person, DateTime, Exception?> _logDisabled =
        LoggerMessage.Define<Person, DateTime>(
            logLevel: LogLevel.Debug,
            eventId: 2,
            formatString: Message);

    [LoggerMessage(Level = LogLevel.Information, Message = Message)]
    partial void LogEnabled(Person person, DateTime time);

    [LoggerMessage(Level = LogLevel.Debug, Message = Message)]
    partial void LogDisabled(Person person, DateTime time);

    [LoggerMessage(Message = Message)]
    partial void LogDynamicLevel(LogLevel level, Person person, DateTime time);

    public LoggerBenchmark()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.Services.AddSingleton<ILoggerProvider, InMemoryProvider>());
        _logger = loggerFactory.CreateLogger(nameof(LoggerBenchmark));
        _person = new Person(123, "Joe Blogs");
        _startTime = DateTime.UtcNow;
    }

    [Benchmark]
    public void InterpolatedEnabled()
    {
        _logger.LogInformation($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void InterpolatedDisabled()
    {
        _logger.LogDebug($"Testing a message by {_person} and a {_startTime}");
    }

    [Benchmark]
    public void DirectCallEnabled()
    {
        _logger.LogInformation(Message, _person, _startTime);
    }

    [Benchmark]
    public void DirectCallDisabled()
    {
        _logger.LogDebug(Message, _person, _startTime);
    }

    [Benchmark]
    public void IsEnabledCheckEnabled()
    {
        if (_logger.IsEnabled(LogLevel.Information))
        {
            _logger.LogInformation(Message, _person, _startTime);
        }
    }

    [Benchmark]
    public void IsEnabledCheckDisabled()
    {
        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(Message, _person, _startTime);
        }
    }

    [Benchmark]
    public void LoggerMessageDefineEnabled()
    {
        _logEnabled(_logger, _person, _startTime, null);
    }

    [Benchmark]
    public void LoggerMessageDefineDisabled()
    {
        _logDisabled(_logger, _person, _startTime, null);
    }

    [Benchmark]
    public void SourceGeneratorEnabled()
    {
        LogEnabled(_person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDisabled()
    {
        LogDisabled(_person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDynamicLevelEnabled()
    {
        LogDynamicLevel(LogLevel.Information, _person, _startTime);
    }

    [Benchmark]
    public void SourceGeneratorDynamicLevelDisabled()
    {
        LogDynamicLevel(LogLevel.Debug, _person, _startTime);
    }
}


public class InMemoryProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new InMemoryLogger();

    public void Dispose()
    {
    }

    private class InMemoryLogger : ILogger
    {
        private int _count;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Information;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _count++;
        }
    }

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();

        /// 
        public void Dispose()
        {
        }
    }
}
public record Person(int Id, string Name);