using System.ComponentModel.DataAnnotations;

namespace Server.GraphQL.Data
{
    public class Speaker: IRelayNode
    {
        [IsProjected(true)] // Needed to make sure this is available to calculate Relay Node Id
        public int SpeakerId { get; set; }

        public override int DbId() => SpeakerId;

        // FIXME in c#11 we can use `required string` and remove `[Required]` annotation
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(4000)]
        public string? Bio { get; set; }

        [StringLength(1000)]
        public virtual string? WebSite { get; set; }

        public List<Topic> Topics { get; set; } = new List<Topic>();

    }
}