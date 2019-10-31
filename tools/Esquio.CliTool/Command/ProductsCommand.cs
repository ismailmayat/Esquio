﻿using Esquio.CliTool.Internal;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;

namespace Esquio.CliTool.Command
{
    [Command(Constants.ProductsCommandName, Description = Constants.ProductsDescriptionCommandName),
        Subcommand(typeof(AddCommand)),
        Subcommand(typeof(RemoveCommand)),
        Subcommand(typeof(ListCommand))]
    internal class ProductsCommand
    {
        private int OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine(Constants.SpecifySubCommandErrorMessage);

            app.ShowHelp(usePager: true);
            return 1;
        }

        private class AddCommand
        {
            [Option("--name <NAME>", Description = "The product name to be added.")]
            [Required]
            public string Name { get; set; }

            [Option("--description <DESCRIPTION>", Description = "The product description to be added.")]
            [Required]
            public string Description { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                    apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.AddProductAsync(Name, Description);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine($"The product {Name} was added succesfully.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class RemoveCommand
        {
            [Option("--product-id <PRODUCT-ID>", Description = "The product id to delete.")]
            public int Id { get; set; }

            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                    uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                    apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.RemoveProductAsync(Id);

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine($"The product with identifier {Id} was removed succesfully.");
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }

        private class ListCommand
        {
            [Option(Constants.UriParameter, Description = Constants.UriDescription)]
            public string Uri { get; set; }

            [Option(Constants.ApiKeyParameter, Description = Constants.ApiKeyDescription)]
            public string ApiKey { get; set; }

            private async Task<int> OnExecute(IConsole console)
            {
                var defaultForegroundColor = console.ForegroundColor;

                using (var client = EsquioClient.Create(
                     uri: Uri ?? Environment.GetEnvironmentVariable(Constants.UriEnvironmentVariable) ?? Constants.UriDefaultValue,
                     apikey: ApiKey ?? Environment.GetEnvironmentVariable(Constants.ApiKeyEnvironmentVariable)))
                {
                    var response = await client.ListProductsAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        console.ForegroundColor = Constants.SuccessColor;
                        console.WriteLine(await response.GetContentDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 0;
                    }
                    else
                    {
                        console.ForegroundColor = Constants.ErrorColor;
                        console.WriteLine(await response.GetErrorDetailAsync());
                        console.ForegroundColor = defaultForegroundColor;

                        return 1;
                    }
                }
            }
        }
    }
}
