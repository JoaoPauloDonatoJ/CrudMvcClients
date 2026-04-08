using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.ViewModels
{
    public class ClientEditViewModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }
        public bool Ativo { get; set; }

        public bool IsAdmin { get; set; }
        public List<SelectListItem> AllProfiles { get; set; } = new List<SelectListItem>();
        public List<int> SelectedProfileIds { get; set; } = new List<int>();

    }
}
