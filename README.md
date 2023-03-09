# README

Initial installation:

1. Install VS Code .net extensions
2. Open in VS Code
3. Accept prompts to install/restore required tools/components
4. Run migrations to set up database (from root of project, containing `GraphQL` dir): `dotnet ef database update --project GraphQL` - if prompted, run `dotnet tool restore` to install tools.
5. Run project with `dotnet run --project GraphQL`
6. Look through startup messages to find the http URL, e.g. `http://localhost:5252`. Your port number may be different.
7. Open this URL with `/graphql/` appended, e.g. `http://localhost:5252/graphql`
8. Browse to the schema in the graphql app displayed, and accept the default connection parameters (should match the URL used for web interface, with no trailing '/')

## Cheatsheet

To delete database and start again:

```bash
dotnet ef database drop --project GraphQL
dotnet ef database update --project GraphQL
```

To [drop migrations and start again](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=dotnet-core-cli#resetting-all-migrations) (note this is only during development - you will lose all data and can't migrate existing records etc.):

```bash
# Delete `Migrations` folder
rm -rf ./GraphQL/Migrations

# Drop database
dotnet ef database drop --project GraphQL

# Create a new initial migration
dotnet ef migrations add Initial --project GraphQL

# Apply the migration to recreate database
dotnet ef database update --project GraphQL
```

## Notes on how this was produced

* Followed tutorial at https://github.com/ChilliCream/graphql-workshop/blob/master/docs/1-creating-a-graphql-server-project.md, also referring to [getting started page](https://chillicream.com/docs/hotchocolate/v12/get-started) which is more up to date but doesn't use ef.
* For ef versions, used 6.0.11 rather than 5.0.0 as specified - this fixed an error where sqlite could not be run when updating database, on macos arm64, see [this issue](https://github.com/dotnet/efcore/issues/24198)
* For [this stage](https://github.com/ChilliCream/graphql-workshop/blob/master/docs/1-creating-a-graphql-server-project.md#register-the-db-context-service) had to update from `Startup.cs` to `Program.cs` using [these docs](https://learn.microsoft.com/en-us/aspnet/core/migration/50-to-60?view=aspnetcore-7.0&tabs=visual-studio). This requires moving the service configuration to `builder.Services`, and using our namespace plus `Microsoft.EntityFrameworkCore`.
* Note that to get the entity framework tools, we use the following step of the tutorial:
    ```bash
    dotnet new tool-manifest
    dotnet tool install dotnet-ef --version 5.0.0 --local
    ```
* In the section on `Configuring EF Migrations`:
    * The line `dotnet build GraphQL` should be run in the root dir (containing the `GraphQL` dir), *not* in the `GraphQL` dir itself.
    * The line `dotnet ef migrations add Initial --project GraphQL` will fail because it can't create the `ApplicationDbContext` instance. We need to adjust `ApplicationDbContext.cs` so that it has a no-param constructor, and an `OnConfiguring` method that includes the `UseSqlite("Data Source=data.db")` call. We can then remove this from `Program.cs` so we just call `builder.Services.AddDbContext<ApplicationDbContext>();` without having to specify options here.
    An alternate solution from [stack overflow](https://stackoverflow.com/questions/70273434/unable-to-resolve-service-for-type-%C2%A8microsoft-entityframeworkcore-dbcontextopti) is to provide a factory for the context, similar to this example (looks to be from [this ms page](https://learn.microsoft.com/en-gb/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory)):
      ```cs
      public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
      {
          public BloggingContext CreateDbContext(string[] args)
          {
              var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();
                optionsBuilder.UseNpgsql();

              return new BloggingContext(optionsBuilder.Options);
          }
      }
      ```
    * The line `dotnet ef database update --project GraphQL` won't run on macos-arm64 with ef-core/sqlite 5.0.0, 6.0.11 works.
* When installing HotChocolate, we used 12.15.2 rather than 11.0.0.
* No location given for `Query.cs`, it is in `GraphQL` project root dir - seems to be the case for other files where no location is given.
* When registering `Query` we need to add to `Program.cs` and use `builder.Services`.
* The `MapGraphQl()` call is made on `app` instance in `Program.cs`

Continued with part 2:

* Instead of `public DbSet<Speaker> Speakers { get; set; } = default!;`, used `public DbSet<Speaker> Speakers => Set<Speaker>();` as recommended in [ef nullability guide](https://learn.microsoft.com/en-us/ef/core/miscellaneous/nullable-reference-types)
* Then set up synchronised DbContext access according to [this page](https://chillicream.com/docs/hotchocolate/v12/integrations/entity-framework)
* Then added projection according to [Enable projection](https://chillicream.com/docs/hotchocolate/fetching-data/projections). Without it turned on, we get empty `Topics` property in queries, when turned on this is populated as expected.
* Add an interface `IRelayNode` (just `RelayNode` in GraphQL) for our own version of Node, see [docs](https://chillicream.com/docs/hotchocolate/defining-a-schema/interfaces)

## TODO

1. Enable Relay features
    1. Node id - currently doesn't work with projection, can we enable this?
    2. Cursors for lists
2. Work out whether we can use pooled DbContext for parallel queries (currently we are using synchronous access).
3. Work out whether we can have [DataLoader](https://chillicream.com/docs/hotchocolate/v12/fetching-data/dataloader) for batched queries at the same time as projection - might need to wait for [HC 13.0](https://github.com/ChilliCream/hotchocolate/milestone/65) based on note under *Projection* below?

## Projection notes

[Issue on workshop](https://github.com/ChilliCream/graphql-workshop/issues/91) looks relevant, but I can't find the relevant issue on hot chocolate itself. [This issue](https://github.com/ChilliCream/graphql-workshop/issues/56) is probably also relevant. Looks like workshop really is out of date, although I think we've got it up to date as far as session 2 with the changes above.