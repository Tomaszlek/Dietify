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

            AnsiConsole.WriteLine("Selected day: " + selected_date.ToShortDateString() + "\n");

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

        public DateTime SelectDateScreen(DateTime date)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection By Date[/]").RuleStyle("grey"));

            var calendar = new Calendar(date.Year, date.Month).Centered();
            calendar.AddCalendarEvent(date.Year, date.Month, date.Day);
            AnsiConsole.Write(calendar);

            DateTime choice = AnsiConsole.Ask<DateTime>("Enter Date rrrr.mm.dd: ");

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

        public DTO DisplayEnterMacro(DTO dto)
        {
            

            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Macro Entry Menu[/]").RuleStyle("grey"));

            /*var layout = new Layout("Root").SplitColumns(new Layout("L"), new Layout("R"));
            layout["L"].Update();*/
            AnsiConsole.Cursor.MoveDown();
            Text t = new Text("Carbs = " + dto.Carbs + " | Fats = " + dto.Fats + " | Proteis = " + dto.Proteins + " | Calories = " + dto.Calories).Centered();
            AnsiConsole.Write(t);

            dto.choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }));
            

            //Console.ReadKey();
            return dto;
        }

        public string DisAppMacro()
        {
            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Do you want to apply?")
            .AddChoices(new[] { "Yes", "No" }));

            return choice;
        }

        public int EnterInt(string text)
        {
            int number = AnsiConsole.Ask<int>(text);
            return number;
        }

        public float EnterFloat(string text)
        {
            float number = AnsiConsole.Ask<float>(text);
            return number;
        }

        public string EnterString(string text)
        {
            string str = AnsiConsole.Ask<string>(text);
            return str;
        }

        public CalorieModel AddEntry()
        {
            AnsiConsole.Clear();

            string productName = AnsiConsole.Ask<string>("Enter product name:");
            int calories = AnsiConsole.Ask<int>("Enter number of calories:");

            return new CalorieModel();
        }

        public void ViewEntries(List<CalorieModel> entries)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entries View[/]").RuleStyle("grey"));

            var table = new Table();
            table.AddColumn("Product Name");
            table.AddColumn("Carbs");
            table.AddColumn("Fats");
            table.AddColumn("Proteins");
            table.AddColumn("Calories");

            foreach (var entry in entries)
            {
                table.AddRow(entry.ProductName, entry.Carbs.ToString(), entry.Fats.ToString(), entry.Proteins.ToString(), entry.Calories.ToString());
            }

            AnsiConsole.Write(table);
            Console.ReadKey();
        }

        public void ExitProgram()
        {
            AnsiConsole.MarkupLine("[red]Exiting program...[/]");
            
        }
    }
}
