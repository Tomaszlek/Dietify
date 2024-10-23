using DietMaker.Model;
using DietMaker.View;
using Spectre.Console;
using System.Collections.Generic;
using System;

void clear_dto()
{

}

namespace DietMaker.Controller
{
    public class CalorieController
    {
        private Dictionary<DateTime, List<CalorieModel>> DayList;
        private CalorieView _view;
        private bool running = true;
        private DateTime selected_date;
        private DTO dto;

        public CalorieController()
        {
            DayList = new Dictionary<DateTime, List<CalorieModel>>();
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
                        Exit();

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
                case "Select Day":

                    //day selection screen

                    break;
                case "Return":

                    break;
            }
        }

        public void Exit()
        {
            running = false;
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
                        _view.DisplayEnterMacro();
                        break;
                    case "Return":
                        go_back = true;
                        break;
                }
            }
        }

        public void SelectTracking()
        {
            _view.DisplayTracking();
        }

        public void ViewEntries()
        {
            //_view.ShowEntries(_entries);
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
