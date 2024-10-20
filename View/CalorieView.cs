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

        public string DisplayMenu(DateTime selected_date)
        {
            AnsiConsole.Clear();

            DisplayLogo();
            AnsiConsole.Write(new Rule("[yellow]Main Menu[/]").RuleStyle("grey"));

            Console.WriteLine(selected_date.ToShortDateString());

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What would you like to do?")
            .AddChoices(new[] { "Select Day", "Add Meal", "View Entries", "Select Tracking", "Exit" }));

            return choice;
        }

        public string DisplayTracking()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Tracking Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Dayly", "Weekly", "Monthly", "Return" }));

            return choice;
        }

        public string SelectDayScreen()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What day do you want to chose?")
            .AddChoices(new[] {"Tommorow", "Yeasterday", "Today", "Select Date", "Return"}));

            return choice;
        }

        public string AddMeal()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Meal Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "My Meals", "Meal Database", "Enter Macro", "Return" }));

            return choice;
        }

        public void DisplayEnterMacro()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Macro Entry Menu[/]").RuleStyle("grey"));
            /*
                        int calories = AnsiConsole.Ask<int>("Enter calories:");
                        int carbs = AnsiConsole.Ask<int>("Enter carbs:");
                        int fats = AnsiConsole.Ask<int>("Enter fats:");
                        int protein = AnsiConsole.Ask<int>("Enter protein:");*/

            Console.ReadKey();
        }

        public CalorieModel AddEntry()
        {
            AnsiConsole.Clear();

            string productName = AnsiConsole.Ask<string>("Enter product name:");
            int calories = AnsiConsole.Ask<int>("Enter number of calories:");

            return new CalorieModel(productName, calories);
        }

        public void ShowEntries(List<CalorieModel> entries)
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
