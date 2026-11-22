using Microsoft.AspNetCore.Identity;

namespace ArticleProject.Domain.Aggregates
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public long? ImageId { get; set; }
        public UserImage? Image { get; set; }

        public long? ResearcherId { get; set; }
        public Researcher? Researcher { get; set; }

        public long? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

        public void UpdateImage(UserImage image)
        {
            Image = image;
        }

        public void UpdateResearcher(long researcherId)
        {
            ResearcherId = researcherId;
        }
    }
}