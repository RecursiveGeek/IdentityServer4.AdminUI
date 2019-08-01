using System.IO;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4.AdminUI.Models
{
    public static class ConfigurationManager
    {
        #region field
        public static IConfiguration AppSetting { get; }
        #endregion
        #region constructor
        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
        #endregion

    }
}