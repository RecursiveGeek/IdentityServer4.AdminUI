using System.Collections.Generic;

namespace IdentityServer4.AdminUI.Models
{
    public class SecurityTypes : List<string>
    {

        public SecurityTypes()
        {

            // dependency injection to get the settings, just like startup does
            var SecurityTypes = ConfigurationManager.AppSetting["SecretTypes"];
            var array = SecurityTypes.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
               Add(array[i]);
            }
        }


    }
}
