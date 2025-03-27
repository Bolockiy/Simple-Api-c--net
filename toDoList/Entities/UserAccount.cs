using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toDoList.Entities.UserAccount
{
    [Table("UserAccount")]
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }

        [MaxLength(255)]
        public string? FullName { get; set; }

        [MaxLength(500)]
        public string? Password { get; set; }

        public int? Role { get; set; } = 0;

    }
}
