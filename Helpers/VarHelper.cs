using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.AdminUI.Helpers
{
    public static class VarHelper
    {
        // This is a string for the path to the 404 page, If this is to change, it will change all the paths.
        public static string error404 = "/views/shared/404.cshtml";
        // session names. - if the session names are to be custom changed. 
        // Note: These will need to be changed accordingly on the view pages as the function to call these does not have a getstring. 
        public static string IdentityName = "IdentityResourceName";
        public static string IdentityId = "IdentityResourceId";
    }
}
