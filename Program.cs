using System;
using System.Threading.Tasks;
using DietMaker.Controller;

namespace DietMaker
{
    class Program
    {
        static async Task Main(string[] args)  // Changed to async Task
        {
            CalorieController controller = new CalorieController();
            await controller.Run();  // Await the Run method if it's async
        }
    }
}
