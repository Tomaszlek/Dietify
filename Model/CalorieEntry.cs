namespace DietMaker.Model
{
    public class CalorieEntry
    {
        public string ProductName { get; set; }
        public int Calories { get; set; }

        public CalorieEntry(string productName, int calories)
        {
            ProductName = productName;
            Calories = calories;
        }
    }
}
