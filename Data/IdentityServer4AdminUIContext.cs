using Microsoft.EntityFrameworkCore;


namespace IdentityServer4.AdminUI.Models
{
    public class IdentityServer4AdminUIContext : DbContext
    {
        public IdentityServer4AdminUIContext (DbContextOptions<IdentityServer4AdminUIContext> options)
            : base(options)
        {
        }

        public DbSet<IdentityServer4.AdminUI.Models.Clients> Clients { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientSecrets> ClientSecrets { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientRedirectUris> ClientRedirectUris { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientScopes> ClientScopes { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientPostLogoutRedirectUris> ClientPostLogoutRedirectUris { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientIdPRestrictions> ClientIdPRestrictions { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientProperties> ClientProperties { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientCorsOrigins> ClientCorsOrigins { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientClaims> ClientClaims { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ClientGrantTypes> ClientGrantTypes { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ApiResources> ApiResources { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ApiScopes> ApiScopes { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ApiScopeClaims> ApiScopeClaims { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ApiClaims> ApiClaims { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.ApiSecrets> ApiSecrets { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.IdentityResources> IdentityResources { get; set; }

        public DbSet<IdentityServer4.AdminUI.Models.IdentityClaims> IdentityClaims { get; set; }

       


    }
}
