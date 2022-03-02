using App.Services;
using Cayent.Core.CQRS.Commands;
using Cayent.Core.CQRS.Queries;
using Cayent.Core.CQRS.Services;
using Data.Providers;

namespace BlazorApp.Server
{
    public static class StartupExtension
    {
        public static void RegisterCQRS(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IContainer, DotNetCoreContainer>();
            services.AddScoped<ITenantProvider, DefaultTenantProvider>();
            services.AddScoped<IAppDbContextFactory, DefaultAppDbContextFactory>();
            services.AddHttpContextAccessor();


            RegisterCore(services, configuration);

            //RegisterCommonCQRS(services);

            services.AddCommandQueryHandlers(typeof(ICommandHandler<>));
            services.AddCommandQueryHandlers(typeof(IQueryHandler<,>));
        }

        static void RegisterCore(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISequentialGuidGenerator, DefaultSequentialGuidGenerator>();

            services.AddTransient<ICommandHandlerFactory, DefaultCommandHandlerFactory>();
            services.AddTransient<ICommandHandlerDispatcher, DefaultCommandHandlerDispatcher>();

            services.AddTransient<IQueryHandlerFactory, DefaultQueryHandlerFactory>();
            services.AddTransient<IQueryHandlerDispatcher, QueryHandlerDispatcher>();


        }


        static void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        {
            var handlers = typeof(ChatService).Assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
            );

            foreach (var handler in handlers)
            {
                foreach (var impl in handler.GetInterfaces().Where(e => e.IsGenericType && e.GetGenericTypeDefinition() == handlerInterface))
                {
                    services.AddScoped(impl, handler);
                }


            }
        }

    }

    public class DotNetCoreContainer : IContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public DotNetCoreContainer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        T IContainer.Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
