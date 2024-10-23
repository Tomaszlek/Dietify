namespace DietMaker.Model
{
    public class CalorieModel
    {
        public string ProductName { get; set; }
        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Proteins { get; set; }


        public CalorieModel(string productName, int calories)
        {
            ProductName = productName;
            Calories = calories;
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

    public struct DTO
    {
        int Calories;
        int Fats;
        int Carbs;
        int Proteins;
        string product_name;
        float partial_ammount;
        int gram_ammount;
    }

}
