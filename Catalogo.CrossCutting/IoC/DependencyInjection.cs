using Catalogo.Application.Interfaces;
using Catalogo.Application.Mappings;
using Catalogo.Application.Services;
using Catalogo.Application.UoW;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Connection;
using Catalogo.Infrastructure.Context;
using Catalogo.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Catalogo.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSingleton<IConfiguration>(configuration)
                .AddTransient<IConnectionFactory, ConnectionFactory>()
                .AddScoped<ICategoriaRepository, CategoriaRepository>()
                .AddScoped<IProdutoRepository, ProdutoRepository>()
                .AddScoped<IProdutoService, ProdutoService>()
                .AddScoped<ICategoriaService, CategoriaService>()
                .AddScoped<UnitOfWorkRepository>()
                .AddScoped<IUnitOfWorkRepository>(x => x.GetRequiredService<UnitOfWorkRepository>())
                .AddScoped<IUnitOfWork>(x => x.GetRequiredService<UnitOfWorkRepository>());

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
