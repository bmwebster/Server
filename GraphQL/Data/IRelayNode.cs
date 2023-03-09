namespace Server.GraphQL.Data
{
  [InterfaceType("Node")]  // Used as a GraphQL interface type
  public abstract class IRelayNode {    

    // This just lets us find the actual database id
    [GraphQLIgnore]
    public abstract int DbId();

    [GraphQLType(typeof(IdType))]
    public string Id([Service] IIdSerializer serializer) => 
      serializer.Serialize<int>("", this.GetType().Name, DbId());
  }
}