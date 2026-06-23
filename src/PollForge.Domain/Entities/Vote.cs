namespace PollForge.Domain.Entities;

public class Vote
{
    public Guid Id { get; private set; }
    public Guid PollId { get; private set; }
    public Guid OptionId { get; private set; }
    public Guid? VoterId { get; private set; }
    public string Fingerprint { get; private set; } = null!;
    public string IpAddress { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    private Vote() { }

    public static Vote Create(Guid pollId, Guid optionId, Guid? voterId, string fingerprint, string ipAddress)
    {
        return new Vote
        {
            Id = Guid.NewGuid(),
            PollId = pollId,
            OptionId = optionId,
            VoterId = voterId,
            Fingerprint = fingerprint,
            IpAddress = ipAddress,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
