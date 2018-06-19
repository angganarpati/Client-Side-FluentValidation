using ClientSideFluentValidation.PropertyValidators;
using FluentValidation.AspNetCore;
using FluentValidation.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace ClientSideFluentValidation
{
    public static class StartupExtension
    {
        public static IServiceCollection AddValidation(this IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(
                cfg =>
                {
                    cfg.ConfigureClientsideValidation(x =>
                    {
                        x.Add(typeof(GreaterThanOrEqualValidator), (context, rule, validator) =>
                        new GreatherThanOrEqualPropertyValidator(rule, validator));

                        x.Add(typeof(GreaterThanValidator), (context, rule, validator) =>
                        new GreatherThanPropertyValidator(rule, validator));

                        x.Add(typeof(LessThanValidator), (context, rule, validator) =>
                        new LessThanPropertyValidator(rule, validator));

                        x.Add(typeof(LessThanOrEqualValidator), (context, rule, validator) =>
                       new LessThanOrEqualPropertyValidator(rule, validator));
                    });
                    cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
                }
            );

            return services;
        }
    }
}