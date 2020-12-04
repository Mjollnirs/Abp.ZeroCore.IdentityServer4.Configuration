// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE-IdentityServer4-EntityFramework in the project root for license information.

// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: ClientStore.cs
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
    /// Implementation of IClientStore thats uses Repository.
    /// </summary>
    /// <seealso cref="IClientStore" />
    public class ClientStore : AbpServiceBase, IClientStore
    {
        private readonly IRepository<IdentityServer4vNext.Entities.Client, int> _repository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStore"/> class.
        /// </summary>
        /// <param name="repository">The Repository of Client</param>
        /// <param name="queryableExecuter">The Async Queryable Executer</param>
        public ClientStore(IRepository<IdentityServer4vNext.Entities.Client, int> repository, IAsyncQueryableExecuter queryableExecuter)
        {
            _repository = repository;
            _asyncQueryableExecuter = queryableExecuter;
        }

        /// <summary>
        /// Finds a client by id
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>
        /// The client
        /// </returns>
        [UnitOfWork]
        public virtual async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = await _asyncQueryableExecuter.FirstOrDefaultAsync(_repository.GetAll()
                .Include(x => x.AllowedGrantTypes)
                .Include(x => x.RedirectUris)
                .Include(x => x.PostLogoutRedirectUris)
                .Include(x => x.AllowedScopes)
                .Include(x => x.ClientSecrets)
                .Include(x => x.Claims)
                .Include(x => x.IdentityProviderRestrictions)
                .Include(x => x.AllowedCorsOrigins)
                .Include(x => x.Properties)
                .Where(x => x.ClientId == clientId));

            Logger.DebugFormat("{0} found in database: {1}", clientId, client != null);

            return ObjectMapper.Map<Client>(client);
        }
    }
}
