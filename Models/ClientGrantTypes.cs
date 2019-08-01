using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientGrantTypes
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Required]
        [Display(Name = "Grant Type")]
        [StringLength(250, MinimumLength = 0)]
        public string GrantType { get; set; }
        #endregion
    }
}
