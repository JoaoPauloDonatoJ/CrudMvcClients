using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
