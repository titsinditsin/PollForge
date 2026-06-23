using PollForge.Domain.Enums;

namespace PollForge.Domain.Entities;

public class Poll
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid AuthorId { get; private set; }
    public PollType Type { get; private set; }
    public PollStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ClosesAt { get; private set; }

    private readonly List<PollOption> _options = [];
    public IReadOnlyList<PollOption> Options => _options.AsReadOnly();

    private Poll() { }

    public static Poll Create(string title, string? description, Guid authorId, PollType type, DateTimeOffset? closesAt)
    {
        return new Poll
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            AuthorId = authorId,
            Type = type,
            Status = PollStatus.Draft,
            CreatedAt = DateTimeOffset.UtcNow,
            ClosesAt = closesAt
        };
    }

    public void AddOption(string text)
    {
        var option = PollOption.Create(Id, text, _options.Count + 1);
        _options.Add(option);
    }

    public void Activate()
    {
        if (Status != PollStatus.Draft)
            throw new InvalidOperationException("Only draft polls can be activated.");

        if (_options.Count < 2)
            throw new InvalidOperationException("A poll must have at least two options to be activated.");

        Status = PollStatus.Active;
    }

    public void Close()
    {
        if (Status != PollStatus.Active)
            throw new InvalidOperationException("Only active polls can be closed.");

        Status = PollStatus.Closed;
    }
}
