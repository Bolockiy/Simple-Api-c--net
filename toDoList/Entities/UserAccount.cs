using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using toDoList.Security;

namespace toDoList.Entities.UserAccount
{
    [Table("useraccount")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }

        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(500)]
        public string? Password { get; set; }

        public int? Role { get; set; } = 0;

        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public void UpdateDate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
