﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Prism.Ioc
{
    public static class IServiceProviderExtensions
    {
        public static IServiceProvider CreateServiceProvider(this IContainerProvider container, IServiceCollection services)
        {
            var containerRegistry = container as IContainerRegistry;
            RegisterTypesWithPrismContainer(containerRegistry, services);
            var serviceProvider = container is IServiceProvider sp ? sp : new PrismServiceProvider(container);

            if (!containerRegistry.IsRegistered<IServiceProvider>())
            {
                containerRegistry.RegisterInstance<IServiceProvider>(serviceProvider);
            }

            return serviceProvider;
        }

        private static void RegisterTypesWithPrismContainer(IContainerRegistry containerRegistry, IServiceCollection services)
        {
            foreach (var service in services)
            {
                switch (service.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (service.ImplementationType != null)
                            containerRegistry.RegisterSingleton(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationInstance != null)
                            containerRegistry.RegisterInstance(service.ServiceType, service.ImplementationInstance);
                        else if (service.ImplementationFactory != null)
                            containerRegistry.RegisterInstance(service.ServiceType, service.ImplementationFactory(containerRegistry as IServiceProvider));
                        break;
                    case ServiceLifetime.Transient:
                        if (service.ImplementationType != null)
                            containerRegistry.Register(service.ServiceType, service.ImplementationType);
                        else if (service.ImplementationInstance != null)
                            throw new NotSupportedException($"You cannot provide an instance for a Transient Service '{service.ServiceType.FullName}'");
                        else if (service.ImplementationFactory != null)
                            containerRegistry.Register(service.ServiceType, service.ImplementationFactory);
                        break;
                    default:
                        throw new NotSupportedException($"We do not currently support Transient Lifetimes from IServiceCollection '{service.ServiceType.FullName}'");
                }
            }
        }
    }
}
