using System.ComponentModel.DataAnnotations;


namespace IdentityServer4.AdminUI.Models
{
    public class IdentityClaims
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Identity Resource Id")]
        public int IdentityResourceId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Type { get; set; }
        #endregion
    }
}
