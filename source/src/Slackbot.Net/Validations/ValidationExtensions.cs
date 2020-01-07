using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Slackbot.Net.Validations
{
    public static class ValidationExtensions
    {
        internal static IEnumerable<ValueTuple<string, string>> ValidationErrors(this object @this)
        {
            var context = new ValidationContext(@this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(@this, context, results, true);
            foreach (var validationResult in results)
            {
                yield return new ValueTuple<string, string> { Item1 = validationResult.MemberNames.First(), Item2 = validationResult.ErrorMessage};
            }
        }

        internal static IServiceCollection ConfigureAndValidate<T>(this IServiceCollection @this,IConfiguration config) where T : class
            => @this
                .Configure<T>(config)
                .PostConfigure<T>(settings =>
                {
                    var configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        var aggrErrors = string.Join(",", configErrors.Select(e => e.Item2));
                        var count = configErrors.Length;
                        var configType = typeof(T).Name;
                        throw new ConfigurationException(
                            $"Found {count} configuration error(s) in {configType}: {aggrErrors}");
                    }
                });

        internal static IServiceCollection ConfigureAndValidate<T>(this IServiceCollection @this,Action<T> config) where T : class
            => @this
                .Configure<T>(config)
                .PostConfigure<T>(settings =>
                {
                    var configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        var aggrErrors = string.Join(", ", configErrors.Select(e => e.Item2));
                        var count = configErrors.Length;
                        var configType = typeof(T).Name;
                        throw new ConfigurationException(
                            $"Found {count} configuration error(s) in {configType}: {aggrErrors}");
                    }
                });

        internal static IServiceCollection ConfigureAndValidate<T>(this IServiceCollection @this, string name,Action<T> config) where T : class
            => @this
                .Configure<T>(name,config)
                .PostConfigure<T>(name, settings =>
                {
                    var configErrors = settings.ValidationErrors().ToArray();
                    if (configErrors.Any())
                    {
                        var aggrErrors = "";
                        var count = configErrors.Length;
                        var configType = typeof(T).Name;
                        foreach (var error in configErrors)
                        {
                            var obj = settings as T;
                            var val = obj.GetType().GetProperty(error.Item1).GetValue(obj);
                            aggrErrors += $"'{error.Item1}':'{val}' <=> {error.Item2}";
                        }

                        throw new ConfigurationException(
                            $"Found {count} configuration error(s) in {configType}. {aggrErrors}.");
                    }
                });
    }
}