namespace DietMaker.Model
{
    // Model reprezentujący dane o posiłku
    public class CalorieModel
    {
        public string ProductName { get; set; }
        public uint Calories { get; set; }
        public uint Fats { get; set; }
        public uint Carbs { get; set; }
        public uint Proteins { get; set; }

        public CalorieModel(uint carbs, uint fats, uint proteins, uint calories, string productName)
        {
            Carbs = carbs;
            Fats = fats;
            Proteins = proteins;
            Calories = calories;
            ProductName = productName;
        }

        public CalorieModel()
        {
            Carbs = 0;
            Fats = 0;
            Proteins = 0;
            Calories = 0;
            ProductName = string.Empty;
        }

        // Konwersja na UserDTO
        public UserDTO ToDTO()
        {
            return new UserDTO
            {
                ProductName = this.ProductName,
                Carbs = (int)this.Carbs,
                Fats = (int)this.Fats,
                Proteins = (int)this.Proteins,
                Calories = (int)this.Calories
            };
        }
    }

    // Model reprezentujący dane użytkownika i cele żywieniowe
    public class UserModel
    {
        public string UserName { get; set; }
        public uint CarbsGoal { get; set; }
        public uint ProteinsGoal { get; set; }
        public uint FatsGoal { get; set; }
        public uint CaloriesGoal { get; set; }

        public UserModel()
        {
            UserName = string.Empty;
            CarbsGoal = 0;
            ProteinsGoal = 0;
            FatsGoal = 0;
            CaloriesGoal = 0;
        }
    }

    // DTO używany do przesyłania danych między widokiem a kontrolerem
    public class UserDTO
    {
        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Proteins { get; set; }
        public string ProductName { get; set; }
        public string Choice { get; set; }
        public float PartialAmount { get; set; }
        public int GramAmount { get; set; }

        public UserDTO()
        {
            ResetValues();
        }

        // Metoda resetująca wartości DTO
        public void ResetValues()
        {
            Calories = 0;
            Fats = 0;
            Carbs = 0;
            Proteins = 0;
            GramAmount = 0;
            PartialAmount = 1.0f;
            ProductName = string.Empty;
            Choice = string.Empty;
        }
    }
}
