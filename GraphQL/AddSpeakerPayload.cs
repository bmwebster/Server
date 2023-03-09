using Server.GraphQL.Data;

namespace Server.GraphQL
{
    public class AddSpeakerPayload
    {
        public AddSpeakerPayload(Speaker speaker)
        {
            Speaker = speaker;
        }

        public Speaker Speaker { get; }
    }
}