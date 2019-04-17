using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace patient.demography.helpers
{
    public static class extensions
    {
        public static bool IsTrulyEmpty(this string inputstring)
        {
            return (inputstring ?? "").Trim().Length <= 0;
        }

        public static string TrulyTrim(this string inputstring)
        {
            return (inputstring ?? "").Trim();
        }

        public static byte[] ToByteArray(this string inputstring)
        {
            return Convert.FromBase64String(inputstring);
        }

        public static string RemoveSpace(this string inputstring)
        {
            return inputstring.Replace(' ', '_');
        }

        public static IServiceCollection AddHttpContextAccessor(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<HttpContextAccessor, HttpContextAccessor>();
            return services;
        }
    }
}