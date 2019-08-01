using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ApiScopeClaims
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        [Display(Name = "Api Scope Id")]
        public int ApiScopeId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Type { get; set; }
        #endregion


    }
}
