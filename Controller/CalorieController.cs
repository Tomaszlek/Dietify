using DietMaker.Model;
using DietMaker.View;
using Spectre.Console;
using System.Collections.Generic;
using System;


namespace DietMaker.Controller
{
    public class CalorieController
    {
        private UserModel _user;
        private Dictionary<string, List<CalorieModel>> DayList;
        private CalorieView _view;
        private bool running = true;
        private DateTime selected_date;
        private DTO dto;

        public CalorieController()
        {
            _user = new UserModel();
            DayList = new Dictionary<string, List<CalorieModel>>();
            _view = new CalorieView();
            selected_date = DateTime.Today;
            dto = new DTO();
        }

        public void Run()
        {
            while (running)
            {
                //AnsiConsole.Clear();
                string choice = _view.DisplayMenu(selected_date);

                switch (choice)
                {
                    case "Select Day":
                        SelectDay();
                        break;
                    case "Add Meal":
                        AddMeal();
                        break;
                    case "View Entries":
                        ViewEntries();
                        break;
                    case "Select Tracking":
                        SelectTracking();
                        break;
                    case "Exit":
                        _view.ExitProgram();
                        running = false;
                        break;
                    case "Options":
                        Options();
                        break;
                }

            }

        }

        public int CalculateCalories(DTO dto)
        {
            return dto.Carbs * 4 + dto.Fats * 9 + dto.Proteins * 4;
        }
        public void Options()
        {
            bool go_back = false;

            while (!go_back)
            {
                string choice = _view.OptionsMenu();
                switch (choice)
                {
                    case "Set Your Goal":
                        SetYourGoal();
                        break;
                    case "Save Data":
                        
                        break;
                    case "Return":
                        go_back = true;
                        break;
                }
            }
        }

        public void SetYourGoal()
        {
            bool go_back = false;

            while (!go_back)
            {
                dto = _view.SetYourGoal(dto, _user);
                string choice = dto.choice;
                switch (choice)
                {
                    case "Carbs":
                        dto.Carbs = _view.EnterInt("How many grams of Carbs: ");
                        break;
                    case "User Name":
                        dto.product_name = _view.EnterString("Set your User Name:");
                        break;
                    case "Fats":
                        dto.Fats = _view.EnterInt("How many grams of Fats: ");
                        break;
                    case "Proteins":
                        dto.Proteins = _view.EnterInt("How many grams of Proteins: ");
                        break;
                    case "Calories":
                        dto.Calories = _view.EnterInt("How many Calories: ");
                        break;
                    case "Apply/Discard":

                        string choice1 = _view.DisAppMacro();

                        if(choice1 == "NO")
                        {
                            dto.reset_values();
                            go_back = true;
                        }
                        else
                        {
                            _user.carbs_goal = dto.Carbs;
                            _user.UserName = dto.product_name;
                            _user.fats_goal = dto.Fats;
                            _user.proteins_goal = dto.Proteins;
                            _user.calories_goal = dto.Calories;
                        }
                        go_back = true;
                        break;
                }
            }
        }

        public void SelectDay()
        {
            string choice = _view.SelectDayScreen();

            switch (choice)
            {
                case "Tommorow":
                    selected_date = selected_date.AddDays(1);
                    //Console.ReadKey();
                    break;
                case "Yeasterday":
                    selected_date = selected_date.AddDays(-1);
                    break;
                case "Today":
                    selected_date = DateTime.Today;
                    break;
                case "Select Date":
                    selected_date = _view.SelectDateScreen(selected_date);
                    break;
                case "Return":

                    break;
            }
        }

       

        public void AddMeal()
        {
            bool go_back = false;

            while (!go_back)
            {
                string choice = _view.AddMeal();
                switch (choice)
                {
                    case "My Meals":
                        //my meals screen
                        break;
                    case "Meal Database":
                        //API sherch screen
                        break;
                    case "Enter Macro":
                        EnterMacro();
                        break;
                    case "Return":
                        go_back = true;
                        break;
                }
            }
        }

        public void EnterMacro()
        {
            bool go_back = false;

            while (!go_back)
            {
                dto = _view.DisplayEnterMacro(dto);
                string choice = dto.choice;
                switch (choice)
                {
                    case "Carbs":
                        dto.Carbs = _view.EnterInt("How many grams of Carbs: ");
                        break;
                    case "Fats":
                        dto.Fats = _view.EnterInt("How many grams of Fats: ");
                        break;
                    case "Proteins":
                        dto.Proteins = _view.EnterInt("How many grams of Proteins: ");
                        break;
                    case "Calories":
                        dto.Calories = _view.EnterInt("How many Calories: ");
                        break;
                    case "Apply/Discard":

                        string choice1 = _view.DisAppMacro();

                        if (choice == "NO")
                        {
                            dto.reset_values();
                            go_back = true;
                        }
                        else
                        {

                            if (!DayList.ContainsKey(selected_date.ToShortDateString()))
                            {
                                DayList.Add(selected_date.ToShortDateString(), new List<CalorieModel>());
                            }

                            CalorieModel model = new CalorieModel();
                            model.ProductName = "Macro "; model.Carbs = dto.Carbs; model.Fats = dto.Fats; model.Proteins = dto.Proteins;

                            if (dto.Calories == 0)
                            {                               
                                model.Calories = model.Carbs * 4 + model.Fats * 9 + model.Proteins * 4;
                            }
                            else
                            {
                                model.Calories = dto.Calories;                               
                            }

                            DayList[selected_date.ToShortDateString()].Add(model);
                            dto.reset_values();
                            go_back = true;
                        }
                        break;
                }
            }
        }

        public void SelectTracking()
        {
            _view.DisplayTrackingMenu();
        }

        public void ViewEntries()
        {
            bool go_back = false;
            string choice = string.Empty;

            while (!go_back)
            {
                if (DayList.ContainsKey(selected_date.ToShortDateString()))
                {
                    choice = _view.ViewEntries(DayList[selected_date.ToShortDateString()]);
                }
                else
                {
                    choice = _view.ViewEntries(new List<CalorieModel>());
                }

                if (choice == "Make Some Changes")
                {
                    EditEntriesChoices();
                }
                else
                {
                    go_back = true;
                }
            }
        }

        public void EditEntriesChoices()
        {
            bool go_back = false;
            string choice = string.Empty;
            int entry_index = -1;

            while(!go_back){

                if (DayList.ContainsKey(selected_date.ToShortDateString()))
                {
                    choice = _view.EditEntriesChoices(DayList[selected_date.ToShortDateString()]);
                }
                else
                {
                    var empty_entry = new List<CalorieModel>();
                    choice = _view.EditEntriesChoices(empty_entry);
                }

                switch (choice)
                {
                    case "Edit Entry":
                        
                        break;
                    case "Remove Entry":

                        break;

                    case "Add Entry":
                        AddEntry();
                        break;
                    case "Return":
                        go_back = true;
                        break;
                }

            }
        }

        public void AddEntry()
        {
            bool go_back = false;

            while (!go_back)
            {
                dto = _view.AddEntry(dto);
                string choice = dto.choice;
                switch (choice)
                {
                    case "Product Name":
                        dto.product_name = _view.EnterString("What is your product called: ");
                        break;
                    case "Carbs":
                        dto.Carbs = _view.EnterInt("How many grams of Carbs: ");
                        break;
                    case "Fats":
                        dto.Fats = _view.EnterInt("How many grams of Fats: ");
                        break;
                    case "Proteins":
                        dto.Proteins = _view.EnterInt("How many grams of Proteins: ");
                        break;
                    case "Calories":
                        dto.Calories = _view.EnterInt("How many Calories: ");
                        break;
                    case "Apply/Discard":

        
                        break;
                }
            }
        }

        public void TotalCalories()
        {
            /*int totalCalories = 0;
            foreach (var entry in _entries)
            {
                totalCalories += entry.Calories;
            }
            _view.ShowTotalCalories(totalCalories);*/
        }
    }
}
