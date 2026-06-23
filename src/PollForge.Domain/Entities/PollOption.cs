namespace PollForge.Domain.Entities;

public class PollOption
{
    public Guid Id { get; private set; }
    public Guid PollId { get; private set; }
    public string Text { get; private set; } = null!;
    public int DisplayOrder { get; private set; }

    private PollOption() { }

    public static PollOption Create(Guid pollId, string text, int displayOrder)
    {
        return new PollOption
        {
            Id = Guid.NewGuid(),
            PollId = pollId,
            Text = text,
            DisplayOrder = displayOrder
        };
    }
}
