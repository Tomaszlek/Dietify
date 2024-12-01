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

        /*public CalorieView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Placeholder for initialization logic of the main form
        }*/

        public void DisplayLogo(DateTime selectedDate)
        {
            MessageBox.Show($"Welcome to Diet Maker!\nSelected Date: {selectedDate.ToShortDateString()}",
                "Diet Maker", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string DisplayMenu(DateTime selectedDate, UserDTO userDTO, UserModel user, Dictionary<string, List<CalorieModel>> mealData)
        {
            using (Form menuForm = new Form
            {
                Text = "Diet Maker - Main Menu",
                Width = 400,
                Height = 350,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel dla lepszego rozmieszczenia kontrolek
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                menuForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = $"Diet Maker - {selectedDate.ToShortDateString()}",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji menu
                ListBox listBoxMenu = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 200,
                    Items = { "Select Day", "Add Meal", "View Entries", "Options", "Exit" }
                };
                panel.Controls.Add(listBoxMenu);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                string selectedOption = "Exit"; // Domyślny wybór (jeśli użytkownik zamknie okno)
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxMenu.SelectedItem != null)
                    {
                        selectedOption = listBoxMenu.SelectedItem.ToString();
                        menuForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Wyświetlenie formularza
                menuForm.ShowDialog();
                return selectedOption;
            }
        }


        public string DisplayTrackingMenu()
        {
            using (Form trackingForm = new Form
            {
                Text = "Tracking Menu",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                trackingForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = "Select Tracking Period:",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxTracking = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 150,
                    Items = { "Daily", "Weekly", "Monthly", "Return" }
                };
                panel.Controls.Add(listBoxTracking);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                string selectedOption = "Return";
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxTracking.SelectedItem != null)
                    {
                        selectedOption = listBoxTracking.SelectedItem.ToString();
                        trackingForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select an option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                trackingForm.ShowDialog();
                return selectedOption;
            }
        }


        public string OptionsMenu()
        {
            using (Form optionsForm = new Form
            {
                Text = "Options Menu",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                optionsForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = "Options:",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 150,
                    Items = { "Set Your Goal", "Save Data", "Return" }
                };
                panel.Controls.Add(listBoxOptions);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                string selectedOption = "Return";
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxOptions.SelectedItem != null)
                    {
                        selectedOption = listBoxOptions.SelectedItem.ToString();
                        optionsForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select an option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                optionsForm.ShowDialog();
                return selectedOption;
            }
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
            using (Form selectDayForm = new Form
            {
                Text = "Select Day Menu",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                selectDayForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = "What day do you want to choose?",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxDays = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 150,
                    Items = { "Tomorrow", "Yesterday", "Today", "Select Date", "Return" }
                };
                panel.Controls.Add(listBoxDays);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                string selectedOption = "Return";
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxDays.SelectedItem != null)
                    {
                        selectedOption = listBoxDays.SelectedItem.ToString();
                        selectDayForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select an option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                selectDayForm.ShowDialog();
                return selectedOption;
            }
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
            using (Form addMealForm = new Form
            {
                Text = "Add Meal Menu",
                Width = 400,
                Height = 300,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                addMealForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = "What are you interested in?",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxMealOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 150,
                    Items = { "Meal Database", "Enter Macro", "Return" }
                };
                panel.Controls.Add(listBoxMealOptions);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                string selectedOption = "Return"; // Domyślna opcja
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxMealOptions.SelectedItem != null)
                    {
                        selectedOption = listBoxMealOptions.SelectedItem.ToString();
                        addMealForm.Close();
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Wyświetlenie formularza
                addMealForm.ShowDialog();
                return selectedOption;
            }
        }


        public UserDTO DisplayEnterMacro(UserDTO userDTO)
        {
            // Zapisz oryginalne wartości przed rozpoczęciem edycji
            UserDTO originalUserDTO = new UserDTO
            {
                Carbs = userDTO.Carbs,
                Fats = userDTO.Fats,
                Proteins = userDTO.Proteins,
                Calories = userDTO.Calories
            };

            using (Form macroForm = new Form
            {
                Text = "Macro Entry Menu",
                Width = 400,
                Height = 400,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                macroForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = "Enter Macros:",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblHeader);

                // Informacje o bieżących wartościach
                Label lblCurrentMacros = new Label
                {
                    Text = $"Carbs = {userDTO.Carbs}, Fats = {userDTO.Fats}, Proteins = {userDTO.Proteins}, Calories = {userDTO.Calories}",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60
                };
                panel.Controls.Add(lblCurrentMacros);

                // Lista opcji makro
                ListBox listBoxOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12),
                    Height = 150,
                    Items = { "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard" }
                };
                panel.Controls.Add(listBoxOptions);

                // Przycisk wyboru
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                // Przycisk powrotu (Return)
                Button btnReturn = new Button
                {
                    Text = "Return",
                    Dock = DockStyle.Bottom,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                // Przycisk powrotu (Return) zamykający formularz
                btnReturn.Click += (s, e) =>
                {
                    // Po kliknięciu przycisku Return, zamykamy formularz i wracamy do menu głównego
                    macroForm.Close();
                };

                // Przycisk Select - logika wyboru opcji
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxOptions.SelectedItem != null)
                    {
                        string choice = listBoxOptions.SelectedItem.ToString();

                        // Otwórz okno dialogowe do wprowadzenia nowej wartości
                        switch (choice)
                        {
                            case "Carbs":
                                userDTO.Carbs = (int)EnterUint("Enter grams of Carbs:");
                                break;
                            case "Fats":
                                userDTO.Fats = (int)EnterUint("Enter grams of Fats:");
                                break;
                            case "Proteins":
                                userDTO.Proteins = (int)EnterUint("Enter grams of Proteins:");
                                break;
                            case "Calories":
                                userDTO.Calories = (int)EnterUint("Enter Calories:");
                                break;
                            case "Apply/Discard":
                                // Wyświetl dialog z pytaniem, czy zatwierdzić czy odrzucić zmiany
                                DialogResult dialogResult = MessageBox.Show("Do you want to apply the changes?", "Apply Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (dialogResult == DialogResult.Yes)
                                {
                                    // Zatwierdzenie zmian - zmiany są już zapisane w userDTO
                                    macroForm.Close(); // Zakończ formularz
                                }
                                else if (dialogResult == DialogResult.No)
                                {
                                    // Odrzucenie zmian - przywrócenie oryginalnych danych
                                    userDTO.Carbs = originalUserDTO.Carbs;
                                    userDTO.Fats = originalUserDTO.Fats;
                                    userDTO.Proteins = originalUserDTO.Proteins;
                                    userDTO.Calories = originalUserDTO.Calories;
                                    macroForm.Close(); // Zakończ formularz
                                }
                                return;
                        }

                        // Zaktualizuj etykietę z bieżącymi wartościami
                        lblCurrentMacros.Text = $"Carbs = {userDTO.Carbs}, Fats = {userDTO.Fats}, Proteins = {userDTO.Proteins}, Calories = {userDTO.Calories}";
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };

                // Dodanie przycisków do panelu
                panel.Controls.Add(btnSelect);
                panel.Controls.Add(btnReturn);

                // Wyświetlenie formularza
                macroForm.ShowDialog();
            }

            return userDTO;
        }







        public string ApplyDiscard()
        {
            string result = "";

            using (Form applyDiscardForm = new Form
            {
                Text = "Apply Changes",
                Width = 300,
                Height = 200,
                StartPosition = FormStartPosition.CenterParent
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                applyDiscardForm.Controls.Add(panel);

                // Etykieta z pytaniem
                Label lblMessage = new Label
                {
                    Text = "Do you want to apply the changes?",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40
                };
                panel.Controls.Add(lblMessage);

                // Przycisk Apply
                Button btnApply = new Button
                {
                    Text = "Apply",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                // Przycisk Discard
                Button btnDiscard = new Button
                {
                    Text = "Discard",
                    Dock = DockStyle.Top,
                    Height = 40,
                    Font = new Font("Arial", 12)
                };

                // Przyciski odpowiedzialne za wybór
                btnApply.Click += (s, e) =>
                {
                    result = "Apply"; // Zatwierdzenie zmian
                    applyDiscardForm.Close();
                };

                btnDiscard.Click += (s, e) =>
                {
                    result = "Discard"; // Odrzucenie zmian
                    applyDiscardForm.Close();
                };

                panel.Controls.Add(btnApply);
                panel.Controls.Add(btnDiscard);

                applyDiscardForm.ShowDialog();
            }

            return result; // Zwróć wynik po zamknięciu formularza
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
