// Project: Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework
// File: DefaultIdentityServerConfigCreator.cs
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
using Abp.Dependency;
using Abp.ObjectMapping;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Linq;

namespace Abp.ZeroCore.IdentityServer4.Configuration.EntityFramework.Seed
{
    public class DefaultIdentityServerConfigCreator
    {
        private readonly IAbpConfigurationDbContext _context;
        public IObjectMapper ObjectMapper { get; set; }

        public DefaultIdentityServerConfigCreator(IAbpConfigurationDbContext dbContext)
        {
            _context = dbContext;
            ObjectMapper = IocManager.Instance.Resolve<IObjectMapper>();
        }

        public void Create()
        {
            if (_context.ApiResources.Count() == 0)
                _context.ApiResources.AddRange(ObjectMapper.Map<IList<Entities.ApiResource>>(new List<ApiResource>()
                {
                    new ApiResource("default-api", "Default (all) API") { ApiSecrets = new List<Secret>(){ new Secret("secret") } },
                    new ApiResource("api1", "API 1") { ApiSecrets = new List<Secret>(){ new Secret("secret") } },
                }));

            if (_context.IdentityResources.Count() == 0)
                _context.IdentityResources.AddRange(ObjectMapper.Map<List<Entities.IdentityResource>>(new List<IdentityResource>()
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Profile(),
                    new IdentityResources.Email(),
                    new IdentityResources.Phone()
                }));

            if (_context.Clients.Count() == 0)
                _context.Clients.AddRange(ObjectMapper.Map<List<Entities.Client>>(new List<Client>()
                {
                    new Client()
                    {
                        ClientId = "client",
                        AllowedGrantTypes = GrantTypes.ClientCredentials.Union(GrantTypes.ResourceOwnerPassword).ToList(),
                        AllowedScopes = {"default-api"},
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        }
                    }
                }));
        }
    }
}
