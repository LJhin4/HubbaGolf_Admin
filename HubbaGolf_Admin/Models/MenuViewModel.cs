using HubbaGolfAdmin.Database.Dtos;

namespace HubbaGolf_Admin.Models
{
    public class MenuViewModel
    {
        public List<MenuDto>? MenuAdmin { get; set; }
        public List<CategoryDto>? MenuCategory { get; set; }
        public List<CategoryDto>? MenuLocation { get; set; }
    }
}
