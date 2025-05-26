var builder = DistributedApplication.CreateBuilder(args);

// Create these values as secrets
var postgresUser = builder.AddParameter("db-user", secret: true);
var postgresPassword = builder.AddParameter("db-password", secret: true);

// Add a default for the database name 
var postgresDbName = builder.AddParameter("db-name", "listmonk", publishValueAsDefault: true);

var listmonkSuperUser = builder.AddParameter("listmonk-admin-user", secret: true);
var listmonkSuperUserPassword = builder.AddParameter("listmonk-admin-password", secret: true);

var dbPort = 5432;
var dbContainerName = "listmonk_db";
var publicPort = 9000;

// Sets the POSTGRES_USER and POSTGRES_PASSWORD implicitly
var db = builder.AddPostgres("db", postgresUser, postgresPassword, port: dbPort)
    .WithImage("postgres", "17-alpine") // Ensure we use the same image as docker-compose
    .WithContainerName(dbContainerName) // Use a fixed container name
    .WithLifetime(ContainerLifetime.Persistent) // Don't tear-down the container when we stop Aspire
    .WithDataVolume("listmonk-data") // Wire up the PostgreSQL data volume
    .WithEnvironment("POSTGRES_DB", postgresDbName) // Explicitly set this value, so that it's auto-created
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Restart = "unless-stopped";
        service.Healthcheck = new()
        {
            Interval = "10s",
            Timeout = "5s",
            Retries = 6,
            StartPeriod = "0s", 
            Test = ["CMD-SHELL", "pg_isready -U listmonk"]
        };
    });

builder.AddContainer(name: "listmonk", image: "listmonk/listmonk", tag: "latest") 
    .WaitFor(db) // The app depends on the db, so wait for it to be healthy
    .WithHttpEndpoint(port: publicPort, targetPort: 9000) // Expose port 9000 in the container as "publicPort"
    .WithExternalHttpEndpoints() // The HTTP endpoint should be publicly accessibly
    .WithArgs("sh", "-c", "./listmonk --install --idempotent --yes --config '' && ./listmonk --upgrade --yes --config '' && ./listmonk --config ''")
    .WithBindMount(source: "./uploads", target: "/listmonk/uploads") // mount the folder ./uploads on the host into the container 
    .WithEnvironment("LISTMONK_app__address", $"0.0.0.0:{publicPort.ToString()}") // This points to the app itself (used in emails)
    .WithEnvironment("LISTMONK_db__user", postgresUser) // Database connection settings
    .WithEnvironment("LISTMONK_db__password", postgresPassword)
    .WithEnvironment("LISTMONK_db__database", postgresDbName)
    .WithEnvironment("LISTMONK_db__host", dbContainerName)
    .WithEnvironment("LISTMONK_db__port", dbPort.ToString())
    .WithEnvironment("LISTMONK_db__ssl_mode", "disable")
    .WithEnvironment("LISTMONK_db__max_open", "25")
    .WithEnvironment("LISTMONK_db__max_idle", "25")
    .WithEnvironment("LISTMONK_db__max_lifetime", "300s")
    .WithEnvironment("TZ", "Etc/UTC")
    .WithEnvironment("LISTMONK_ADMIN_USER", listmonkSuperUser) // Optional super-user
    .WithEnvironment("LISTMONK_ADMIN_PASSWORD", listmonkSuperUserPassword)
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Restart = "unless-stopped";
    });

builder.AddDockerComposeEnvironment("docker-compose");

builder.Build().Run();
