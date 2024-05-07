namespace Library.Data.Entities;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;

    public DateTime? RowModifiedUtc { get; set; }

    public DateTime? RowCreatedUtc { get; set; }

    public DateTime? RowDeletedUtc { get; set; }
}