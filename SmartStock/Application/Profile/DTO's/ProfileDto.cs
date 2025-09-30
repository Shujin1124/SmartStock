using System.ComponentModel.DataAnnotations;

namespace SmartStock.Application.Profile.DTO_s
{
    public class ProfileDto
    {
        // Account info
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedAt { get; set; }

        // Profile info
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePic { get; set; }
    }

   
}
