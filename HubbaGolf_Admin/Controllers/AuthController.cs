using Microsoft.AspNetCore.Mvc;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Database;

namespace HubbaGolf_Admin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _AuthService;
        private readonly SessionStore _SessionStore;

        public AuthController(
            IAuthService authService,
            SessionStore sessionStore)
        {
            _AuthService = authService;
            _SessionStore = sessionStore;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/log-in")]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (_SessionStore.IsLogged)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(password))
            {
                var zResult = await _AuthService.AuthenticateAsync(username, password);
                if (zResult.Success)
                {
                    _SessionStore.UserLogged = zResult.Data;
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Message = zResult.Message;
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return View("~/Views/Auth/Login.cshtml");
        }
        public async Task<IActionResult> ChangePass(string passold, string passnew1, string passnew2)
        {
            if (!_SessionStore.IsLogged)
            {
                return View("~/Views/Auth/Login.cshtml");
            }
            var auth = await _AuthService.AuthenticateAsync(_SessionStore.UserLogged.UserName, passold);
            if (!auth.Success)
                return BadRequest("Old password is incorrect!");

            var result = await _AuthService.ChangePassAsync(_SessionStore.UserLogged.Id, passnew1);
            return result.Success ? Ok() : BadRequest(result.Message);
        }
    }
}
