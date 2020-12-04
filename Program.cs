using System;
using System.Globalization;

namespace MineSweepCS
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static long nesting = 0;

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

            int cols = int.Parse(args[0]), rows = int.Parse(args[1]);
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                     NumberStyles.AllowExponent;
            var provider = CultureInfo.CreateSpecificCulture("en-GB");
            double concentration = double.Parse(args[2], styles, provider);
            using (var game = new MineSweeper(cols, rows, concentration)) game.Run();
        }
    }
#endif
}
