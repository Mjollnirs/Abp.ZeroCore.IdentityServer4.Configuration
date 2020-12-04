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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.EntityFrameworkCore;

namespace Abp.IdentityServer4vNext.Stores
{
    /// <summary>
    /// Implementation of IResourceStore thats uses Repository.
    /// </summary>
    /// <seealso cref="IResourceStore" />
    public class ResourceStore : AbpServiceBase, IResourceStore
    {
        private readonly IRepository<IdentityServer4vNext.Entities.ApiResource, int> _apiResourceRepository;
        private readonly IRepository<IdentityServer4vNext.Entities.IdentityResource, int> _identityResourceRepository;
        private readonly IRepository<IdentityServer4vNext.Entities.ApiScope, int> _apiScopeRepository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceStore"/> class.
        /// </summary>
        /// <param name="apiResourceRepository">The Repository of ApiResource</param>
        /// <param name="identityResourceRepository">The Repository of IdentityResource</param>
        /// <param name="queryableExecuter">The Async Queryable Executer</param>
        /// <param name="apiScopeRepository"></param>
        public ResourceStore(IRepository<IdentityServer4vNext.Entities.ApiResource, int> apiResourceRepository,
            IRepository<IdentityServer4vNext.Entities.IdentityResource, int> identityResourceRepository,
            IAsyncQueryableExecuter queryableExecuter, IRepository<IdentityServer4vNext.Entities.ApiScope, int> apiScopeRepository)
        {
            _apiResourceRepository = apiResourceRepository;
            _identityResourceRepository = identityResourceRepository;
            _asyncQueryableExecuter = queryableExecuter;
            _apiScopeRepository = apiScopeRepository;
        }


        [UnitOfWork]
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var resources = await _asyncQueryableExecuter.ToListAsync(_identityResourceRepository.GetAll()
                .Include(x => x.UserClaims)
                .Where(x => names.Contains(x.Name)));

            Logger.DebugFormat("Found {0} identity scopes in database", resources.Select(x => x.Name));

            return ObjectMapper.Map<IEnumerable<IdentityResource>>(resources);
        }

        [UnitOfWork]
        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var scopes = await _apiScopeRepository.GetAll().Include(c => c.UserClaims)
                .Where(c => scopeNames.Contains(c.Name)).ToListAsync();
            return ObjectMapper.Map<IEnumerable<ApiScope>>(scopes);
        }

        [UnitOfWork]
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var names = scopeNames.ToArray();

            var api = await _asyncQueryableExecuter.ToListAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Where(x => x.Scopes.Any(y => names.Contains(x.Name))));

            Logger.DebugFormat("Found {0} API scopes in database", api.SelectMany(x => x.Scopes).Select(x => x.Name));

            return ObjectMapper.Map<IEnumerable<ApiResource>>(api);
        }

        [UnitOfWork]
        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var apis = await _asyncQueryableExecuter.ToListAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims)
                .Where(x => apiResourceNames.Contains(x.Name)));


            return ObjectMapper.Map<IEnumerable<ApiResource>>(apis);
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<global::IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            var idientities =
                await _asyncQueryableExecuter.ToListAsync(_identityResourceRepository.GetAll()
                    .Include(x => x.UserClaims));

            var apis = await _asyncQueryableExecuter.ToListAsync(_apiResourceRepository.GetAll()
                .Include(x => x.Secrets)
                .Include(x => x.Scopes)
                .ThenInclude(s => s.UserClaims)
                .Include(x => x.UserClaims));

            var scopes = await _apiScopeRepository.GetAll().Include(c => c.UserClaims).ToListAsync();
            var result = new global::IdentityServer4.Models.Resources(
                ObjectMapper.Map<IEnumerable<IdentityResource>>(idientities),
                ObjectMapper.Map<IEnumerable<ApiResource>>(apis),
                ObjectMapper.Map<IEnumerable<ApiScope>>(scopes));
            return result;
        }
    }
}
