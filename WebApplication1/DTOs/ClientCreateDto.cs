using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class ClientCreateDto
    {
        //[Required(ErrorMessage = "O nome é obrigatório.")]
        //[StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        //[Required(ErrorMessage = "O email é obrigatório.")]
        //[EmailAddress(ErrorMessage = "O email deve ser um endereço de email válido.")]
        public string Email { get; set; }
    }
}
