using Microsoft.AspNetCore.Mvc;
using HubbaGolf_Admin.Models;
using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Services.Interfaces;

namespace HubbaGolf_Admin.ViewComponents
{
    [ViewComponent(Name = "Menu")]
    public class MenuViewComponent : ViewComponent
    {
        private readonly ISystemService _SystemService;
        private readonly IWebService _WebService;
        private readonly SessionStore _SessionStore;
        public MenuViewComponent(ISystemService systemService, IWebService webService, SessionStore sessionStore)
        {
            _SystemService = systemService;
            _WebService = webService;
            _SessionStore = sessionStore;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var zMenuView = new MenuViewModel
            {
                MenuAdmin = await _SystemService.GetListAllMenuAsync(),
                MenuCategory = await _WebService.GetListCategoryAsync(),
                MenuLocation = await _WebService.GetListLocationAsync(),
                Facilities = await _WebService.GetListCategoryByParentIdAsync(24)
            };

            return View(zMenuView);
        }
    }
}
