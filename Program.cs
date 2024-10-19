using DietMaker.Controller;

namespace DietMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            CalorieController controller = new CalorieController();
            controller.Run();
           
        }
    }
}
