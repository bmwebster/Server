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

        private IQueryable<T> queryById<T>(string id, Func<object, IQueryable<T>> query, IIdSerializer serializer) {
            IdValue idv = serializer.Deserialize(id);
            if (idv.TypeName == typeof(T).Name) {
                return query(idv.Value);
            } else {
                return Enumerable.Empty<T>().AsQueryable();
            }
        }

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Speaker> GetSpeakerById([ID] string id, [Service] IIdSerializer serializer, ApplicationDbContext context) {
            return queryById(id, (idv) => context.Speakers.Where(s => s.SpeakerId.Equals(idv)), serializer);
        }            

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<Topic> GetTopicById([ID] string id, [Service] IIdSerializer serializer, ApplicationDbContext context) {
            return queryById(id, (idv) => context.Topics.Where(s => s.TopicId.Equals(idv)), serializer);
        }            

        [UseFirstOrDefault]
        [UseProjection]
        public IQueryable<IRelayNode> GetNode([ID] string id, [Service] IIdSerializer serializer, ApplicationDbContext context) {

            var queries = new List<IQueryable<IRelayNode>>{
                GetSpeakerById(id, serializer, context),
                GetTopicById(id, serializer, context)
            };

            return queries.Find(q => q.Count() > 0) ?? Enumerable.Empty<IRelayNode>().AsQueryable();
        }

    }
}