using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using DietMaker.Model;
using System.Text;
using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

namespace DietMaker.API
{
    public class NutritionixClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _appId;
        private readonly string _apiKey;
        private const string BaseUrl = "https://trackapi.nutritionix.com/v2";

        public NutritionixClient(string appId, string apiKey)
        {
            _httpClient = new HttpClient();
            _appId = appId;
            _apiKey = apiKey;
        }

        public async Task<CalorieModel> FetchMealDataAsync(string mealName)
        {
            try
            {
                var requestContent = new StringContent(
                    JsonSerializer.Serialize(new { query = mealName }),
                    Encoding.UTF8,
                    "application/json"
                );

                var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/natural/nutrients")
                {
                    Headers =
                    {
                        { "x-app-id", _appId },
                        { "x-app-key", _apiKey }
                    },
                    Content = requestContent
                };

                // Await the asynchronous send
                var response = await _httpClient.SendAsync(request);
                // Await the content read
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var nutritionixResponse = JsonSerializer.Deserialize<NutritionixResponseModel>(responseBody);

                    if (nutritionixResponse?.foods != null && nutritionixResponse.foods.Count > 0)
                    {
                        var firstFood = nutritionixResponse.foods[0];
                        return new CalorieModel(
                            productName: firstFood.food_name,
                            carbs: (uint)Math.Round(firstFood.nf_total_carbohydrate),
                            fats: (uint)Math.Round(firstFood.nf_total_fat),
                            proteins: (uint)Math.Round(firstFood.nf_protein),
                            calories: (uint)firstFood.nf_calories
                        );
                    }
                    Console.ReadKey();
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]No data found for the specified meal.[/]");
                }

                return null;
            }
            catch (HttpRequestException e)
            {
                AnsiConsole.MarkupLine($"[red]Connection error:[/] {Markup.Escape(e.Message)}");
                return null;
            }
            catch (JsonException e)
            {
                AnsiConsole.MarkupLine($"[red]Data parsing error:[/] {Markup.Escape(e.Message)}");
                return null;
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Unexpected error:[/] {Markup.Escape(e.Message)}");
                return null;
            }
        }
    }
}