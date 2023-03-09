using Server.GraphQL.Data;

namespace Server.GraphQL
{
    public class Query
    {
        // Note - must have no [Service] annotation on context, so it can be provided by
        // the `RegisterDbContext` call in Program.cs. See 
        // https://chillicream.com/docs/hotchocolate/v12/integrations/entity-framework#registerdbcontext
        [UseProjection]
        public IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;

        [UseProjection]
        public IQueryable<Topic> GetTopics(ApplicationDbContext context) =>
            context.Topics;

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Speaker> GetSpeakerById([ID] int id, ApplicationDbContext context) {
            return context.Speakers.Where(s => s.SpeakerId.Equals(id));
        }            

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Topic> GetTopicById([ID] int id, ApplicationDbContext context) {
            return context.Topics.Where(s => s.TopicId.Equals(id));
        }
    }
}