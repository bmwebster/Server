using System.ComponentModel.DataAnnotations;

namespace Server.GraphQL.Data
{
    [Node(IdField = nameof(SpeakerId))]
    public class Speaker
    {
        public int SpeakerId { get; set; }

        // FIXME in c#11 we can use `required string` and remove `[Required]` annotation
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(4000)]
        public string? Bio { get; set; }

        [StringLength(1000)]
        public virtual string? WebSite { get; set; }

        public List<Topic> Topics { get; set; } = new List<Topic>();

        public static async Task<Speaker?> Get(int id, ApplicationDbContext context)
        {
            return await context.Speakers.FindAsync(id);
        }
    }
}