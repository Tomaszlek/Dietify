using DietMaker.Model;
using DietMaker.View;
using Spectre.Console;
using System.Collections.Generic;

namespace DietMaker.Controller
{
    public class CalorieController
    {
        private List<CalorieEntry> _entries;
        private CalorieView _view;
        private bool running = true;

        public CalorieController()
        {
            _entries = new List<CalorieEntry>();
            _view = new CalorieView();
        }

        public void Run()
        {
            while (running)
            {
                //AnsiConsole.Clear();
                string choice = _view.DisplayMenu();

                switch (choice)
                {
                    case "Add Entry":
                        AddEntry();
                        break;
                    case "View Entries":
                        ViewEntries();
                        break;
                    case "Total Calories":
                        TotalCalories();
                        break;
                    case "Exit":
                        _view.ExitProgram();
                        Exit();
                        break;
                }

            }

        }

        public void Exit()
        {
            running = false;
        }

        public void AddEntry()
        {
            var entry = _view.AddEntry();
            _entries.Add(entry);
        }

        public void ViewEntries()
        {
            _view.ShowEntries(_entries);
        }

        public void TotalCalories()
        {
            int totalCalories = 0;
            foreach (var entry in _entries)
            {
                totalCalories += entry.Calories;
            }
            _view.ShowTotalCalories(totalCalories);
        }
    }
}
