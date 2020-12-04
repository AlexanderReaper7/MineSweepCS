using System;
using System.Collections.Generic;

namespace MineSweepCS
{
    public enum CellState
    {
        Hidden,
        Revealed,
        Flagged
    }
    public class MineField
    {
        static readonly (int, int)[] Neighbours = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        
        public List<List<byte?>> Mines;
        public List<List<CellState>> States;

        public MineField(int cols, int rows, double mineConcentration)
        {
            Mines = new List<List<byte?>>();
            Random rand = new Random(69696969 + DateTime.Now.Millisecond + DateTime.Now.DayOfYear - DateTime.Now.Second);
            for (int i = 0; i < cols; i++)
            {
                List<byte?> row = new List<byte?>();
                for (int j = 0; j < rows; j++) row.Add(RandFunc(mineConcentration, rand) ? null : (byte?)0);
                Mines.Add(row);
            }
            CountMines();

            States = new List<List<CellState>>();
            for (int i = 0; i < cols; i++)
            {
                List<CellState> row = new List<CellState>();
                for (int j = 0; j < rows; j++) row.Add(CellState.Revealed);
                States.Add(row);
            }

        }

        public int Cols => States.Count;

        public int Rows => States[0].Count;

        /// <summary>
        ///  Returns false if revealed cell is a mine.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool RevealCell(int x, int y)
        {
            if (States[x][y] == CellState.Hidden)
            {
                States[x][y] = CellState.Revealed;
                if (Mines[x][y] is byte)
                {
                    if (Mines[x][y].Value == 0)
                    {
                        Console.WriteLine($"n: {++Program.nesting}");
                        foreach ((int, int) neighbour in Neighbours)
                        {
                            try { RevealCell(x + neighbour.Item1, y + neighbour.Item2); }
                            catch { }
                        }
                        --Program.nesting;
                    }
                    return true;
                }
            }
            return false;
        }

        public bool FlagCell(int x, int y)
        {
            States[x][y] = CellState.Flagged;
            return Mines[x][y] is null;
        }

        public bool UnFlagCell(int x, int y)
        {
            States[x][y] = CellState.Hidden;
            return Mines[x][y] is null;
        }



        static bool RandFunc(double chance, Random rand)
        {
            return rand.NextDouble() <= chance;
        }

        void CountMines()
        {
            for (int x = 0; x < Mines.Count; x++)
            {
                for (int y = 0; y < Mines[x].Count; y++)
                {
                    if (Mines[x][y] is byte)
                    {
                        foreach ((int, int) i in Neighbours)
                        {
                            try
                            {
                                if (Mines[x+i.Item1][y+i.Item2] is null) Mines[x][y]++;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}