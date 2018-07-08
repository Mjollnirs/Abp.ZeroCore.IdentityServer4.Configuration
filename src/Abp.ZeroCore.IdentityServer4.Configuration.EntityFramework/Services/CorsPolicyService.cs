// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE-IdentityServer4-EntityFramework in the project root for license information.

// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: CorsPolicyService.cs
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
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Abp.IdentityServer4.Services
{
    /// <summary>
    /// Implementation of ICorsPolicyService that consults the client configuration in the Repository for allowed CORS origins.
    /// </summary>
    /// <seealso cref="IdentityServer4.Services.ICorsPolicyService" />
    public class CorsPolicyService : AbpServiceBase, ICorsPolicyService
    {
        private readonly IRepository<Entities.Client, int> _repository;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;


        /// <summary>
        /// Initializes a new instance of the <see cref="CorsPolicyService"/> class.
        /// </summary>
        /// <param name="repository">The Repository of Client</param>
        /// <param name="queryableExecuter">The Async Queryable Executer</param>
        public CorsPolicyService(IRepository<Entities.Client, int> repository,
            IAsyncQueryableExecuter queryableExecuter)
        {
            _repository = repository;
            _asyncQueryableExecuter = queryableExecuter;
        }

        /// <summary>
        /// Determines whether origin is allowed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var origins = await _asyncQueryableExecuter.ToListAsync(_repository.GetAll().SelectMany(x => x.AllowedCorsOrigins.Select(y => y.Origin)));
            var distinctOrigins = origins.Where(x => x != null).Distinct();
            var isAllowed = distinctOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);

            Logger.DebugFormat("Origin {0} is allowed: {1}", origin, isAllowed);

            return isAllowed;
        }
    }
}
