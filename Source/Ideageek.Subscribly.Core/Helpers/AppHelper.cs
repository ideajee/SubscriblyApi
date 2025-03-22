using Microsoft.Extensions.Configuration;

namespace Ideageek.Subscribly.Core.Helpers
{
    public interface IAppHelper
    {
        string GetConnectionString();
    }
    public class AppHelper : IAppHelper
    {
        private readonly IConfiguration _configuration;
        public AppHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnectionString()
        {
            return _configuration.GetConnectionString("SubscriblyConnection");
        }
    }
}