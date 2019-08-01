using System;

namespace IdentityServer4.AdminUI.Models
{
    public class ErrorViewModel
    {
        #region Properties
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        #endregion
    }
}