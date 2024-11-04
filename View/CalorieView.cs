using Spectre.Console;
using DietMaker.Model;
using System.Collections.Generic;
using SixLabors.ImageSharp.Processing;


namespace DietMaker.View
{
    public class CalorieView
    {

        public void DisplayLogo(DateTime selectedDate)
        {
            AnsiConsole.Write(new FigletText("Diet Maker")
                .Color(Color.Green));
            AnsiConsole.Write(new Rule("[yellow]Welcome to Diet Maker[/]"));

            AnsiConsole.Write(new Markup($"[bold yellow]Selected Date:[/] [bold yellow]{selectedDate.ToString()}[/]"));
            AnsiConsole.WriteLine();
        }

        // Enhanced Main Menu with better styling and layout
        public string DisplayMenu(DateTime selectedDate, UserDTO userDTO, UserModel user, Dictionary<string, List<CalorieModel>> mealData)
        {
            AnsiConsole.Clear();
            DisplayLogo(selectedDate);

            DayTracker(selectedDate, userDTO, user, mealData);

            AnsiConsole.Cursor.SetPosition(0, Console.WindowHeight - 19);

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]Choose an option:[/]")
                .PageSize(6)
                .HighlightStyle("cyan bold")
                .AddChoices("Select Day", "Add Meal", "View Entries", "Select Tracking", "Options", "Exit"));

            return choice;
        }

        // Enhanced Tracking Menu with more colors and layout
        public string DisplayTrackingMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Tracking Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]Choose a tracking period:[/]")
                .HighlightStyle("cyan bold")
                .AddChoices("Daily", "Weekly", "Monthly", "Return"));

            return choice;
        }

        // Better-styled Options Menu
        public string OptionsMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Options Menu[/]").RuleStyle("grey"));

            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]Options:[/]")
                .HighlightStyle("cyan bold")
                .AddChoices("Set Your Goal", "Save Data", "Return"));

            return choice;
        }

        // Goal Setting Menu with user-friendly feedback
        public UserDTO SetYourGoal(UserDTO userDTO, UserModel user)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Goal Setting[/]").RuleStyle("grey"));

            AnsiConsole.MarkupLine($"[green bold]Looking Good, {user.UserName}![/]");
            AnsiConsole.MarkupLine($"[bold]Current Goals[/]: Carbs: {userDTO.Carbs}, Fats: {userDTO.Fats}, Proteins: {userDTO.Proteins}, Calories: {userDTO.Calories}\n");

            userDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]Set Your Goal:[/]")
                .HighlightStyle("cyan bold")
                .AddChoices("User Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard"));

            return userDTO;
        }

        // Enhanced Day Tracker with colored bar charts
        public void DayTracker(DateTime selectedDate, UserDTO userDTO, UserModel user, Dictionary<string, List<CalorieModel>> mealData)
        {
            // Check if there are entries for the selected date
            if (!mealData.TryGetValue(selectedDate.ToShortDateString(), out List<CalorieModel> entries))
            {
                AnsiConsole.MarkupLine($"[red]No entries found for {selectedDate.ToShortDateString()}[/]");
                return;
            }

            uint totalCarbs = 0, totalFats = 0, totalProteins = 0, totalCalories = 0;

            foreach (var entry in entries)
            {
                totalCarbs += entry.Carbs;
                totalFats += entry.Fats;
                totalProteins += entry.Proteins;
                totalCalories += entry.Calories;
            }

            AnsiConsole.Cursor.SetPosition(0, Console.WindowHeight - 9);

            DisplayProgressBar("Carbs", totalCarbs, user.CarbsGoal, Color.Green);
            DisplayProgressBar("Fats", totalFats, user.FatsGoal, Color.Yellow);
            DisplayProgressBar("Proteins", totalProteins, user.ProteinsGoal, Color.Blue);
            DisplayProgressBar("Calories", totalCalories, user.CaloriesGoal, Color.Red);

            AnsiConsole.Cursor.SetPosition(0, 0);
        }

        private void DisplayProgressBar(string label, uint total, uint goal, Color color)
        {
            float percentage = goal == 0 ? 0 : (float)total / goal;
            percentage = Math.Clamp(percentage, 0, 1);

            int totalBarWidth = 150;
            int barWidth = (int)(percentage * totalBarWidth);

            string paddedLabel = $"{label}".PadRight(15);

            // Ustawienie wykresu z etykietą zawierającą emotikonę
            var barChart = new BarChart()
                .Width(totalBarWidth)
                .Label(paddedLabel)
                .LeftAlignLabel()
                .WithMaxValue(100);

            barChart.AddItem("", Math.Round((double)percentage * 100, 2), color);

            AnsiConsole.Write(barChart);
        }

        public string SelectDayScreen()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection Menu[/]").RuleStyle("grey"));

            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What day do you want to choose?[/]")
                .AddChoices("Tomorrow", "Yesterday", "Today", "Select Date", "Return")
                .HighlightStyle(new Style(Color.Green)));
        }

        public DateTime SelectDateScreen(DateTime date)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection By Date[/]").RuleStyle("grey"));

            var calendar = new Calendar(date.Year, date.Month).Centered();
            calendar.AddCalendarEvent(date.Year, date.Month, date.Day);
            AnsiConsole.Write(calendar);

            return AnsiConsole.Ask<DateTime>("[bold]Enter Date (yyyy-MM-dd):[/]");
        }

        public string AddMeal()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Meal Menu[/]").RuleStyle("grey"));

            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What are you interested in?[/]")
                .AddChoices("My Meals", "Meal Database", "Enter Macro", "Return")
                .HighlightStyle(new Style(Color.Green)));
        }

        public UserDTO DisplayEnterMacro(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Macro Entry Menu[/]").RuleStyle("grey"));

            AnsiConsole.Write(new Text($"Carbs = {UserDTO.Carbs} | Fats = {UserDTO.Fats} | Proteins = {UserDTO.Proteins} | Calories = {UserDTO.Calories}").Centered());

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What are you interested in?[/]")
                .AddChoices("Carbs", "Fats", "Proteins", "Calories", "Apply/Discard")
                .HighlightStyle(new Style(Color.Green)));

            return UserDTO;
        }

        public string ApplyDiscard()
        {
            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]Do you want to apply?[/]")
                .AddChoices("Yes", "No")
                .HighlightStyle(new Style(Color.Green)));
        }

        public uint EnterUint(string text)
        {
            return AnsiConsole.Ask<uint>(text);
        }

        public void Error(string message)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Error[/]").RuleStyle("grey"));
            AnsiConsole.Write(new Text(message).Centered());
            Console.ReadKey();
        }

        public float EnterFloat(string text)
        {
            return AnsiConsole.Ask<float>(text);
        }

        public string EnterString(string text)
        {
            return AnsiConsole.Ask<string>(text);
        }

        public string ViewEntries(List<CalorieModel> entries)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entries View[/]").RuleStyle("grey"));

            var table = new Table();
            table.AddColumn("Num.");
            table.AddColumn("Product Name");
            table.AddColumn("Carbs");
            table.AddColumn("Fats");
            table.AddColumn("Proteins");
            table.AddColumn("Calories");

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                table.AddRow(i.ToString(), entry.ProductName, entry.Carbs.ToString(), entry.Fats.ToString(), entry.Proteins.ToString(), entry.Calories.ToString());
            }

            AnsiConsole.Write(table);

            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What is Your Intent?[/]")
                .AddChoices("Make Some Changes", "Return")
                .HighlightStyle(new Style(Color.Green)));
        }

        public UserDTO ModifyEntry(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entry Modification Menu[/]").RuleStyle("grey"));

            AnsiConsole.Write(new Text($"Product Name = {UserDTO.ProductName} | Carbs = {UserDTO.Carbs} | Fats = {UserDTO.Fats} | Proteins = {UserDTO.Proteins} | Calories = {UserDTO.Calories}").Centered());

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What are you interested in?[/]")
                .AddChoices("Product Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard")
                .HighlightStyle(new Style(Color.Green)));

            return UserDTO;
        }

        public UserDTO AddEntry(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Manual Entry Adding Menu[/]").RuleStyle("grey"));

            AnsiConsole.Write(new Text($"Product Name = {UserDTO.ProductName} | Carbs = {UserDTO.Carbs} | Fats = {UserDTO.Fats} | Proteins = {UserDTO.Proteins} | Calories = {UserDTO.Calories}").Centered());

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]What are you interested in?[/]")
                .AddChoices("Product Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard")
                .HighlightStyle(new Style(Color.Green)));

            return UserDTO;
        }

        public string EditEntries(List<CalorieModel> entries)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entry Modification Menu[/]").RuleStyle("grey"));

            return AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[bold]How Do You Want To Modify?[/]")
                .AddChoices("Edit Entry", "Remove Entry", "Return")
                .HighlightStyle(new Style(Color.Green)));
        }

        public void DisplayMealSearchError(string mealName)
        {
            AnsiConsole.MarkupLine($"[red]Could not find meal '{mealName}'. Try again later or use a different search term.[/]");
        }

        public void DisplayMealAdded(CalorieModel mealData)
        {
            AnsiConsole.MarkupLine($"[green]Added meal: {mealData.ProductName}, calories: {mealData.Calories} kcal[/]");
            Console.ReadKey();
        }

        private void DisplayMealDetails(CalorieModel meal)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Meal Details[/]").RuleStyle("grey"));

            if (meal != null)
            {
                AnsiConsole.MarkupLine($"[green]Meal Name:[/] {meal.ProductName}");
                AnsiConsole.MarkupLine($"[green]Calories:[/] {meal.Calories}");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]No meal details available to display.[/]");
            }
        }

        public void DisplayDataSaved()
        {
            AnsiConsole.MarkupLine("[green]Data has been successfully saved.[/]");
        }

        public void DisplayGoalUpdated()
        {
            AnsiConsole.MarkupLine("[green]Your goal has been successfully updated.[/]");
        }

        public void DisplayExitMessage()
        {
            AnsiConsole.MarkupLine("[blue]Goodbye.[/]");
        }

        public void ExitProgram()
        {
            AnsiConsole.Clear();
        }
    }
}
