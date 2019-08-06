﻿using System.IO;
using Microsoft.Extensions.Configuration;

namespace IdentityServer4.AdminUI.Models
{
    public static class ConfigurationManager
    {
        #region Fields
        public static IConfiguration AppSetting { get; }
        #endregion

        #region Constructors
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