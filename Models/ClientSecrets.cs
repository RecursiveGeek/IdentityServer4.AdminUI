using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientSecrets
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }

        [StringLength(2000, MinimumLength = 0)]
        public string Description { get; set; }

        public Nullable<DateTime> Expiration { get; set; }

        [Display(Name = "Secret Type")]
        [StringLength(250, MinimumLength = 0)]
        public string Type { get; set; }

        [Display(Name = "Client Secret")]
        [StringLength(2000, MinimumLength = 0)]
        public string Value { get; set; }
        #endregion

    }
}

