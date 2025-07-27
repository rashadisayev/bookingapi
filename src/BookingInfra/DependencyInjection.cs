namespace BookingInfra;

using BookingApp.Common.Contracts;
using BookingInfra.Services;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddBookingInfra(this IServiceCollection services)
    {
        services.AddSingleton<IInMemoryHomeStore , InMemoryHomeStore >();

        return services;
    }
}

