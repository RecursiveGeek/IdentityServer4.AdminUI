using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ApiScopes
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        [Display(Name = "Api Resource Id")]
        public int ApiResourceId { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        public string Description { get; set; }
        [Display(Name = "Display Name")]
        [StringLength(200, MinimumLength = 0)]
        public string DisplayName { get; set; }

        public Boolean Emphasize { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Name { get; set; }

        public Boolean Required { get; set; }
        [Display(Name = "Show In Discovery Document")]
        public Boolean ShowInDiscoveryDocument { get; set; }
        #endregion


    }
}
