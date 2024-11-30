#nullable disable
using HubbaGolfAdmin.Database;

namespace HubbaGolfAdmin.Middlewares
{
    public class AuthenMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _configuration { get; }
        #region [prop]
        List<string> _NoLoginList;
        List<string> NoLoginList
        {
            get
            {
                if (_NoLoginList == null || _NoLoginList.Count == 0)
                {
                    _NoLoginList = _configuration.GetSection("AllowNoLoginLink").Get<List<string>>();
                }

                return _NoLoginList;
            }
        }
        #endregion
        public AuthenMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var zSessionStoreSrv = context.RequestServices.GetService<SessionStore>();

            var excludedPaths = new[] { "/Booking/AddEvent", "/Booking/GetEvent", "/swagger/index.html", "/Website/GetListArticleByCategoryId", "/Website/GetAllLocation", "/Website/GetArticleById", "/Website/GetMenuHeader", "/Website/getCourseByCountryId" };

            if (excludedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context);
                return;
            }

            if (zSessionStoreSrv.IsLogged == false)
            {
                var zPath = context.Request.Path;
                var zAllowList = NoLoginList;
                if (zAllowList is null || zAllowList.Count == 0)
                {
                    throw new Exception("AllowNoLoginLink must be set in appsetting");
                }

                var IsNoLogin = zAllowList.Contains(zPath.Value);

                if (IsNoLogin == false)
                {
                    string zPathBase = context.Request.PathBase;
                    string zRedirectUrl = $"{zPathBase}/log-in";
                    context.Response.Redirect(zRedirectUrl);
                    return;
                }
            }

            await _next(context);
        }
    }
}
