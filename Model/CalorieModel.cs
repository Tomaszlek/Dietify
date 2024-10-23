namespace DietMaker.Model
{
    public class CalorieModel
    {
        public string ProductName { get; set; }
        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Proteins { get; set; }


        public CalorieModel(int carbs, int fats, int proteins, int calories, string productname)
        {
            Carbs = carbs;
            Fats = fats;
            Proteins = proteins;
            Calories = calories;
            ProductName = productname;
        }

        public CalorieModel() {
            Carbs = 0;
            Fats = 0;
            Proteins = 0;
            Calories = 0;
            ProductName = string.Empty;
        }

    }

    public class UserModel
    {
        public string UserName { get; set; }
        public int carbs_goal { get; set; }
        public int proteins_goal { get; set; }
        public int fats_goal { get; set; }
        public int calories_goal { get; set; }

    }

    public class DTO
    {
        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Proteins { get; set; }
        public string product_name { get; set; }
        public string choice { get; set; }
        public float partial_ammount { get; set; }
        public int gram_ammount { get; set; }

        public DTO()
        {
            Calories = 0;
            Fats = 0;
            Carbs = 0;
            Proteins = 0;
            gram_ammount = 0;
            partial_ammount = 1;
            product_name = string.Empty;
            choice = string.Empty;
        }

        public void reset_values()
        {
            Calories = 0;
            Fats = 0;
            Carbs = 0;
            Proteins = 0;
            gram_ammount = 0;
            partial_ammount = 1;
            product_name = string.Empty;
            choice = string.Empty;
        }

    }

}
