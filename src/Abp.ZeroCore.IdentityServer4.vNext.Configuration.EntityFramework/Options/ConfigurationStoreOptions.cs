// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: ConfigurationStoreOptions.cs
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

namespace Abp.IdentityServer4vNext.Options
{
    /// <summary>
    /// Options for configuring the configuration context.
    /// </summary>
    public class ConfigurationStoreOptions
    {
        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>
        /// <value>
        /// The default schema.
        /// </value>
        public string DefaultSchema { get; set; } = null;

        /// <summary>
        /// Gets or sets the identity resource table configuration.
        /// </summary>
        /// <value>
        /// The identity resource.
        /// </value>
        public TableConfiguration IdentityResource { get; set; } = new TableConfiguration("IdentityResources");
        /// <summary>
        /// Gets or sets the identity claim table configuration.
        /// </summary>
        /// <value>
        /// The identity claim.
        /// </value>
        public TableConfiguration IdentityClaim { get; set; } = new TableConfiguration("IdentityClaims");

        /// <summary>
        /// Gets or sets the API resource table configuration.
        /// </summary>
        /// <value>
        /// The API resource.
        /// </value>
        public TableConfiguration ApiResource { get; set; } = new TableConfiguration("ApiResources");
        /// <summary>
        /// Gets or sets the API secret table configuration.
        /// </summary>
        /// <value>
        /// The API secret.
        /// </value>
        public TableConfiguration ApiSecret { get; set; } = new TableConfiguration("ApiSecrets");
        /// <summary>
        /// Gets or sets the API scope table configuration.
        /// </summary>
        /// <value>
        /// The API scope.
        /// </value>
        public TableConfiguration ApiScope { get; set; } = new TableConfiguration("ApiScopes");
        /// <summary>
        /// Gets or sets the API claim table configuration.
        /// </summary>
        /// <value>
        /// The API claim.
        /// </value>
        public TableConfiguration ApiClaim { get; set; } = new TableConfiguration("ApiClaims");
        /// <summary>
        /// Gets or sets the API scope claim table configuration.
        /// </summary>
        /// <value>
        /// The API scope claim.
        /// </value>
        public TableConfiguration ApiScopeClaim { get; set; } = new TableConfiguration("ApiScopeClaims");

        /// <summary>
        /// Gets or sets the client table configuration.
        /// </summary>
        /// <value>
        /// The client.
        /// </value>
        public TableConfiguration Client { get; set; } = new TableConfiguration("Clients");
        /// <summary>
        /// Gets or sets the type of the client grant table configuration.
        /// </summary>
        /// <value>
        /// The type of the client grant.
        /// </value>
        public TableConfiguration ClientGrantType { get; set; } = new TableConfiguration("ClientGrantTypes");
        /// <summary>
        /// Gets or sets the client redirect URI table configuration.
        /// </summary>
        /// <value>
        /// The client redirect URI.
        /// </value>
        public TableConfiguration ClientRedirectUri { get; set; } = new TableConfiguration("ClientRedirectUris");
        /// <summary>
        /// Gets or sets the client post logout redirect URI table configuration.
        /// </summary>
        /// <value>
        /// The client post logout redirect URI.
        /// </value>
        public TableConfiguration ClientPostLogoutRedirectUri { get; set; } = new TableConfiguration("ClientPostLogoutRedirectUris");
        /// <summary>
        /// Gets or sets the client scopes table configuration.
        /// </summary>
        /// <value>
        /// The client scopes.
        /// </value>
        public TableConfiguration ClientScopes { get; set; } = new TableConfiguration("ClientScopes");
        /// <summary>
        /// Gets or sets the client secret table configuration.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public TableConfiguration ClientSecret { get; set; } = new TableConfiguration("ClientSecrets");
        /// <summary>
        /// Gets or sets the client claim table configuration.
        /// </summary>
        /// <value>
        /// The client claim.
        /// </value>
        public TableConfiguration ClientClaim { get; set; } = new TableConfiguration("ClientClaims");
        /// <summary>
        /// Gets or sets the client IdP restriction table configuration.
        /// </summary>
        /// <value>
        /// The client IdP restriction.
        /// </value>
        public TableConfiguration ClientIdPRestriction { get; set; } = new TableConfiguration("ClientIdPRestrictions");
        /// <summary>
        /// Gets or sets the client cors origin table configuration.
        /// </summary>
        /// <value>
        /// The client cors origin.
        /// </value>
        public TableConfiguration ClientCorsOrigin { get; set; } = new TableConfiguration("ClientCorsOrigins");
        /// <summary>
        /// Gets or sets the client property table configuration.
        /// </summary>
        /// <value>
        /// The client property.
        /// </value>
        public TableConfiguration ClientProperty { get; set; } = new TableConfiguration("ClientProperties");
    }
}
