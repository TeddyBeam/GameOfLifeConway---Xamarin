using System;
using System.Diagnostics;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GameOfLifeConway
{
    public class LifeGrid : Grid
    {
        private int size = 40;
        private float refreshRate = 1 / 5f;
        private LifeCell[,] cells = null;

        private Tuple<int, int>[] neighborsCoordinate =
        {
            new Tuple<int, int>(-1, 0), // East
            new Tuple<int, int>(1, 0), // West
            new Tuple<int, int>(0, -1), // North
            new Tuple<int, int>(0, 1), // South
            new Tuple<int, int>(-1, -1), // Southeast
            new Tuple<int, int>(-1, 1), // Northeast
            new Tuple<int, int>(1, -1), // Northwest
            new Tuple<int, int>(1, 1), // Southwest
        };

        public int Size
        {
            get { return size; }
            set
            {
                if (size == value || size < 1)
                    return;

                size = value;
                /// Update all the cells...
            }
        }

        public LifeGrid()
        {
            Padding = new Thickness(5, 5, 5, 5);
            BackgroundColor = Color.White;
            ColumnSpacing = 1.1d;
            RowSpacing = 1.1d;

            UpdateGridDefinitions();
            CreateCells();
            UpdateNeighbors();

            Device.StartTimer(TimeSpan.FromSeconds(refreshRate), OnTimerTick);
        }

        protected virtual bool OnTimerTick()
        {
            UpdateCells();
            return true;
        }

        private void UpdateGridDefinitions()
        {
            var rowDefinitions = new RowDefinitionCollection();
            var columnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < Size; i++)
            {
                columnDefinitions.Add(new ColumnDefinition { Width = 7 });
                rowDefinitions.Add(new RowDefinition { Height = 7 });
            }
            RowDefinitions = rowDefinitions;
            ColumnDefinitions = columnDefinitions;
        }

        private void CreateCells()
        {
            cells = new LifeCell[Size, Size];
            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y] = new LifeCell(x, y);
                    Children.Add(cells[x, y], x, y);
                }
            }
        }

        private void UpdateNeighbors()
        {
            foreach(var cell in cells)
            {
                var neighbors = GetNeighbors(cell.Row, cell.Collumn);
                cell.Neighbors.AddRange(neighbors);
            }
        }

        private void UpdateCells()
        {
            var nextStates = new List<bool>();

            foreach (var cell in cells)
                nextStates.Add(cell.GetNextState());

            int i = 0;
            foreach(var cell in cells)
            {
                cell.IsAlive = nextStates[i];
                i++;
            }
        }

        protected virtual IEnumerable<LifeCell> GetNeighbors(int x, int y)
        {
            if (x < 0 || y < 0 || cells == null)
                yield break;

            foreach(var coordinate in neighborsCoordinate)
            {
                var newX = x + coordinate.Item1;
                var newY = y + coordinate.Item2;

                if (newX > 0 && x + newX < cells.GetLength(0) && newY > 0 && y + newY < cells.GetLength(1))
                    yield return cells[newX, newY];
            }
        }
    }
}
