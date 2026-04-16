using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "O nome é obrigatório.")]
        //[StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        //[Required(ErrorMessage = "O email é obrigatório.")]
        //[EmailAddress(ErrorMessage = "O email deve ser um endereço de email válido.")]
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Ativo { get; set; } = true;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public DateTime? DataExclusao { get; set; }

        public bool Excluido { get; set; } = false;

        public Client Client { get; set; } //Navegação para a entidade Client

        public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>(); //Navegação para a entidade UserProfile

    }
}
