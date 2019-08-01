using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientCorsOrigins
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 0)]
        public string Origin { get; set; }
        #endregion
    }
}
