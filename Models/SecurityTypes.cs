using System.Collections.Generic;

namespace IdentityServer4.AdminUI.Models
{
    public class SecurityTypes : List<string>
    {
        public SecurityTypes()
        {
        var Delimiter = ConfigurationManager.AppSetting["Custom:Delimiter"];
        // dependency injection to get the settings, just like startup does
        var SecurityTypes = ConfigurationManager.AppSetting["Custom:SecretTypes"];
            var array = SecurityTypes.Split(Delimiter);
            for (var i = 0; i < array.Length; i++)
            {
               Add(array[i]);
            }
        }


    }
}
