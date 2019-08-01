using System.Collections.Generic;

namespace IdentityServer4.AdminUI.Models
{
    public class GrantTypes : List<string>
    {
        public GrantTypes()
        {
            #region fields
            var GrantType = ConfigurationManager.AppSetting["GrantTypes"];
            var array = GrantType.Split('|');
            for (int i = 0; i < array.Length; i++)
            {
                Add(array[i]);
            }
            #endregion
        }

    }
}
