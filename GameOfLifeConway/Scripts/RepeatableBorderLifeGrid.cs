using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLifeConway
{
    public class RepeatableBorderLifeGrid : LifeGrid
    {
        public RepeatableBorderLifeGrid(IStartupPattern startupPattern) : base(startupPattern) { }

        protected override IEnumerable<LifeCell> GetNeighbors(int x, int y)
        {
            foreach (var coordinate in NeighborsCoordinate)
            {
                var newX = x + coordinate.Item1;
                var newY = y + coordinate.Item2;

                if (newX < 0)
                    newX = Cells.GetLength(0) - 1;

                if (newY < 0)
                    newY = Cells.GetLength(1) - 1;

                if (newX >= Cells.GetLength(0))
                    newX = 0;

                if (newY >= Cells.GetLength(1))
                    newY = 0;

                yield return Cells[newX, newY];
            }
        }
    }
}
