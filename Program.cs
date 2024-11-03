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
            // Definicja klatek animacji
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
            int delay = 250;  // Opóźnienie między klatkami w milisekundach
            int totalDuration = 4500;  // Całkowity czas trwania animacji w milisekundach

            // Obliczamy liczbę powtórzeń animacji w zależności od czasu trwania
            int repetitions = totalDuration / (frameCount * delay);

            for (int j = 0; j < repetitions; j++)
            {
                for (int i = 0; i < frameCount; i++)
                {
                    if (Console.KeyAvailable == true)
                    {
                        return;
                    }

                    Console.Clear();
                    AnsiConsole.
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
        static async Task Main(string[] args)  // Changed to async Task
        {
            PlayAsciiAnimation();
            
            CalorieController controller = new CalorieController();
            await controller.Run();  // Await the Run method if it's async
        }
    }
}
