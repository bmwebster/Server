using System.ComponentModel.DataAnnotations;

namespace Server.GraphQL.Data
{
    public class Topic: IRelayNode
    {
        [IsProjected(true)] // Needed to make sure this is available to calculate Relay Node Id
        public int TopicId { get; set; }

        public override int DbId() => TopicId;

        // FIXME in c#11 we can use `required string` and remove `[Required]` annotation
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

    }
}