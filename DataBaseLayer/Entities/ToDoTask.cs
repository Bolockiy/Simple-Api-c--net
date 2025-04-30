using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiToDo.Domain.Entities
{
    [Table("tasks")]
    public class ToDoTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; } = null;

        public DateTime? UpdatedAt { get; set; } = null;

        public void Update(string? title, string? description, bool? isCompleted)
        {
            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            if (!string.IsNullOrWhiteSpace(description))
                Description = description;

            if (isCompleted.HasValue)
            {
                IsCompleted = isCompleted.Value;
                CompletedAt = IsCompleted ? DateTime.UtcNow : null;
            }

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
