﻿using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.AdminUI.Models
{
    public class ApiResources
    {
        #region Properties
        public int Id { get; set; }
        [StringLength(1000, MinimumLength = 0)]
        public string Description { get; set; }
        [Display(Name = "Display Name")]
        [StringLength(200, MinimumLength = 0)]
        public string DisplayName { get; set; }
        public Boolean Enabled { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 0)]
        public string Name { get; set; }
        #endregion
    }
}
