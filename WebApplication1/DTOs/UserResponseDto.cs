namespace WebApplication1.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public List<string> Profiles { get; set; } = new List<string>();

    }
}
