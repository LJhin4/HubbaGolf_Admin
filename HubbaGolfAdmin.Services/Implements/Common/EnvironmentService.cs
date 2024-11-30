using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
namespace HubbaGolfAdmin.Services.Implements
{
    public class EnvironmentService(HostBuilderContext hostingEnvironment, IConfiguration configuration)
    {
        private readonly IConfiguration _Config = configuration;

        private readonly HostBuilderContext _hostingEnvironment = hostingEnvironment;

        /// <summary>
        /// If it is a server, the url has /admin added
        /// </summary>
        /// <param name="relativePath">Normal path when uploading files</param>
        /// <returns>path has /admin added if server </returns>
        public string GetUrl(string relativePath)
        {
            if (relativePath == null)
            {
                return string.Empty;
            }
            var env = _hostingEnvironment.HostingEnvironment;
            return env.EnvironmentName != "Development" ? "" + relativePath : relativePath;
        }

        public string GetApiUrl(string relativePath)
        {
            var env = _hostingEnvironment.HostingEnvironment;
            return env.EnvironmentName != "Development"
                ? _Config.GetSection("UploadFiles:Api")?.Value + relativePath
                : "only server can view";
        }

        public string GetEbhFile(string relativePath)
        {
            var env = _hostingEnvironment.HostingEnvironment;
            return env.EnvironmentName != "Development"
                ? _Config.GetSection("LinkEcoi")?.Value + "/" + relativePath
                : relativePath;
        }
    }
}