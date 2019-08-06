using System.Collections.Generic;

namespace IdentityServer4.AdminUI.Models
{
    public class GrantTypes : List<string>
    {
        public GrantTypes()
        {
            #region Fields
            var Delimiter = ConfigurationManager.AppSetting["Custom:Delimiter"];
            var GrantType = ConfigurationManager.AppSetting["Custom:GrantTypes"];
            var array = GrantType.Split(Delimiter);
            for (int i = 0; i < array.Length; i++)
            {
                Add(array[i]);
            }
            #endregion
        }

    }
}
