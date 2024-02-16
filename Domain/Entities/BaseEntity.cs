namespace Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; } // Primary key
    public DateTime CreatedAt { get; set; } // Creation date
    public DateTime? UpdatedAt { get; set; } // Update date
    public bool IsDeleted { get; set; } // Soft delete flag
}