// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE-IdentityServer4-EntityFramework in the project root for license information.

// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: ResourceStore.cs
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
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework.Stores
{
    /// <summary>
    /// Implementation of IResourceStore thats uses Repository.
    /// </summary>
    /// <seealso cref="IdentityServer4.Stores.IResourceStore" />
    public class ResourceStore : AbpServiceBase, IResourceStore
    {
        private readonly IRepository<Entities.ApiResource, int> _apiResourceRepository;
        private readonly IRepository<Entities.IdentityResource, int> _identityResourceRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceStore"/> class.
        /// </summary>
        /// <param name="apiResourceRepository">The Repository of ApiResource</param>
        /// <param name="identityResourceRepository">The Repository of IdentityResource</param>
        /// <param name="queryableExecuter">The Async Queryable Executer</param>
        public ResourceStore(IRepository<Entities.ApiResource, int> apiResourceRepository,
            IRepository<Entities.IdentityResource, int> identityResourceRepository,
            IAsyncQueryableExecuter queryableExecuter)
        {
            _apiResourceRepository = apiResourceRepository;
            _identityResourceRepository = identityResourceRepository;
            _asyncQueryableExecuter = queryableExecuter;
        }

        /// <summary>
        /// Finds the API resource by name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<ApiResource> FindApiResourceAsync(string name)
        {
            var api = await _asyncQueryableExecuter.FirstOrDefaultAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Where(x => x.Name == name));

            if (api != null)
            {
                Logger.DebugFormat("Found {0} API resource in database", name);
            }
            else
            {
                Logger.DebugFormat("Did not find {0} API resource in database", name);
            }

            return ObjectMapper.Map<ApiResource>(api);
        }

        /// <summary>
        /// Gets API resources by scope name.
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var api = await _asyncQueryableExecuter.ToListAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Where(x => x.Scopes.Where(y => names.Contains(x.Name)).Any()));

            Logger.DebugFormat("Found {0} API scopes in database", api.SelectMany(x => x.Scopes).Select(x => x.Name));

            return ObjectMapper.Map<IEnumerable<ApiResource>>(api);
        }

        /// <summary>
        /// Gets identity resources by scope name.
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var resources = await _asyncQueryableExecuter.ToListAsync(_identityResourceRepository.GetAll()
                .Include(x => x.UserClaims)
                .Where(x => names.Contains(x.Name)));

            Logger.DebugFormat("Found {0} identity scopes in database", resources.Select(x => x.Name));

            return ObjectMapper.Map<IEnumerable<IdentityResource>>(resources);
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<global::IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            var idientities = await _asyncQueryableExecuter.ToListAsync(_identityResourceRepository.GetAll().Include(x => x.UserClaims));

            var apis = await _asyncQueryableExecuter.ToListAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                    .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims));

            var result = new global::IdentityServer4.Models.Resources(ObjectMapper.Map<IEnumerable<IdentityResource>>(idientities), ObjectMapper.Map<IEnumerable<ApiResource>>(apis));

            Logger.DebugFormat("Found {0} as all scopes in database", result.IdentityResources.Select(x => x.Name).Union(result.ApiResources.SelectMany(x => x.Scopes).Select(x => x.Name)));

            return result;
        }
    }
}
