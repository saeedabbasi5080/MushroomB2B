namespace MushroomB2B.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    protected void SetModified() => ModifiedAt = DateTime.UtcNow;
    public void SoftDelete() => IsDeleted = true;
}
