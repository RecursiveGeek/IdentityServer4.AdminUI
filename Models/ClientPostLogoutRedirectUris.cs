using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientPostLogoutRedirectUris
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Post Logout Redirect Uri")]
        [StringLength(2000, MinimumLength = 0)]
        public string PostLogoutRedirectUri { get; set; }
        #endregion



    }
}
