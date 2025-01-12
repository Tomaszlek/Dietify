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

        private void RenderProgressCharts(Panel panel, Dictionary<string, List<CalorieModel>> dayList, UserModel user)
        {
            panel.Controls.Clear(); // Czyszczenie panelu

            var today = DateTime.Today.ToShortDateString();
            var progress = new UserDTO();

            if (dayList.TryGetValue(today, out var meals))
            {
                progress.Carbs = (int)meals.Sum(m => m.Carbs);
                progress.Fats = (int)meals.Sum(m => m.Fats);
                progress.Proteins = (int)meals.Sum(m => m.Proteins);
                progress.Calories = (int)meals.Sum(m => m.Calories);
            }

            // Dodawanie pasków postępu
            AddProgressBar(panel, "Calories", progress.Calories, Math.Max(1, user.CaloriesGoal));
            AddProgressBar(panel, "Proteins", progress.Proteins, Math.Max(1, user.ProteinsGoal));
            AddProgressBar(panel, "Carbs", progress.Carbs, Math.Max(1, user.CarbsGoal));
            AddProgressBar(panel, "Fats", progress.Fats, Math.Max(1, user.FatsGoal));
        }

        private void AddProgressBar(Panel panel, string label, int value, uint goal)
        {
            Color barColor = DetermineProgressColor(value, goal);
            // Ta sama implementacja, co poprzednio
            Label lbl = new Label
            {
                Text = $"{label}: {value}/{goal} ({CalculatePercentage(value, goal)}%)",
                Dock = DockStyle.Top,
                Height = 20
            };
            panel.Controls.Add(lbl);

            ProgressBar progressBar = new ProgressBar
            {
                Minimum = 0,
                Maximum = (int)goal,
                Value = Math.Min((int)value, (int)goal),
                Dock = DockStyle.Top,
                Height = 20
            };
            progressBar.ForeColor = barColor;
            panel.Controls.Add(progressBar);
        }

        private Color DetermineProgressColor(int value, uint goal)
        {
            double percentage = (value / (double)goal) * 100;

            if (percentage <= 70) return Color.Green;   // Good progress
            if (percentage <= 90) return Color.Orange;  // Approaching limit
            return Color.Red;                           // Exceeded goal
        }

        private int CalculatePercentage(int value, uint goal)
        {
            return goal > 0 ? (int)((value / (double)goal) * 100) : 0;
        }

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
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Tło w ciemnym kolorze
                ForeColor = Color.White // Tekst w jasnym kolorze
            })
            {
                // Ustawienie koloru tytułu formularza
                menuForm.Paint += (s, e) =>
                {
                    e.Graphics.DrawString(menuForm.Text, new Font("Arial", 14, FontStyle.Bold),
                        new SolidBrush(Color.Red), new PointF(10, 10));
                };

                // Główna tabela układu
                TableLayoutPanel mainLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 3,
                    ColumnCount = 1,
                    BackColor = Color.FromArgb(30, 30, 30) // Tło w ciemnym kolorze
                };
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); // Górny pasek (nagłówek)
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60));  // Sekcja menu
                mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40));  // Sekcja wykresów
                menuForm.Controls.Add(mainLayout);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = $"Diet Maker - {selectedDate.ToShortDateString()}",
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 24, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 100,
                    BackColor = Color.FromArgb(45, 45, 45), // Pasek w nieco jaśniejszym kolorze
                    ForeColor = Color.FromArgb(200, 255, 20, 20)
                };
                mainLayout.Controls.Add(lblHeader);

                // Sekcja menu
                Panel menuPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                mainLayout.Controls.Add(menuPanel);

                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Top,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70), // Guzik w jasnym ciemnym kolorze
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90); // Obramowanie przycisku
                menuPanel.Controls.Add(btnSelect);

                ListBox listBoxMenu = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 16),
                    Height = 250,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło listy
                    ForeColor = Color.Blue, // Niebieskie napisy w opcjach
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 30, // Wyższe wiersze dla lepszej czytelności
                };

                // Dodanie opcji menu
                listBoxMenu.Items.AddRange(new object[] { "Select Day", "Add Meal", "View Entries", "Options", "Exit" });

                // Ustawienie koloru podświetlenia
                listBoxMenu.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxMenu.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(150, 50, 255, 50) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.Blue;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxMenu.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                menuPanel.Controls.Add(listBoxMenu);

                

                // Panel wykresów
                Panel progressPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                mainLayout.Controls.Add(progressPanel);

                // Renderowanie wykresów
                RenderProgressCharts(progressPanel, mealData, user);

                string selectedOption = "Exit"; // Domyślny wybór
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

                // Wyświetlenie formularza
                menuForm.ShowDialog();
                return selectedOption;
            }
        }

        public string DisplayTrackingMenu()
        {
            string selectedOption = "Return"; // Domyślna opcja

            using (Form trackingForm = new Form
            {
                Text = "Tracking Menu",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
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
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxTracking = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 150,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst jasny
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40, // Wyższe wiersze
                    Items = { "Daily", "Weekly", "Monthly", "Return" }
                };

                // Podświetlenie wybranej opcji
                listBoxTracking.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxTracking.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxTracking.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxTracking);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
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

                // Wyświetlenie formularza
                trackingForm.ShowDialog();
                return selectedOption; // Zwrócenie wybranej opcji
            }
        }


        public string OptionsMenu()
        {
            string selectedOption = "Return"; // Domyślna opcja

            using (Form optionsForm = new Form
            {
                Text = "Options Menu",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
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
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 150,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst jasny
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40, // Wyższe wiersze
                    Items = { "Set Your Goal", "Save Data", "Return" }
                };

                // Podświetlenie wybranej opcji
                listBoxOptions.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxOptions.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxOptions.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxOptions);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxOptions.SelectedItem != null)
                    {
                        selectedOption = listBoxOptions.SelectedItem.ToString();
                        optionsForm.Close(); // Zamknięcie formularza po dokonaniu wyboru
                    }
                    else
                    {
                        MessageBox.Show("Please select an option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Wyświetlenie formularza
                optionsForm.ShowDialog();
                return selectedOption; // Zwrócenie wybranej opcji
            }
        }


        public UserDTO SetYourGoal(UserDTO userDTO, UserModel user)
        {
            using (Form goalForm = new Form
            {
                Text = "Set Your Goal",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel { Dock = DockStyle.Fill };
                goalForm.Controls.Add(panel);

                // Nagłówek
                Label lblHeader = new Label
                {
                    Text = $"Set Your Goal - Current Goals:\nCarbs: {userDTO.Carbs}, Fats: {userDTO.Fats}, Proteins: {userDTO.Proteins}, Calories: {userDTO.Calories}",
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 150,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst jasny
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40,
                    Items = { "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard", "Return" }
                };

                // Podświetlenie wybranej opcji
                listBoxOptions.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxOptions.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxOptions.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxOptions);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxOptions.SelectedItem != null)
                    {
                        string choice = listBoxOptions.SelectedItem.ToString();
                        userDTO.Choice = choice; // Ustaw wybór użytkownika
                        goalForm.Close(); // Zamknij formularz po dokonaniu wyboru
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Przycisk "Return"
                Button btnReturn = new Button
                {
                    Text = "Return",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(255, 99, 71), // Czerwony kolor (Tomato)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnReturn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnReturn.Click += (s, e) =>
                {
                    userDTO.Choice = "Return"; // Ustaw wybór użytkownika na "Return"
                    goalForm.Close(); // Zamknij formularz
                };
                panel.Controls.Add(btnReturn);

                goalForm.ShowDialog();
            }

            return userDTO; // Zwrócenie zaktualizowanego obiektu UserDTO
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
            string selectedOption = "Return"; // Domyślna opcja

            using (Form selectDayForm = new Form
            {
                Text = "Select Day Menu",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
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
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxDays = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 150,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst jasny
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40, // Wyższe wiersze
                    Items = { "Tomorrow", "Yesterday", "Today", "Select Date", "Return" }
                };

                // Podświetlenie wybranej opcji
                listBoxDays.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxDays.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxDays.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxDays);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxDays.SelectedItem != null)
                    {
                        selectedOption = listBoxDays.SelectedItem.ToString();
                        selectDayForm.Close(); // Zamknij formularz po dokonaniu wyboru
                    }
                    else
                    {
                        MessageBox.Show("Please select an option.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Wyświetlenie formularza
                selectDayForm.ShowDialog();
                return selectedOption; // Zwrócenie wybranej opcji
            }
        }

        public DateTime SelectDateScreen(DateTime date)
        {
            DateTime selectedDate = date; // Ustawienie domyślnej daty

            using (Form calendarForm = new Form
            {
                Text = "Select Date",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Ustawienie tytułu formularza na czerwony
                calendarForm.Paint += (s, e) =>
                {
                    e.Graphics.DrawString(calendarForm.Text, new Font("Consolas", 16, FontStyle.Bold),
                        new SolidBrush(Color.Red), new PointF(10, 10));
                };

                // Panel kontenerowy
                Panel panel = new Panel
                {
                    Dock = DockStyle.Fill
                };
                calendarForm.Controls.Add(panel);

                // MonthCalendar (kalendarz)
                MonthCalendar calendar = new MonthCalendar
                {
                    Dock = DockStyle.Fill,
                    MaxSelectionCount = 1,
                    Font = new Font("Consolas", 14), // Czcionka dla kalendarza
                    ForeColor = Color.White,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne kalendarza
                    TitleBackColor = Color.FromArgb(60, 60, 60)
                };

                // Obsługa zmiany daty
                calendar.DateSelected += (s, e) =>
                {
                    selectedDate = e.Start;  // Przypisanie daty do zmiennej
                };

                // Domyślna data po załadowaniu formularza
                calendar.SetDate(date);

                panel.Controls.Add(calendar);

                // Przycisk wyboru daty
                Button btnSelectDate = new Button
                {
                    Text = "Select Date",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelectDate.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelectDate.Click += (s, e) =>
                {
                    calendarForm.Close(); // Zamknięcie formularza po wyborze daty
                };
                panel.Controls.Add(btnSelectDate);

                // Wyświetlenie formularza
                calendarForm.ShowDialog();
                return selectedDate; // Zwrócenie wybranej daty
            }
        }

        public string AddMeal()
        {
            string selectedOption = "Return"; // Domyślna opcja

            using (Form addMealForm = new Form
            {
                Text = "Add Meal Menu",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
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
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Lista opcji
                ListBox listBoxMealOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 150,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst jasny
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40, // Wyższe wiersze
                    Items = { "Meal Database", "Enter Macro", "Return" }
                };

                // Podświetlenie wybranej opcji
                listBoxMealOptions.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxMealOptions.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxMealOptions.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxMealOptions);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxMealOptions.SelectedItem != null)
                    {
                        selectedOption = listBoxMealOptions.SelectedItem.ToString();
                        addMealForm.Close(); // Zamknięcie formularza po dokonaniu wyboru
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Wyświetlenie formularza
                addMealForm.ShowDialog();
                return selectedOption; // Zwrócenie wybranej opcji
            }
        }


        public UserDTO DisplayEnterMacro(UserDTO userDTO)
        {
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
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Panel kontenerowy
                Panel panel = new Panel { Dock = DockStyle.Fill };
                macroForm.Controls.Add(panel);

                // Nagłówek formularza
                Label lblHeader = new Label
                {
                    Text = "Enter Macros:",
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 18, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60,
                    ForeColor = Color.Blue // Niebieski nagłówek
                };
                panel.Controls.Add(lblHeader);

                // Wyświetlenie aktualnych wartości makroskładników
                Label lblCurrentMacros = new Label
                {
                    Text = $"Carbs = {userDTO.Carbs}, Fats = {userDTO.Fats}, Proteins = {userDTO.Proteins}, Calories = {userDTO.Calories}",
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 60
                };
                panel.Controls.Add(lblCurrentMacros);

                // Lista opcji
                ListBox listBoxOptions = new ListBox
                {
                    Dock = DockStyle.Top,
                    Font = new Font("Consolas", 14),
                    Height = 200,
                    BackColor = Color.FromArgb(40, 40, 40), // Tło ciemne
                    ForeColor = Color.White, // Tekst biały
                    BorderStyle = BorderStyle.None, // Usunięcie obramowania
                    ItemHeight = 40, // Wyższe wiersze
                    Items = { "Carbs", "Fats", "Proteins", "Calories", "Apply/Discard", "Return" }
                };
                listBoxOptions.DrawMode = DrawMode.OwnerDrawFixed;
                listBoxOptions.DrawItem += (s, e) =>
                {
                    e.DrawBackground();
                    bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    Color backgroundColor = isSelected ? Color.FromArgb(80, 255, 0, 0) : Color.FromArgb(40, 40, 40);
                    Color textColor = Color.White;

                    using (SolidBrush backgroundBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                    }

                    using (SolidBrush textBrush = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(listBoxOptions.Items[e.Index].ToString(),
                            e.Font, textBrush, e.Bounds, StringFormat.GenericDefault);
                    }

                    e.DrawFocusRectangle();
                };

                panel.Controls.Add(listBoxOptions);

                // Przycisk "Select"
                Button btnSelect = new Button
                {
                    Text = "Select",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSelect.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSelect.Click += (s, e) =>
                {
                    if (listBoxOptions.SelectedItem != null)
                    {
                        string choice = listBoxOptions.SelectedItem.ToString();
                        userDTO.Choice = choice; // Zapisanie wyboru użytkownika
                        macroForm.Close(); // Zamknięcie formularza
                    }
                    else
                    {
                        MessageBox.Show("Please select an option before proceeding.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                panel.Controls.Add(btnSelect);

                // Przycisk "Return"
                Button btnReturn = new Button
                {
                    Text = "Return",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(255, 99, 71), // Czerwony kolor (Tomato)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnReturn.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnReturn.Click += (s, e) =>
                {
                    userDTO.Choice = "Return"; // Ustawienie wyboru na "Return"
                    macroForm.Close(); // Zamknięcie formularza
                };
                panel.Controls.Add(btnReturn);

                macroForm.ShowDialog();
            }

            return userDTO; // Zwróć zaktualizowany obiekt UserDTO
        }


        public string ApplyDiscard()
        {
            string result = ""; // Domyślny wynik

            using (Form applyDiscardForm = new Form
            {
                Text = "Apply Changes",
                Width = 640,
                Height = 480,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
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
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Height = 40,
                    ForeColor = Color.Blue
                };
                panel.Controls.Add(lblMessage);

                // Przycisk Apply (zielony)
                Button btnApply = new Button
                {
                    Text = "Apply",
                    Dock = DockStyle.Top,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(50, 205, 50), // Zielony kolor (LightGreen)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnApply.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);

                // Przycisk Discard (czerwony)
                Button btnDiscard = new Button
                {
                    Text = "Discard",
                    Dock = DockStyle.Top,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(255, 99, 71), // Czerwony kolor (Tomato)
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnDiscard.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);

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

                // Dodanie przycisków do panelu
                panel.Controls.Add(btnApply);
                panel.Controls.Add(btnDiscard);

                applyDiscardForm.ShowDialog(); // Wyświetlenie formularza
            }

            return result; // Zwrócenie wynik po zamknięciu formularza
        }


        public uint EnterUint(string text)
        {
            string userInput = EnterString(text); // Użycie EnterString do pobrania wartości
            return uint.TryParse(userInput, out uint result) ? result : 0; // Obsługa błędu parsowania
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
            string result = ""; // Domyślny wynik

            using (Form inputForm = new Form
            {
                Text = "Input Required",
                Width = 640,
                Height = 320,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Tytuł formularza w czerwonym kolorze
                inputForm.Paint += (s, e) =>
                {
                    e.Graphics.DrawString(inputForm.Text, new Font("Consolas", 16, FontStyle.Bold),
                        new SolidBrush(Color.Red), new PointF(10, 10));
                };

                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 3,
                    ColumnCount = 1,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40)); // Tekst
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40)); // Pole tekstowe
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // Przycisk
                inputForm.Controls.Add(layout);

                // Wyświetlany tekst
                Label lblPrompt = new Label
                {
                    Text = text,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 14),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.Blue // Tekst w niebieskim kolorze
                };
                layout.Controls.Add(lblPrompt, 0, 0);

                // Pole tekstowe
                TextBox tb = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 14),
                    BackColor = Color.FromArgb(40, 40, 40),
                    ForeColor = Color.White
                };
                layout.Controls.Add(tb, 0, 1);

                // Przycisk "OK"
                Button btnOk = new Button
                {
                    Text = "OK",
                    Dock = DockStyle.Fill,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnOk.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnOk.Click += (s, e) =>
                {
                    result = tb.Text; // Przypisanie wprowadzonego tekstu
                    inputForm.Close(); // Zamknięcie formularza
                };
                layout.Controls.Add(btnOk, 0, 2);

                inputForm.ShowDialog(); // Wyświetlenie formularza
            }

            return result; // Zwrócenie wprowadzonego tekstu
        }

        public string ViewEntries(List<CalorieModel> entries)
        {
            string result = "Return"; // Domyślna opcja

            using (Form mealsForm = new Form
            {
                Text = "Meals",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Ciemne tło
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Tytuł w czerwonym kolorze
                mealsForm.Paint += (s, e) =>
                {
                    e.Graphics.DrawString(mealsForm.Text, new Font("Consolas", 16, FontStyle.Bold),
                        new SolidBrush(Color.Red), new PointF(10, 10));
                };

                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 2,
                    ColumnCount = 1,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 80)); // Tabela
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 20)); // Przyciski
                mealsForm.Controls.Add(layout);

                // Tabela wyświetlająca wpisy
                DataGridView dataGrid = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoGenerateColumns = false,
                    ReadOnly = true, // Tabela tylko do odczytu
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    BackgroundColor = Color.FromArgb(40, 40, 40), // Tło tabeli
                    ForeColor = Color.White,
                    Font = new Font("Consolas", 14)
                };

                dataGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Product Name", DataPropertyName = "ProductName" });
                dataGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Calories", DataPropertyName = "Calories" });
                dataGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Proteins", DataPropertyName = "Proteins" });
                dataGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Carbs", DataPropertyName = "Carbs" });
                dataGrid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Fats", DataPropertyName = "Fats" });

                dataGrid.DataSource = new BindingSource { DataSource = entries };
                layout.Controls.Add(dataGrid, 0, 0);

                // Panel z przyciskami
                FlowLayoutPanel buttonPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.RightToLeft,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                layout.Controls.Add(buttonPanel, 0, 1);

                // Przycisk "Edit"
                Button btnEdit = new Button
                {
                    Text = "Edit",
                    Width = 150,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnEdit.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnEdit.Click += (s, e) =>
                {
                    if (dataGrid.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dataGrid.SelectedRows[0].Index;
                        if (selectedIndex >= 0 && selectedIndex < entries.Count)
                        {
                            EditEntry(entries[selectedIndex]); // Otwórz edycję wybranego wpisu
                            dataGrid.Refresh(); // Odśwież tabelę po edycji
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an entry to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                buttonPanel.Controls.Add(btnEdit);

                // Przycisk "Remove"
                Button btnRemove = new Button
                {
                    Text = "Remove",
                    Width = 150,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnRemove.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnRemove.Click += (s, e) =>
                {
                    if (dataGrid.SelectedRows.Count > 0)
                    {
                        int selectedIndex = dataGrid.SelectedRows[0].Index;
                        if (selectedIndex >= 0 && selectedIndex < entries.Count)
                        {
                            var confirmResult = MessageBox.Show("Are you sure to delete this entry?",
                                "Confirm Delete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning);

                            if (confirmResult == DialogResult.Yes)
                            {
                                entries.RemoveAt(selectedIndex); // Usuń wpis
                                dataGrid.DataSource = new BindingSource { DataSource = entries }; // Odśwież dane
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select an entry to remove.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                buttonPanel.Controls.Add(btnRemove);

                // Przycisk "Close"
                Button btnClose = new Button
                {
                    Text = "Close",
                    Width = 150,
                    Height = 50,
                    Font = new Font("Consolas", 14, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnClose.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnClose.Click += (s, e) => mealsForm.Close();
                buttonPanel.Controls.Add(btnClose);

                mealsForm.ShowDialog();
            }
            return "Return"; // Zwrócenie wartości po zakończeniu
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

        private void EditEntry(CalorieModel entry)
        {
            using (Form editForm = new Form
            {
                Text = "Edit Entry",
                Width = 1280,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(30, 30, 30), // Tło ciemne
                ForeColor = Color.White // Jasny tekst
            })
            {
                // Ustawienie tytułu formularza na czerwony
                editForm.Paint += (s, e) =>
                {
                    e.Graphics.DrawString(editForm.Text, new Font("Consolas", 16, FontStyle.Bold),
                        new SolidBrush(Color.Red), new PointF(10, 10));
                };

                TableLayoutPanel layout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    RowCount = 6,
                    ColumnCount = 2,
                    BackColor = Color.FromArgb(30, 30, 30)
                };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 85));
                editForm.Controls.Add(layout);

                // Nazwa produktu
                layout.Controls.Add(new Label
                {
                    Text = "Product Name",
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 14),
                    ForeColor = Color.Blue
                }, 0, 0);

                TextBox txtName = new TextBox
                {
                    Text = entry.ProductName,
                    Dock = DockStyle.Fill,
                    Font = new Font("Consolas", 14),
                    BackColor = Color.FromArgb(40, 40, 40),
                    ForeColor = Color.White
                };
                layout.Controls.Add(txtName, 1, 0);

                // Pole do edycji kalorii, białek, węgli, tłuszczów
                AddNumericField(layout, "Calories", entry.Calories, 1, (value) => entry.Calories = value);
                AddNumericField(layout, "Proteins", entry.Proteins, 2, (value) => entry.Proteins = value);
                AddNumericField(layout, "Carbs", entry.Carbs, 3, (value) => entry.Carbs = value);
                AddNumericField(layout, "Fats", entry.Fats, 4, (value) => entry.Fats = value);

                // Przycisk zapisu
                Button btnSave = new Button
                {
                    Text = "Save",
                    Dock = DockStyle.Bottom,
                    Height = 50,
                    Font = new Font("Consolas", 16, FontStyle.Bold),
                    BackColor = Color.FromArgb(70, 70, 70),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btnSave.FlatAppearance.BorderColor = Color.FromArgb(90, 90, 90);
                btnSave.Click += (s, e) =>
                {
                    // Zapisanie nazwy produktu
                    entry.ProductName = txtName.Text;

                    editForm.Close();
                };
                editForm.Controls.Add(btnSave);

                editForm.ShowDialog();
            }
        }

        private void AddNumericField(TableLayoutPanel layout, string label, uint value, int row, Action<uint> onSave)
        {
            layout.Controls.Add(new Label
            {
                Text = label,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 14),
                ForeColor = Color.Blue
            }, 0, row);

            NumericUpDown numericField = new NumericUpDown
            {
                Minimum = 0, // Minimalna wartość
                Maximum = 10000, // Maksymalna wartość
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 14),
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White
            };

            // Ustawienie wartości z uwzględnieniem zakresu
            numericField.Value = Math.Min(numericField.Maximum, Math.Max(numericField.Minimum, value));

            numericField.ValueChanged += (s, e) => onSave((uint)numericField.Value);
            layout.Controls.Add(numericField, 1, row);
        }

        public string EditEntries(List<CalorieModel> entries)
        {
            using (Form editForm = new Form
            {
                Text = "Edit Entries",
                Width = 1280,
                Height = 720,
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
    }
}
