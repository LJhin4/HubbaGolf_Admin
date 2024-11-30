using Microsoft.AspNetCore.Mvc;

namespace HubbaGolf_Admin.ViewComponents
{
    [ViewComponent(Name = "Header")]
    public class HeaderViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
