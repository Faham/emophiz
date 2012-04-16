using System;

namespace Minigames
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (MiniGamesCore game = new MiniGamesCore())
            {
                game.Run();
            }
        }
    }
#endif
}

