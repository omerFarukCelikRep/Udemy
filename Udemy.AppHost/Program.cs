IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<PostgresServerResource> postgres = builder.AddPostgres("demo");

builder.AddProject<Projects.Udemy_Catalog_WebApi>("udemy-catalog-webapi")
    .WithReference(postgres)
    .WaitFor(postgres);

await builder.Build().RunAsync();
