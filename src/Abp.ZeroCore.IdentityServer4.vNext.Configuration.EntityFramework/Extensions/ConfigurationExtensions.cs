// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE-IdentityServer4-EntityFramework in the project root for license information.

// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: ConfigurationExtensions.cs
// 
// Copyright 2018 Mjollnir<mjollnir@59k.org>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Abp.IdentityServer4vNext.Entities;
using Abp.IdentityServer4vNext.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Abp.IdentityServer4vNext.Extensions
{
    /// <summary>
    /// Extension methods to define the database schema for the configuration and operational data stores.
    /// </summary>
    public static class ConfigurationExtensions
    {
        private static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder,
            TableConfiguration configuration, string prefix = null)
            where TEntity : class
        {
            prefix = prefix ?? "Abp";

            return string.IsNullOrWhiteSpace(configuration.Schema)
                ? entityTypeBuilder.ToTable(prefix + configuration.Name)
                : entityTypeBuilder.ToTable(prefix + configuration.Name, configuration.Schema);
        }

        public static void ConfigureConfigurationContext(this ModelBuilder modelBuilder,
            ConfigurationStoreOptions storeOptions = null, string prefix = null)
        {
            if (storeOptions == null) storeOptions = new ConfigurationStoreOptions();
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema))
                modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<Client>(client =>
            {
                client.ToTable(storeOptions.Client, prefix);
                client.HasKey(x => x.Id);

                client.Property(x => x.ClientId).HasMaxLength(200).IsRequired();
                client.Property(x => x.ProtocolType).HasMaxLength(200).IsRequired();
                client.Property(x => x.ClientName).HasMaxLength(200);
                client.Property(x => x.ClientUri).HasMaxLength(2000);
                client.Property(x => x.LogoUri).HasMaxLength(2000);
                client.Property(x => x.Description).HasMaxLength(1000);
                client.Property(x => x.FrontChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.BackChannelLogoutUri).HasMaxLength(2000);
                client.Property(x => x.ClientClaimsPrefix).HasMaxLength(200);
                client.Property(x => x.PairWiseSubjectSalt).HasMaxLength(200);

                client.HasIndex(x => x.ClientId).IsUnique();

                client.HasMany(x => x.AllowedGrantTypes).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.RedirectUris).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.PostLogoutRedirectUris).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.AllowedScopes).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.ClientSecrets).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.Claims).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.IdentityProviderRestrictions).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.AllowedCorsOrigins).WithOne(x => x.Client).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                client.HasMany(x => x.Properties).WithOne(x => x.Client).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ClientGrantType>(grantType =>
            {
                grantType.ToTable(storeOptions.ClientGrantType, prefix);
                grantType.Property(x => x.GrantType).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ClientRedirectUri>(redirectUri =>
            {
                redirectUri.ToTable(storeOptions.ClientRedirectUri, prefix);
                redirectUri.Property(x => x.RedirectUri).HasMaxLength(2000).IsRequired();
            });

            modelBuilder.Entity<ClientPostLogoutRedirectUri>(postLogoutRedirectUri =>
            {
                postLogoutRedirectUri.ToTable(storeOptions.ClientPostLogoutRedirectUri, prefix);
                postLogoutRedirectUri.Property(x => x.PostLogoutRedirectUri).HasMaxLength(2000).IsRequired();
            });

            modelBuilder.Entity<ClientScope>(scope =>
            {
                scope.ToTable(storeOptions.ClientScopes, prefix);
                scope.Property(x => x.Scope).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ClientSecret>(secret =>
            {
                secret.ToTable(storeOptions.ClientSecret, prefix);
                secret.Property(x => x.Value).HasMaxLength(2000).IsRequired();
                secret.Property(x => x.Type).HasMaxLength(250).IsRequired();
                secret.Property(x => x.Description).HasMaxLength(2000);
            });

            modelBuilder.Entity<ClientClaim>(claim =>
            {
                claim.ToTable(storeOptions.ClientClaim, prefix);
                claim.Property(x => x.Type).HasMaxLength(250).IsRequired();
                claim.Property(x => x.Value).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ClientIdPRestriction>(idPRestriction =>
            {
                idPRestriction.ToTable(storeOptions.ClientIdPRestriction, prefix);
                idPRestriction.Property(x => x.Provider).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ClientCorsOrigin>(corsOrigin =>
            {
                corsOrigin.ToTable(storeOptions.ClientCorsOrigin, prefix);
                corsOrigin.Property(x => x.Origin).HasMaxLength(150).IsRequired();
            });

            modelBuilder.Entity<ClientProperty>(property =>
            {
                property.ToTable(storeOptions.ClientProperty, prefix);
                property.Property(x => x.Key).HasMaxLength(250).IsRequired();
                property.Property(x => x.Value).HasMaxLength(2000).IsRequired();
            });

            modelBuilder.Entity<IdentityResource>(identityResource =>
            {
                identityResource.ToTable(storeOptions.IdentityResource, prefix).HasKey(x => x.Id);

                identityResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                identityResource.Property(x => x.DisplayName).HasMaxLength(200);
                identityResource.Property(x => x.Description).HasMaxLength(1000);

                identityResource.HasIndex(x => x.Name).IsUnique();

                identityResource.HasMany(x => x.UserClaims).WithOne(x => x.IdentityResource).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<IdentityClaim>(claim =>
            {
                claim.ToTable(storeOptions.IdentityClaim, prefix).HasKey(x => x.Id);

                claim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });


            modelBuilder.Entity<ApiResource>(apiResource =>
            {
                apiResource.ToTable(storeOptions.ApiResource, prefix).HasKey(x => x.Id);

                apiResource.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiResource.Property(x => x.DisplayName).HasMaxLength(200);
                apiResource.Property(x => x.Description).HasMaxLength(1000);

                apiResource.HasIndex(x => x.Name).IsUnique();

                apiResource.HasMany(x => x.Secrets).WithOne(x => x.ApiResource).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.Scopes).WithOne(x => x.ApiResource).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                apiResource.HasMany(x => x.UserClaims).WithOne(x => x.ApiResource).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiSecret>(apiSecret =>
            {
                apiSecret.ToTable(storeOptions.ApiSecret, prefix).HasKey(x => x.Id);

                apiSecret.Property(x => x.Description).HasMaxLength(1000);
                apiSecret.Property(x => x.Value).HasMaxLength(2000).IsRequired();
                apiSecret.Property(x => x.Type).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<ApiResourceClaim>(apiClaim =>
            {
                apiClaim.ToTable(storeOptions.ApiClaim, prefix).HasKey(x => x.Id);

                apiClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });

            modelBuilder.Entity<ApiScope>(apiScope =>
            {
                apiScope.ToTable(storeOptions.ApiScope, prefix).HasKey(x => x.Id);

                apiScope.Property(x => x.Name).HasMaxLength(200).IsRequired();
                apiScope.Property(x => x.DisplayName).HasMaxLength(200);
                apiScope.Property(x => x.Description).HasMaxLength(1000);

                apiScope.HasIndex(x => x.Name).IsUnique();

                apiScope.HasMany(x => x.UserClaims).WithOne(x => x.ApiScope).IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ApiScopeClaim>(apiScopeClaim =>
            {
                apiScopeClaim.ToTable(storeOptions.ApiScopeClaim, prefix).HasKey(x => x.Id);

                apiScopeClaim.Property(x => x.Type).HasMaxLength(200).IsRequired();
            });
        }
    }
}