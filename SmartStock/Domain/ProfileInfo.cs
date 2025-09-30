using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartStock.Domain
{
    [Table("profiles")]
    public class ProfileInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }

        [Column(TypeName = "longtext")]
        public string? ProfilePic { get; set; }

        // FK back to account (1:1)
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
