using Catalogo.Application.Interfaces;
using Catalogo.Application.Mappings;
using Catalogo.Application.Services;
using Catalogo.Application.UoW;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Context;
using Catalogo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
                .AddScoped<ICategoriaRepository, CategoriaRepository>()
                .AddScoped<IProdutoRepository, ProdutoRepository>()
                .AddScoped<IProdutoService, ProdutoService>()
                .AddScoped<ICategoriaService, CategoriaService>()
                .AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>()
                .AddScoped<IUnitOfWork, UnitOfWorkRepository>();

            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));

            return services;
        }
    }
}
