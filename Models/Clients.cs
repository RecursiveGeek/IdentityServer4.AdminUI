using System;
using System.ComponentModel.DataAnnotations;


namespace IdentityServer4.AdminUI.Models
{
    public class Clients
    {
        #region Properties
        // below are all of the attributes from the clients table 
        public int Id { get; set; }
        [Display(Name = "Absolute Refresh Token Lifetime")]
        public int AbsoluteRefreshTokenLifetime { get; set; }
        [Display(Name = "Access Token Lifetime")]
        public int AccessTokenLifetime { get; set; }
        [Display(Name = "Access Token Type")]
        public int AccessTokenType { get; set; }
        [Display(Name = "Allow Access Tokens Via Browser")]
        public Boolean AllowAccessTokensViaBrowser { get; set; }
        [Display(Name = "Allow Offline Access")]
        public Boolean AllowOfflineAccess { get; set; }
        [Display(Name = "Allow Plain Text Pkce")]
        public Boolean AllowPlainTextPkce { get; set; }
        [Display(Name = "Allow Remember Consent")]
        public Boolean AllowRememberConsent { get; set; }
        [Display(Name = "Always Include User Claims In Id Token")]
        public Boolean AlwaysIncludeUserClaimsInIdToken { get; set; }
        [Display(Name = "Always Send Client Claims ")]
        public Boolean AlwaysSendClientClaims { get; set; }
        [Display(Name = "Authorization Code Lifetime")]
        public int AuthorizationCodeLifetime { get; set; }
        [Display(Name = "Back Channel Logout Session Required")]
        public Boolean BackChannelLogoutSessionRequired { get; set; }
        [StringLength(2000, MinimumLength = 0)]
        [Display(Name = "Back Channel Logout Uri")]
        public string BackChannelLogoutUri { get; set; }
        [Display(Name = "Client Claims Prefix")]
        [StringLength(200, MinimumLength = 0)]
        public string ClientClaimsPrefix { get; set; }
        [Display(Name = "Client Id")]
        [StringLength(50, MinimumLength = 3)]
        [Required]
        public string ClientId { get; set; }
        [Display(Name = "Client Name")]
        [StringLength(200, MinimumLength = 0)]
        public string ClientName { get; set; }
        [Display(Name = "Client Uri")]
        [StringLength(2000, MinimumLength = 0)]
        public string ClientUri { get; set; }
        [Display(Name = "Consent Lifetime")]
        public Nullable<int> ConsentLifetime { get; set; }
        [Display(Name = "Description")]
        [StringLength(1000, MinimumLength = 0)]
        public string Description { get; set; }
        [Display(Name = "Enable Local Login ")]
        public Boolean EnableLocalLogin { get; set; }
        [Display(Name = "Enabled")]
        public Boolean Enabled { get; set; }
        [Display(Name = "Front Channel Logout Session Required")]
        public Boolean FrontChannelLogoutSessionRequired { get; set; }
        [Display(Name = "Front Channel Logout Uri")]
        [StringLength(2000, MinimumLength = 0)]
        public string FrontChannelLogoutUri { get; set; }
        [Display(Name = "Identity Token Lifetime")]
        public int IdentityTokenLifetime { get; set; }
        [Display(Name = "Include Jwt Id")]
        public Boolean IncludeJwtId { get; set; }
        [Display(Name = "Logo Uri")]
        [StringLength(2000, MinimumLength = 0)]
        public string LogoUri { get; set; }
        [Display(Name = "Pair Wise Subject Salt")]
        [StringLength(200, MinimumLength = 0)]
        public string PairWiseSubjectSalt { get; set; }
        [Display(Name = "Protocol Type")]
        [StringLength(200, MinimumLength = 0)]
        [Required]
        public string ProtocolType { get; set; }
        [Display(Name = "Refresh Token Expiration")]
        public int RefreshTokenExpiration { get; set; }
        [Display(Name = "Refresh Token Usage")]
        public int RefreshTokenUsage { get; set; }
        [Display(Name = "Require Client Secret")]
        public Boolean RequireClientSecret { get; set; }
        [Display(Name = "Require Consent")]
        public Boolean RequireConsent { get; set; }
        [Display(Name = "Require Pkce")]
        public Boolean RequirePkce { get; set; }
        [Display(Name = "Sliding Refresh Token Lifetime")]
        public int SlidingRefreshTokenLifetime { get; set; }
        [Display(Name = "Update Access Token Claims On Refresh")]
        public Boolean UpdateAccessTokenClaimsOnRefresh { get; set; }
        #endregion

    }
}
