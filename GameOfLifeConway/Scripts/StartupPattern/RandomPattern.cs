using System;
using System.Collections.Generic;

namespace GameOfLifeConway
{
    public class RandomPattern : IStartupPattern
    {
        private Random random;

        public RandomPattern()
        {
            random = new Random();
        }

        public RandomPattern(int seed)
        {
            random = new Random(seed);
        }

        public void Setup(LifeCell[,] cells)
        {
            foreach (var cell in cells)
                cell.IsAlive = random.Next(0, 2) == 0;
        }
    }
}
