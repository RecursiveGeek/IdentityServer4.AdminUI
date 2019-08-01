using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ApiSecrets
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        [Display(Name = "Api Resource Id")]
        public int ApiResourceId { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        public string Description { get; set; }
        public Nullable<DateTime> Expiration { get; set; }
        [Display(Name = "Secret Type")]
        [StringLength(250, MinimumLength = 0)]
        public string Type { get; set; }
        [StringLength(2000, MinimumLength = 0)]
        [Display(Name = "Api Secret")]
        public string Value { get; set; }
        #endregion

    }
}
