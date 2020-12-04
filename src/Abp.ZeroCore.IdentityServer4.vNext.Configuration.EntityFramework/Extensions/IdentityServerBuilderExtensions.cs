// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: IdentityServerBuilderExtensions.cs
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

using Abp.IdentityServer4vNext.Services;
using Abp.IdentityServer4vNext.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.IdentityServer4vNext.Extensions
{
    /// <summary>
    /// Extension methods to add Abp Repository support to IdentityServer.
    /// </summary>
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Configures Repository implementation of IClientStore, IResourceStore, and ICorsPolicyService with IdentityServer.
        /// </summary>
        /// <typeparam name="TDbContext">The IAbpConfigurationDbContext to use.</typeparam>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfigurationStore<TDbContext>(this IIdentityServerBuilder builder)
            where TDbContext : IAbpConfigurationDbContext
        {
            builder.AddClientStore<ClientStore>();
            builder.AddResourceStore<ResourceStore>();
            builder.AddCorsPolicyService<CorsPolicyService>();

            return builder;
        }
    }
}
