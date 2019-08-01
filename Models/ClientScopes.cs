using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientScopes
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Scope { get; set; }
        #endregion
    }
}
