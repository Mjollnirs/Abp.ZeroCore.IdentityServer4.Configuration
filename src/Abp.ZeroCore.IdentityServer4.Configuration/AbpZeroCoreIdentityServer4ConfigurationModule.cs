// Project: Abp.ZeroCore.IdentityServer4.Configuration
// File: AbpZeroCoreIdentityServer4ConfigurationModule.cs
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
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using AutoMapper;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Abp.IdentityServer4
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    public class AbpZeroCoreIdentityServer4ConfigurationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<Entities.ApiResource, ApiResource>(MemberList.Destination)
                    .ConstructUsing(src => new ApiResource())
                    .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                    .ReverseMap();

                config.CreateMap<Entities.ApiResourceClaim, string>()
                    .ConstructUsing(x => x.Type)
                    .ReverseMap()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ApiSecret, Secret>(MemberList.Destination)
                    .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                    .ReverseMap();

                config.CreateMap<Entities.ApiScope, Scope>(MemberList.Destination)
                    .ConstructUsing(src => new Scope())
                    .ReverseMap();

                config.CreateMap<Entities.ApiScopeClaim, string>()
                    .ConstructUsing(x => x.Type)
                    .ReverseMap()
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));


                config.CreateMap<Entities.ClientProperty, KeyValuePair<string, string>>()
                    .ReverseMap();

                config.CreateMap<Entities.Client, Client>()
                    .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                    .ReverseMap();

                config.CreateMap<Entities.ClientCorsOrigin, string>()
                    .ConstructUsing(src => src.Origin)
                    .ReverseMap()
                    .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientIdPRestriction, string>()
                    .ConstructUsing(src => src.Provider)
                    .ReverseMap()
                    .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientClaim, Claim>(MemberList.None)
                    .ConstructUsing(src => new Claim(src.Type, src.Value))
                    .ReverseMap();

                config.CreateMap<Entities.ClientScope, string>()
                    .ConstructUsing(src => src.Scope)
                    .ReverseMap()
                    .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientPostLogoutRedirectUri, string>()
                    .ConstructUsing(src => src.PostLogoutRedirectUri)
                    .ReverseMap()
                    .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientRedirectUri, string>()
                    .ConstructUsing(src => src.RedirectUri)
                    .ReverseMap()
                    .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientGrantType, string>()
                    .ConstructUsing(src => src.GrantType)
                    .ReverseMap()
                    .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

                config.CreateMap<Entities.ClientSecret, Secret>(MemberList.Destination)
                    .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                    .ReverseMap();


                config.CreateMap<Entities.IdentityResource, IdentityResource>(MemberList.Destination)
                    .ConstructUsing(src => new IdentityResource())
                    .ReverseMap();

                config.CreateMap<Entities.IdentityClaim, string>()
                   .ConstructUsing(x => x.Type)
                   .ReverseMap()
                   .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AbpZeroCoreIdentityServer4ConfigurationModule).GetAssembly());
        }
    }
}