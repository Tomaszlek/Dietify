using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DietMaker.Model;

namespace DietMaker.View
{
    public class CalorieView
    {
        private Form mainForm;

        public CalorieView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Placeholder for initialization logic of the main form
        }

        public void DisplayLogo(DateTime selectedDate)
        {
            MessageBox.Show($"Welcome to Diet Maker!\nSelected Date: {selectedDate.ToShortDateString()}",
                "Diet Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string DisplayMenu(DateTime selectedDate, UserDTO userDTO, UserModel user, Dictionary<string, List<CalorieModel>> mealData)
        {
            return ShowMenu(new[] { "Select Day", "Add Meal", "View Entries", "Options", "Exit" },
                "Main Menu", $"Diet Maker - {selectedDate.ToShortDateString()}");
        }

        public string DisplayTrackingMenu()
        {
            return ShowMenu(new[] { "Daily", "Weekly", "Monthly", "Return" }, "Tracking Menu", "Select Tracking Period:");
        }

        public string OptionsMenu()
        {
            return ShowMenu(new[] { "Set Your Goal", "Save Data", "Return" }, "Options Menu", "Select an Option:");
        }

        public UserDTO SetYourGoal(UserDTO userDTO, UserModel user)
        {
            using (Form goalForm = new Form
            {
                Text = "Set Your Goal",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                Label lbl = new Label
                {
                    Text = $"Hello, {user.UserName}! Current Goals:\nCarbs: {userDTO.Carbs}, Fats: {userDTO.Fats}, Proteins: {userDTO.Proteins}, Calories: {userDTO.Calories}",
                    Dock = DockStyle.Top,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                goalForm.Controls.Add(lbl);

                ComboBox cb = new ComboBox
                {
                    Dock = DockStyle.Top,
                    Items = { "User Name", "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }
                };
                goalForm.Controls.Add(cb);

                Button btn = new Button
                {
                    Text = "Apply",
                    Dock = DockStyle.Bottom
                };
                btn.Click += (s, e) =>
                {
                    userDTO.Choice = cb.SelectedItem?.ToString();
                    goalForm.Close();
                };
                goalForm.Controls.Add(btn);

                goalForm.ShowDialog();
            }

            return userDTO;
        }

        public void DayTracker(DateTime selectedDate, UserDTO userDTO, UserModel user, Dictionary<string, List<CalorieModel>> mealData)
        {
            if (!mealData.TryGetValue(selectedDate.ToShortDateString(), out List<CalorieModel> entries))
            {
                MessageBox.Show($"No entries found for {selectedDate.ToShortDateString()}",
                    "Day Tracker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string summary = $"Carbs: {entries.Sum(e => e.Carbs)} / {user.CarbsGoal}\n" +
                             $"Fats: {entries.Sum(e => e.Fats)} / {user.FatsGoal}\n" +
                             $"Proteins: {entries.Sum(e => e.Proteins)} / {user.ProteinsGoal}\n" +
                             $"Calories: {entries.Sum(e => e.Calories)} / {user.CaloriesGoal}";

            MessageBox.Show(summary, "Day Tracker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string SelectDayScreen()
        {
            return ShowMenu(new[] { "Tomorrow", "Yesterday", "Today", "Select Date", "Return" }, "Select Day Menu", "Choose an Option:");
        }

        public DateTime SelectDateScreen(DateTime date)
        {
            using (Form calendarForm = new Form
            {
                Text = "Select Date",
                Width = 300,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                MonthCalendar calendar = new MonthCalendar
                {
                    Dock = DockStyle.Fill,
                    MaxSelectionCount = 1
                };
                calendarForm.Controls.Add(calendar);

                Button btn = new Button
                {
                    Text = "Select Date",
                    Dock = DockStyle.Bottom
                };
                btn.Click += (s, e) => calendarForm.Close();
                calendarForm.Controls.Add(btn);

                calendarForm.ShowDialog();
                return calendar.SelectionStart;
            }
        }

        public string AddMeal()
        {
            return ShowMenu(new[] { "Meal Database", "Enter Macro", "Return" }, "Add Meal Menu", "Choose an Option:");
        }

        public UserDTO DisplayEnterMacro(UserDTO userDTO)
        {
            return ModifyMacros(userDTO, "Macro Entry Menu");
        }

        public string ApplyDiscard()
        {
            return ShowMenu(new[] { "Yes", "No" }, "Apply Changes", "Do you want to apply?");
        }

        public uint EnterUint(string text)
        {
            return uint.Parse(EnterString(text));
        }

        public void Error(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public float EnterFloat(string text)
        {
            return float.Parse(EnterString(text));
        }

        public string EnterString(string text)
        {
            using (Form inputForm = new Form
            {
                Text = text,
                Width = 300,
                Height = 200,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                TextBox tb = new TextBox { Dock = DockStyle.Fill };
                Button btn = new Button { Text = "OK", Dock = DockStyle.Bottom };
                btn.Click += (s, e) => inputForm.Close();
                inputForm.Controls.Add(tb);
                inputForm.Controls.Add(btn);

                inputForm.ShowDialog();
                return tb.Text;
            }
        }

        public string ViewEntries(List<CalorieModel> entries)
        {
            using (Form viewForm = new Form
            {
                Text = "Entries",
                Width = 500,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                ListView listView = new ListView { Dock = DockStyle.Fill};
                listView.Columns.Add("Product Name");
                listView.Columns.Add("Carbs");
                listView.Columns.Add("Fats");
                listView.Columns.Add("Proteins");
                listView.Columns.Add("Calories");

                foreach (var entry in entries)
                {
                    listView.Items.Add(new ListViewItem(new[]
                    {
                        entry.ProductName,
                        entry.Carbs.ToString(),
                        entry.Fats.ToString(),
                        entry.Proteins.ToString(),
                        entry.Calories.ToString()
                    }));
                }
                viewForm.Controls.Add(listView);

                Button btn = new Button { Text = "OK", Dock = DockStyle.Bottom };
                btn.Click += (s, e) => viewForm.Close();
                viewForm.Controls.Add(btn);

                viewForm.ShowDialog();
            }

            return "Return";
        }

        public UserDTO ModifyEntry(UserDTO userDTO)
        {
            return ModifyMacros(userDTO, "Modify Entry");
        }

        public void DisplayMealSearchError(string mealName)
        {
            MessageBox.Show($"Could not find meal '{mealName}'. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void DisplayMealAdded(CalorieModel mealData)
        {
            MessageBox.Show($"Added meal: {mealData.ProductName}, calories: {mealData.Calories} kcal", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayDataSaved()
        {
            MessageBox.Show("Data has been successfully saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayGoalUpdated()
        {
            MessageBox.Show("Your goal has been successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void DisplayExitMessage()
        {
            MessageBox.Show("Goodbye!", "Exit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private UserDTO ModifyMacros(UserDTO userDTO, string title)
        {
            // Form logic for macro modification
            return userDTO;
        }

        public string EditEntries(List<CalorieModel> entries)
        {
            using (Form editForm = new Form
            {
                Text = "Edit Entries",
                Width = 600,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Lista wpisów
                ListBox listBox = new ListBox
                {
                    Dock = DockStyle.Top,
                    Height = 200,
                    Font = new Font("Arial", 12)
                };

                foreach (var entry in entries)
                {
                    listBox.Items.Add($"{entry.ProductName} - {entry.Calories} kcal");
                }

                editForm.Controls.Add(listBox);

                // Opcje edycji
                ComboBox comboBoxOptions = new ComboBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Items = { "Edit Entry", "Remove Entry", "Return" }
                };
                comboBoxOptions.SelectedIndex = 0;
                editForm.Controls.Add(comboBoxOptions);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Apply",
                    Dock = DockStyle.Bottom,
                    Height = 40
                };

                string result = "Return";
                btnSelect.Click += (s, e) =>
                {
                    if (comboBoxOptions.SelectedItem != null)
                    {
                        result = comboBoxOptions.SelectedItem.ToString();
                    }
                    editForm.Close();
                };

                editForm.Controls.Add(btnSelect);

                editForm.ShowDialog();
                return result;
            }
        }


        private string ShowMenu(string[] options, string title, string prompt)
        {
            using (Form menuForm = new Form
            {
                Text = title,
                Width = 300,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                ListBox listBox = new ListBox
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 12),
                    Items = { options }
                };
                menuForm.Controls.Add(listBox);

                Button btn = new Button { Text = "Select", Dock = DockStyle.Bottom };
                btn.Click += (s, e) => menuForm.Close();
                menuForm.Controls.Add(btn);

                menuForm.ShowDialog();
                return listBox.SelectedItem?.ToString();
            }
        }
    }
}
