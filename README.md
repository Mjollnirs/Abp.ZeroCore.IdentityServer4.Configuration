# Abp.ZeroCore.IdentityServer4.Configuration

[![Build status](https://ci.appveyor.com/api/projects/status/5482r5ukh7jio2it?svg=true)](https://ci.appveyor.com/project/Mjollnirs/abp-zerocore-identityserver4-configuration)

```csharp
    [DependsOn(typeof(AbpZeroCoreIdentityServer4ConfigurationModule))]
    public class OpenIdCoreModule : AbpModule {
        
    }
```

```csharp
    [DependsOn(
        typeof(OpenIdCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule),
        typeof(AbpZeroCoreIdentityServer4ConfigurationModule))]
    public class OpenIdEntityFrameworkModule : AbpModule {
    }
```

```csharp
using Abp.IdentityServer4;
using Abp.IdentityServer4.Entities;
using Abp.IdentityServer4.Extensions;

public class OpenIdDbContext : AbpZeroDbContext<Tenant, Role, User, OpenIdDbContext>, IAbpConfigurationDbContext {
        public DbSet<Client> Clients { get; set; }

        public DbSet<IdentityResource> IdentityResources { get; set; }

        public DbSet<ApiResource> ApiResources { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureConfigurationContext();

            base.OnModelCreating(modelBuilder);
        }
}
```

InitialHostDbBuilder.cs
```csharp
new DefaultIdentityServerConfigCreator(_context).Create();
```

Startup.cs
```csharp
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            IdentityRegistrar.Register(services);
            AuthConfigurer.Configure(services, _appConfiguration);
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(_appConfiguration["Authentication:IdentityServer:File"],
                    _appConfiguration["Authentication:IdentityServer:Password"]))
                .AddAbpPersistedGrants<IAbpPersistedGrantDbContext>()
                .AddConfigurationStore<IAbpConfigurationDbContext>()
                .AddAbpIdentityServer<User>();
        }
```

```csharp
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseIdentityServer();
        }
```
