using System;

namespace GermGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (GermGame game = new GermGame())
            {
                game.Run();
            }
        }
    }
}

