using System.Collections.Generic;

namespace IdentityServer4.AdminUI.Models
{
    public class GrantTypes : List<string>
    {
        public GrantTypes()
        {
            var Delimiter = ConfigurationManager.AppSetting["Custom:Delimiter"];
            #region fields
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
