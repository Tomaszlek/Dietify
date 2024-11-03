using Spectre.Console;
using DietMaker.Model;
using System.Collections.Generic;
using SixLabors.ImageSharp.Processing;

namespace DietMaker.View
{
    public class CalorieView
    {

        public void DisplayLogo()
        {
            AnsiConsole.Write(new FigletText("Diet Maker").Color(Color.Green).Centered());
        }

        public string DisplayMenu(DateTime selected_date, UserDTO UserDTO, UserModel user)
        {
            AnsiConsole.Clear();

            //DayTracker(UserDTO, user);
            DisplayLogo();
            AnsiConsole.Write(new Rule("[yellow]Main Menu[/]").RuleStyle("grey"));

            /*var image = new CanvasImage("image4.png");
            image.Mutate(ctx => ctx.Invert());
            image.MaxWidth(18);
            AnsiConsole.Write(image);*/

            AnsiConsole.WriteLine("Selected day: " + selected_date.ToShortDateString() + "\n");

            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What would you like to do?")
            .AddChoices(new[] { "Select Day", "Add Meal", "View Entries", "Select Tracking", "Options", "Exit" }));

            return Choice;
        }

        public string DisplayTrackingMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Tracking Menu[/]").RuleStyle("grey"));

            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Dayly", "Weekly", "Monthly", "Return" }));

            return Choice;
        }

        public string OptionsMenu()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Options Menu[/]").RuleStyle("grey"));

            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Set Your Goal", "Save Data", "Return" }));

            return Choice;
        }

        public UserDTO SetYourGoal(UserDTO UserDTO, UserModel user)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Goal Setting Menu[/]").RuleStyle("grey"));

            AnsiConsole.Cursor.MoveDown();
            Text t1 = new Text("Looking Good " + user.UserName + '\n').Centered();
            AnsiConsole.Write(t1);
            
            Text t = new Text("Carbs Goal = " + UserDTO.Carbs + " | Fats Goal = " + UserDTO.Fats + " | Proteis Goal = " + UserDTO.Proteins
            + " | Calories Goal = " + UserDTO.Calories).Centered();
            AnsiConsole.Write(t);

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Set Your Goals, You Can Do It!")
            .AddChoices(new[] { "User Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }));

            return UserDTO;
        }

        public void DayTracker(UserDTO UserDTO, UserModel user)
        {
            
            AnsiConsole.Cursor.SetPosition(0, Console.WindowHeight - 5);
            AnsiConsole.Write(new BarChart()
            .Width((int)(Console.WindowWidth * 0.4))
            .CenterLabel()
            .AddItem("Carbs", (int)(UserDTO.Carbs), Color.Green));
            AnsiConsole.Cursor.SetPosition(0, 0);
        }

        public string SelectDayScreen()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection Menu[/]").RuleStyle("grey"));

            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What day do you want to choose?")
            .AddChoices(new[] {"Tomorrow", "Yesterday", "Today", "Select Date", "Return"}));

            return Choice;
        }

        public DateTime SelectDateScreen(DateTime date)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Day Selection By Date[/]").RuleStyle("grey"));

            var calendar = new Calendar(date.Year, date.Month).Centered();
            calendar.AddCalendarEvent(date.Year, date.Month, date.Day);
            AnsiConsole.Write(calendar);

            DateTime Choice = AnsiConsole.Ask<DateTime>("Enter Date rrrr.mm.dd: ");

            return Choice;

        }

        public string AddMeal()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Meal Menu[/]").RuleStyle("grey"));

            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "My Meals", "Meal Database", "Enter Macro", "Return" }));

            return Choice;
        }

        public UserDTO DisplayEnterMacro(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Macro Entry Menu[/]").RuleStyle("grey"));

            /*var layout = new Layout("Root").SplitColumns(new Layout("L"), new Layout("R"));
            layout["L"].Update();*/
            AnsiConsole.Cursor.MoveDown();
            Text t = new Text("Carbs = " + UserDTO.Carbs + " | Fats = " + UserDTO.Fats + " | Proteis = " + UserDTO.Proteins + " | Calories = " + UserDTO.Calories).Centered();
            AnsiConsole.Write(t);

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }));
            

            //Console.ReadKey();
            return UserDTO;
        }

        public string ApplyDiscard()
        {
            string Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Do you want to apply?")
            .AddChoices(new[] { "Yes", "No" }));

            return Choice;
        }

        public uint EnterUint(string text)
        {
            uint number = AnsiConsole.Ask<uint>(text);
            return number;
        }

        public void Error(string tekst)
        {
            AnsiConsole.Clear();
            Text text = new Text(tekst).Centered();
            AnsiConsole.Write(new Rule("[yellow]Something Unusual Has Happened[/]").RuleStyle("grey"));
            AnsiConsole.Write(text);

            Console.ReadKey();
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

            int i = 0;
            foreach (var entry in entries)
            {
                table.AddRow(i.ToString() , entry.ProductName, entry.Carbs.ToString(), entry.Fats.ToString(), entry.Proteins.ToString(), entry.Calories.ToString());
                i++;
            }

            AnsiConsole.Write(table);

            var selectio_prompt = new SelectionPrompt<string>().Title("What is Your Intent?").AddChoices(new string[] { "Make Some Changes", "Return" });
            string Choice = AnsiConsole.Prompt(selectio_prompt);

            return Choice;
        }

        public UserDTO ModifyEntry(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entry Modyfication Menu[/]").RuleStyle("grey"));

            Text t = new Text("Product Name = " + UserDTO.ProductName + " | Carbs = " + UserDTO.Carbs + " | Fats = " + UserDTO.Fats + " | Proteis = " + UserDTO.Proteins + " | Calories = " + UserDTO.Calories).Centered();
            AnsiConsole.Write(t);

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] { "Product Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }));

            return UserDTO;
        }

        public UserDTO AddEntry(UserDTO UserDTO)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Manual Entry Adding Menu[/]").RuleStyle("grey"));

            //AnsiConsole.Write('\n');
            Text t = new Text("Product Name = " + UserDTO.ProductName + " | Carbs = " + UserDTO.Carbs + " | Fats = " + UserDTO.Fats + " | Proteis = " + UserDTO.Proteins + " | Calories = " + UserDTO.Calories).Centered();
            AnsiConsole.Write(t);

            UserDTO.Choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("What are you interested in?")
            .AddChoices(new[] {"Product Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }));

            //Console.ReadKey();
            return UserDTO;
        }

        public string EditEntries(List<CalorieModel> entries)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new Rule("[yellow]Entry Modification Menu[/]").RuleStyle("grey"));

            var selection_prompt = new SelectionPrompt<string>().Title("How Do You Want To Modify?")
            .AddChoices(new string[] { "Edit Entry", "Remove Entry", "Return" });

            string choice = AnsiConsole.Prompt(selection_prompt);

            return choice;
        }

        public void DisplayMealSearchError(string mealName)
        {
            AnsiConsole.MarkupLine($"[red]Nie udało się znaleźć posiłku o nazwie '{mealName}'. Spróbuj ponownie później lub użyj innego terminu wyszukiwania.[/]");
        }

        public void DisplayMealAdded(CalorieModel mealData)
        {
            AnsiConsole.MarkupLine($"[green]Dodano posiłek: {mealData.ProductName}, kalorie: {mealData.Calories} kcal[/]");
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
            AnsiConsole.MarkupLine("[green]Your goal has been succesfully updated.[/]");
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
