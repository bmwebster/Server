namespace Server.GraphQL
{
    public record AddSpeakerInput(
        string Name,
        string? Bio,
        string? WebSite,
        List<AddTopicInput> topics);
}