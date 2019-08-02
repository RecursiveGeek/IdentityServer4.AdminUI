namespace IdentityServer4.AdminUI.Helpers
{
    public static class VarHelper
    {
        // This is a string for the path to the 404 page, If this is to change, it will change all the paths.
        public static string error404 = Models.ConfigurationManager.AppSetting["Custom:Error404Url"];
        // session names. - if the session names are to be custom changed. 
        // Note: These will need to be changed accordingly on the view pages as the function to call these does not have a getstring. 
        public static string IdentityName = Models.ConfigurationManager.AppSetting["Custom:SecretNames:IdentityName"];
        public static string IdentityId = Models.ConfigurationManager.AppSetting["Custom:SecretNames:IdentityId"];
        public static string ClientId = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ClientId"];
        public static string ClientName = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ClientName"];
        public static string ApiResourceName = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ApiResourceName"];
        public static string ApiResourceId = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ApiResourceId"];
        public static string ApiScopeName = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ApiScopeName"];
        public static string ApiScopeId = Models.ConfigurationManager.AppSetting["Custom:SecretNames:ApiScopeId"];
        
    }
}
