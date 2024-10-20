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

}
