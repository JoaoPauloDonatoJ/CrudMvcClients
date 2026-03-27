using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class ClientCreateDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
