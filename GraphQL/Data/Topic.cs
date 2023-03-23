using System.ComponentModel.DataAnnotations;

namespace Server.GraphQL.Data
{
    [Node(IdField = nameof(TopicId))]
    public class Topic
    {
        public int TopicId { get; set; }

        // FIXME in c#11 we can use `required string` and remove `[Required]` annotation
        [Required]
        [StringLength(200)]
        public string? Name { get; set; }

        [NodeResolver]
        [UseSingleOrDefault]
        [UseProjection]
        public static IQueryable<Topic> GetTopic(int id, ApplicationDbContext context)
        {
            return context.Topics.Where(a => a.TopicId == id);
        }

        // public static async Task<Topic?> Get(int id, ApplicationDbContext context)
        // {
        //     return await context.Topics.FindAsync(id);
        // }

    }
}