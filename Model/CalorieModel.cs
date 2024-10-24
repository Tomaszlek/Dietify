namespace DietMaker.Model
{
    public class CalorieModel
    {
        public string ProductName { get; set; }
        public uint Calories { get; set; }
        public uint Fats { get; set; }
        public uint Carbs { get; set; }
        public uint Proteins { get; set; }


        public CalorieModel(uint carbs, uint fats, uint proteins, uint calories, string productname)
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
        public uint carbs_goal { get; set; }
        public uint proteins_goal { get; set; }
        public uint fats_goal { get; set; }
        public uint calories_goal { get; set; }

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
