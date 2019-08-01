﻿using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ClientClaims
    {
        #region Properties
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Client Id")]
        public int ClientId { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 0)]
        public string Type { get; set; }
        [Required]
        [StringLength(250, MinimumLength = 0)]
        public string Value { get; set; }
        #endregion
    }
}
