using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DietMaker.Model;
using Spectre.Console;

namespace DietMaker
{
    public class MealDataStorage
    {
        private readonly string _filePath;

        public MealDataStorage(string filePath)
        {
            _filePath = filePath;
        }

        // Zapis danych posiłków do pliku JSON
        public async Task SaveMealsAsync(Dictionary<string, List<CalorieModel>> meals)
        {
            try
            {
                var json = JsonSerializer.Serialize(meals, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_filePath, json);
                AnsiConsole.MarkupLine("[green]Info:[/]: Data has been successfully saved to JSON file.");
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/]: Failed to save data to JSON file: {Markup.Escape(e.Message)}");
            }
        }

        // Odczyt danych posiłków z pliku JSON
        public async Task<Dictionary<string, List<CalorieModel>>> LoadMealsAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    AnsiConsole.MarkupLine("[yellow]Info:[/] JSON file not found. Returning an empty dictionary.");
                    return new Dictionary<string, List<CalorieModel>>();
                }

                var json = await File.ReadAllTextAsync(_filePath);
                var meals = JsonSerializer.Deserialize<Dictionary<string, List<CalorieModel>>>(json);
                AnsiConsole.MarkupLine("[green]Info:[/] Data has been successfully loaded from JSON file.");
                return meals ?? new Dictionary<string, List<CalorieModel>>();
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Failed to load data from JSON file: {Markup.Escape(e.Message)}");
                return new Dictionary<string, List<CalorieModel>>();
            }
        }
    }
}
