using Application.UseCases.NotaFiscalCases;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<SaveNotaFiscalUseCase>();
            services.AddScoped<GetNotaFiscalByChaveUseCase>();
            services.AddScoped<DeleteNotaFiscalByChaveUseCase>();
            services.AddScoped<ListNotaFiscalUseCase>();
            return services;
        }
    }
}
