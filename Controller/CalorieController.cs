using DietMaker.Model;
using DietMaker.API;
using DietMaker.View;
using DietMaker;
using Spectre.Console;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace DietMaker.Controller
{
    public class CalorieController
    {
        private readonly NutritionixClient _nutritionixClient;
        private readonly UserModel _user;
        private Dictionary<string, List<CalorieModel>> _dayList;
        private readonly CalorieView _view;
        private readonly MealDataStorage _storage;
        private bool _running;
        private DateTime _selectedDate;
        private UserDTO _userDTO;

        public CalorieController()
        {
            _user = UserModel.LoadFromFile("user_data.json");
            _dayList = new Dictionary<string, List<CalorieModel>>(); 
            _view = new CalorieView();
            _storage = new MealDataStorage("meals.json");
            _selectedDate = DateTime.Today;
            _userDTO = new UserDTO();
            _nutritionixClient = new NutritionixClient("aa6ed47d", "3e9fcbb613cce2df9e19f641d4630c31");
            _running = true;

            LoadMeals();
            

        }

        public async Task Run()
        {
            while (_running)
            {
                ResetUserDTO();
                string choice = _view.DisplayMenu(_selectedDate, _userDTO, _user, _dayList);

                await ExecuteMenuChoice(choice);
            }
        }

        private async Task ExecuteMenuChoice(string choice)
        {
            switch (choice)
            {
                case "Select Day":
                    SelectDay();
                    break;
                case "Add Meal":
                    await AddMeal();
                    break;
                case "View Entries":
                    ViewEntries();
                    break;
                case "Select Tracking":
                    SelectTracking();
                    break;
                case "Exit":
                    ExitProgram();
                    break;
                case "Options":
                    await ShowOptions();
                    break;
                default:
                    _view.Error("Invalid choice, please try again.");
                    break;
            }
        }

        private async void LoadMeals()
        {
            _dayList = await _storage.LoadMealsAsync();
        }

        private void ResetUserDTO()
        {
            _userDTO.ResetValues();
        }

        private async Task ShowOptions()
        {
            bool goBack = false;
            while (!goBack)
            {
                string choice = _view.OptionsMenu();
                switch (choice)
                {
                    case "Set Your Goal":
                        SetYourGoal();
                        break;
                    case "Save Data":
                        await SaveData();
                        break;
                    case "Return":
                        goBack = true;
                        break;
                }
            }
        }

        private async Task SaveData()
        {
            await _storage.SaveMealsAsync(_dayList);
            _user.SaveToFile("user_data.json");
        }

        private void SetYourGoal()
        {
            bool goBack = false;

            while (!goBack)
            {
                _userDTO = _view.SetYourGoal(_userDTO, _user);
                string choice = _userDTO.Choice;
                if (choice == "Apply/Discard")
                {
                    goBack = ApplyOrDiscardGoal();
                }
                else
                {
                    UpdateUserGoals(choice);
                }
            }
        }

        private bool ApplyOrDiscardGoal()
        {
            string choice = _view.ApplyDiscard();
            if (choice == "NO")
            {
                _userDTO.ResetValues();
                return true;
            }
            UpdateUserGoals(null);
            _view.DisplayGoalUpdated();
            return true;
        }

        private void UpdateUserGoals(string choice)
        {
            switch (choice)
            {
                case "Carbs":
                    _userDTO.Carbs = (int)_view.EnterUint("How many grams of Carbs: ");
                    break;
                case "User Name":
                    _userDTO.ProductName = _view.EnterString("Set your User Name:");
                    break;
                case "Fats":
                    _userDTO.Fats = (int)_view.EnterUint("How many grams of Fats: ");
                    break;
                case "Proteins":
                    _userDTO.Proteins = (int)_view.EnterUint("How many grams of Proteins: ");
                    break;
                case "Calories":
                    _userDTO.Calories = (int)_view.EnterUint("How many Calories: ");
                    break;
            }
            _user.CarbsGoal = (uint)_userDTO.Carbs;
            _user.UserName = _userDTO.ProductName;
            _user.FatsGoal = (uint)_userDTO.Fats;
            _user.ProteinsGoal = (uint)_userDTO.Proteins;
            _user.CaloriesGoal = (uint)_userDTO.Calories;
        }

        private void SelectDay()
        {
            string choice = _view.SelectDayScreen();
            switch (choice)
            {
                case "Tomorrow":
                    _selectedDate = _selectedDate.AddDays(1);
                    break;
                case "Yesterday":
                    _selectedDate = _selectedDate.AddDays(-1);
                    break;
                case "Today":
                    _selectedDate = DateTime.Today;
                    break;
                case "Select Date":
                    _selectedDate = _view.SelectDateScreen(_selectedDate);
                    break;
            }
        }

        private async Task AddMeal()
        {
            bool goBack = false;

            while (!goBack)
            {
                string choice = _view.AddMeal();
                switch (choice)
                {
                    case "Meal Database":
                        await AddMealFromApi();
                        break;
                    case "Enter Macro":
                        EnterMacro();
                        break;
                    case "Return":
                        goBack = true;
                        break;
                }
            }
        }

        private async Task AddMealFromApi()
        {
            try
            {
                string mealName = _view.EnterString("Enter meal name to search:");
                await AnsiConsole.Status().StartAsync("Searching for meal...", async ctx =>
                {
                    var mealData = await _nutritionixClient.FetchMealDataAsync(mealName);
                    if (mealData != null)
                    {
                        AddMealToDayList(mealData);
                        _view.DisplayMealAdded(mealData);
                    }
                    else
                    {
                        _view.DisplayMealSearchError(mealName);
                    }
                });
            }
            catch (Exception ex)
            {
                _view.Error($"Error while adding meal: {ex.Message}");
            }
        }

        private void AddMealToDayList(CalorieModel mealData)
        {
            if (!_dayList.ContainsKey(_selectedDate.ToShortDateString()))
            {
                _dayList[_selectedDate.ToShortDateString()] = new List<CalorieModel>();
            }
            _dayList[_selectedDate.ToShortDateString()].Add(mealData);
        }

        private void EnterMacro()
        {
            bool goBack = false;

            while (!goBack)
            {
                // Display the current DTO and get the user's choice
                _userDTO = _view.DisplayEnterMacro(_userDTO);
                string choice = _userDTO.Choice;

                switch (choice)
                {
                    case "Carbs":
                        _userDTO.Carbs = (int)_view.EnterUint("How many grams of Carbs: ");
                        break;

                    case "Fats":
                        _userDTO.Fats = (int)_view.EnterUint("How many grams of Fats: ");
                        break;

                    case "Proteins":
                        _userDTO.Proteins = (int)_view.EnterUint("How many grams of Proteins: ");
                        break;

                    case "Calories":
                        _userDTO.Calories = (int)_view.EnterUint("How many Calories: ");
                        break;

                    case "Apply/Discard":
                        string applyDiscardChoice = _view.ApplyDiscard();

                        if (applyDiscardChoice == "No")
                        {
                            _userDTO.ResetValues();
                            goBack = true;
                        }
                        else
                        {
                            // Ensure DayList contains an entry for the selected date
                            if (!_dayList.ContainsKey(_selectedDate.ToShortDateString()))
                            {
                                _dayList.Add(_selectedDate.ToShortDateString(), new List<CalorieModel>());
                            }

                            // Create and populate a new CalorieModel
                            CalorieModel model = new CalorieModel
                            {
                                ProductName = "Macro",
                                Carbs = (uint)_userDTO.Carbs,
                                Fats = (uint)_userDTO.Fats,
                                Proteins = (uint)_userDTO.Proteins
                            };

                            // Calculate Calories if not provided
                            if (_userDTO.Calories == 0)
                            {
                                model.Calories = model.Carbs * 4 + model.Fats * 9 + model.Proteins * 4;
                            }
                            else
                            {
                                model.Calories = (uint)_userDTO.Calories;
                            }

                            // Add the model to the DayList for the selected date
                            _dayList[_selectedDate.ToShortDateString()].Add(model);
                            _userDTO.ResetValues();
                            goBack = true; // Exit loop after applying
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private bool UpdateMacroValues(string choice)
        {
            switch (choice)
            {
                case "Carbs":
                    _userDTO.Carbs = (int)_view.EnterUint("How many grams of Carbs: ");
                    break;
                case "Fats":
                    _userDTO.Fats = (int)_view.EnterUint("How many grams of Fats: ");
                    break;
                case "Proteins":
                    _userDTO.Proteins = (int)_view.EnterUint("How many grams of Proteins: ");
                    break;
                case "Calories":
                    _userDTO.Calories = (int)_view.EnterUint("How many Calories: ");
                    break;
                case "Apply/Discard":
                    return true;
            }
            return false;
        }

        private void SaveMacroEntry()
        {
            if (!_dayList.ContainsKey(_selectedDate.ToShortDateString()))
            {
                _dayList[_selectedDate.ToShortDateString()] = new List<CalorieModel>();
            }

            var model = new CalorieModel
            {
                ProductName = "Macro",
                Carbs = (uint)_userDTO.Carbs,
                Fats = (uint)_userDTO.Fats,
                Proteins = (uint)_userDTO.Proteins,
                Calories = _userDTO.Calories == 0 ? CalculateCalories(_userDTO) : (uint)_userDTO.Calories
            };

            _dayList[_selectedDate.ToShortDateString()].Add(model);
            _view.DisplayMealAdded(model);
            _userDTO.ResetValues();
        }

        public uint CalculateCalories(UserDTO userDTO)
        {
            return (uint)(userDTO.Carbs * 4 + userDTO.Fats * 9 + userDTO.Proteins * 4);
        }

        private void SelectTracking()
        {
            _view.DisplayTrackingMenu();
        }

        private void ViewEntries()
        {
            bool goBack = false;

            while (!goBack)
            {
                var entries = _dayList.ContainsKey(_selectedDate.ToShortDateString())
                    ? _dayList[_selectedDate.ToShortDateString()]
                    : new List<CalorieModel>();

                string choice = _view.ViewEntries(entries);
                if (choice == "Make Some Changes")
                {
                    EditEntries();
                }
                else
                {
                    goBack = true;
                }
            }
        }

        private void EditEntries()
        {
            bool goBack = false;

            while (!goBack)
            {
                var entries = _dayList.ContainsKey(_selectedDate.ToShortDateString())
                    ? _dayList[_selectedDate.ToShortDateString()]
                    : new List<CalorieModel>();

                string entryChoice = _view.EditEntries(entries);

                switch (entryChoice)
                {
                    case "Edit Entry":
                        if (!entries.Any())
                        {
                            _view.Error("There is Nothing to Modify");
                            break;
                        }

                        int entryIndexToEdit = (int)_view.EnterUint("Give me the index of the entry you want to modify:");

                        if (entryIndexToEdit < 0 || entryIndexToEdit >= entries.Count)
                        {
                            _view.Error("There is no such Index Friend");
                            break;
                        }

                        ModifyEntry((uint)entryIndexToEdit);
                        break;

                    case "Remove Entry":
                        if (!entries.Any())
                        {
                            _view.Error("There is Nothing to Remove Friend :)");
                            break;
                        }

                        int entryIndexToRemove = (int)_view.EnterUint("Give me the index of the entry you want to remove:");
                        string confirmRemove = _view.ApplyDiscard();

                        if (confirmRemove == "Yes")
                        {
                            if (entryIndexToRemove < 0 || entryIndexToRemove >= entries.Count)
                            {
                                _view.Error("There is no such Index Friend");
                                break;
                            }

                            entries.RemoveAt(entryIndexToRemove);
                        }
                        break;

                    case "Return":
                        goBack = true;
                        break;

                    default:
                        _view.Error("Invalid choice, please try again.");
                        break;
                }
            }
        }

        public void ModifyEntry(uint entry_index)
        {
            bool go_back = false;
            _userDTO = _dayList[_selectedDate.ToShortDateString()][(int)entry_index].ToDTO();

            while (!go_back)
            {
                _userDTO = _view.ModifyEntry(_userDTO);
                string choice = _userDTO.Choice;
                switch (choice)
                {
                    case "Product Name":
                        _userDTO.ProductName = _view.EnterString("What is your product called: ");
                        break;
                    case "Carbs":
                        _userDTO.Carbs = (int)_view.EnterUint("How many grams of Carbs: ");
                        break;
                    case "Fats":
                        _userDTO.Fats = (int)_view.EnterUint("How many grams of Fats: ");
                        break;
                    case "Proteins":
                        _userDTO.Proteins = (int)_view.EnterUint("How many grams of Proteins: ");
                        break;
                    case "Calories":
                        _userDTO.Calories = (int)_view.EnterUint("How many Calories: ");
                        break;
                    case "Apply/Discard":
                        string choice1 = _view.ApplyDiscard();

                        switch (choice1)
                        {
                            case "Yes":
                                CalorieModel model = new CalorieModel
                                {
                                    ProductName = _userDTO.ProductName,
                                    Carbs = (uint)_userDTO.Carbs,
                                    Fats = (uint)_userDTO.Fats,
                                    Proteins = (uint)_userDTO.Proteins,
                                    Calories = (uint)_userDTO.Calories
                                };

                                _dayList[_selectedDate.ToShortDateString()][(int)entry_index] = model;
                                break;
                            case "No":
                                _userDTO.ResetValues();
                                break;
                        }
                        go_back = true;
                        break;
                }
            }
        }

        private void ExitProgram()
        {
            SaveData();
            _running = false;
            _view.DisplayExitMessage();
        }
    }
}