using Server.GraphQL.Data;
using Server.GraphQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services
    .AddGraphQLServer()
    // We define our own IRelayNode (graphql RelayNode) interface, so we need to add the types that implement it
    // .AddType<Speaker>()
    // .AddType<Topic>()
    .AddGlobalObjectIdentification()
    .AddQueryFieldToMutationPayloads()
    // FIXME move to Pooled, and look through steps at https://chillicream.com/docs/hotchocolate/v12/integrations/entity-framework#working-with-a-pooled-dbcontext
    .RegisterDbContext<ApplicationDbContext>(DbContextKind.Synchronized)
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    // FIXME enable only in development
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);  

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGraphQL();

app.Run();

