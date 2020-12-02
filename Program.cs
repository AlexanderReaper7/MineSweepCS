using System;

namespace MineSweepCS
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                Console.WriteLine($"arg: {arg}");
            }

            int cols = int.Parse(args[1]), rows = int.Parse(args[2]);
            double concentration = double.Parse(args[3]);
            using (var game = new MineSweeper(cols, rows, concentration)) game.Run();
        }
    }
#endif
}
