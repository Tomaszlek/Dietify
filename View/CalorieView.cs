using Spectre.Console;
using DietMaker.Model;
using System.Collections.Generic;

namespace DietMaker.View
{
    public class CalorieView
    {

        public void DisplayLogo()
        {
            AnsiConsole.Write(new FigletText("Diet Maker").Color(Color.Green).Centered());
        }

        public string DisplayMenu()
        {
            AnsiConsole.Clear();

            DisplayLogo();
            AnsiConsole.Write(new Rule("[yellow]Main Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What would you like to do?").AddChoices(new[] { "Add Entry", "View Entries", "Total Calories", "Exit" }));

            return choice;
        }

        public CalorieEntry AddEntry()
        {
            AnsiConsole.Clear();

            string productName = AnsiConsole.Ask<string>("Enter product name:");
            int calories = AnsiConsole.Ask<int>("Enter number of calories:");

            return new CalorieEntry(productName, calories);
        }

        public void ShowEntries(List<CalorieEntry> entries)
        {
            AnsiConsole.Clear();

            var table = new Table();
            table.AddColumn("Product Name");
            table.AddColumn("Calories");

            foreach (var entry in entries)
            {
                table.AddRow(entry.ProductName, entry.Calories.ToString());
            }

            AnsiConsole.Write(table);
            Console.ReadKey();
        }

        public void ShowTotalCalories(int totalCalories)
        {
            AnsiConsole.Clear();

            AnsiConsole.MarkupLine($"[bold]Total Calories: {totalCalories}[/]");

            Console.ReadKey();
        }

        public void ExitProgram()
        {
            AnsiConsole.MarkupLine("[red]Exiting program...[/]");
            
        }
    }
}
