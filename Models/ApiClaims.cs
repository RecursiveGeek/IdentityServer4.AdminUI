using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ApiClaims
    {
        #region Properties
        public int Id { get; set; }
        [Display(Name = "Api Resource Id")]
        public int ApiResourceId { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Type { get; set; }
        #endregion
    }
}
