using System.ComponentModel.DataAnnotations;

namespace SmartStock.Application.Profile.DTO_s
{
    public class UpdateProfileDto
    {
        [MaxLength(50)]
        public string? Title { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
        [MaxLength(50)]
        public string? Phone { get; set; }
        public string? ProfilePic { get; set; }
    }
}
