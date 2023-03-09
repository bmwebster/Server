using System.Threading.Tasks;
using Server.GraphQL.Data;
using HotChocolate;

namespace Server.GraphQL
{
    public class Mutation
    {
        public async Task<AddSpeakerPayload> AddSpeakerAsync(
            AddSpeakerInput input,
            [Service] ApplicationDbContext context)
        {

            var topics = input.topics.Select(t => new Topic{Name = t.Name}).ToList();

            var speaker = new Speaker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.WebSite,
                Topics = topics
            };

            context.Speakers.Add(speaker);
            await context.SaveChangesAsync();

            return new AddSpeakerPayload(speaker);
        }
    }
}