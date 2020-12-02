using System;
using System.CodeDom;
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
        public readonly List<List<byte?>> Mines;
        public List<List<CellState>> States;

        public MineField(int cols, int rows, double mineConcentration)
        {
            Mines = new List<List<byte?>>();
            Random rand = new Random(69696969 + DateTime.Now.Millisecond + DateTime.Now.DayOfYear - DateTime.Now.Second);
            for (int i = 0; i < cols; i++)
            {
                List<byte?> row = new List<byte?>();
                for (int j = 0; j < rows; j++)
                {
                    row.Add(RandFunc(mineConcentration, rand) ? null : (byte?)0);
                }
                Mines.Add(row);
            }
            CountMines();

            States = new List<List<CellState>>();
            for (int i = 0; i < cols; i++)
            {
                List<CellState> row = new List<CellState>();
                for (int j = 0; j < rows; j++)
                {
                    row.Add(CellState.Hidden);
                }
                States.Add(row);
            }

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
                    if (Mines[x][y] is byte b)
                    {
                        foreach ((int, int) i in new []{(-1,-1), (-1,0), (-1,1), (0,-1), (0,1), (1,-1), (1,0), (1,1)})
                        {
                            try
                            {
                                if (Mines[x+i.Item1][y+i.Item2] is null) Mines[x][y]++;
                            }
                            catch (IndexOutOfRangeException)
                            {
                            }
                        }
                    }
                }
            }
        }
    }
}