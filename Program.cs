using System;
using System.Threading.Tasks;
using DietMaker.Controller;
using Spectre.Console;

namespace DietMaker
{
    class Program
    {
        static void PlayAsciiAnimation()
        {
            string[] words = new string[]
            {
                @"            YOU", @"            ARE", @"            THE", @"            BEST", @"            HURRAY!!!"
            };
            
            string[] frames = new string[]
            {
            @"
             O
            /|\
            / \
           ",
            @"
             O
            /|\
            /  
           ",
            @"
             O
            /|\
             \
           ",
            @"
             O
            /|\
            / \
           ",
            @"
            \O/
             |
            / \
           "
            };

            int frameCount = frames.Length;
            int delay = 300;  
            int totalDuration = 4500;  

            
            int repetitions = totalDuration / (frameCount * delay);

            Console.CursorVisible = false;
            
            for (int j = 0; j < repetitions; j++)
            {
                for (int i = 0; i < frameCount; i++)
                {
                    if (Console.KeyAvailable == true)
                    {
                        return;
                    }

                    Console.Clear();
                    Console.SetCursorPosition(0,9);
                    Console.WriteLine(frames[i]);
                    Console.WriteLine(words[i]);
                    Thread.Sleep(delay);
                    
                }
                if (Console.KeyAvailable == true)
                {
                    Console.Clear();
                    return;
                }
            }
        }
        static async Task Main(string[] args)  
        {
            PlayAsciiAnimation();

            CalorieController controller = new CalorieController();
            await controller.Run();  
        }
    }
}
