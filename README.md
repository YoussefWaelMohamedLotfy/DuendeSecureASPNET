# Duende IdentityServer v6 Secure for ASP.NET 7

This repo is a reference for Duende IdentityServer QuickStarts

## Commands for Migrating IdentityServer's `DbContext`s

+ PersistedGrantDbContext: `Add-Migration <migration-name> -Context PersistedGrantDbContext -OutputDir "Data\Migrations\PersistedGrant"`
+ ConfigurationDbContext: `Add-Migration <migration-name> -Context ConfigurationDbContext -OutputDir "Data\Migrations\Configuration"`
+ ApplicationDbContext: `Add-Migration <migration-name> -Context ApplicationDbContext -OutputDir "Data\Migrations\Application"`
