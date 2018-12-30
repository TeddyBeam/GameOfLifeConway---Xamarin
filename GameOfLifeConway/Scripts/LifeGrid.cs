using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GameOfLifeConway
{
    public class LifeGrid : Grid
    {
        private int width = 40, heigh = 70;
        private float refreshRate = 1 / 15f;
        private LifeCell[,] cells = null;
        private IStartupPattern startupPattern;

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

        public LifeGrid(IStartupPattern startupPattern)
        {
            if (startupPattern == null)
                return;

            this.startupPattern = startupPattern;
            Padding = new Thickness(8, 8, 5, 5);
            BackgroundColor = Color.White;
            ColumnSpacing = 1d;
            RowSpacing = 1d;

            UpdateGridDefinitions();
            CreateCells();
            UpdateNeighbors();
            startupPattern.Setup(cells);

            Device.StartTimer(TimeSpan.FromSeconds(refreshRate), OnTimerTick);
        }

        protected virtual bool OnTimerTick()
        {
            UpdateCells();
            return true;
        }

        protected virtual IEnumerable<LifeCell> GetNeighbors(int x, int y)
        {
            if (x < 0 || y < 0 || cells == null)
                yield break;

            foreach (var coordinate in neighborsCoordinate)
            {
                var newX = x + coordinate.Item1;
                var newY = y + coordinate.Item2;

                if (newX > 0 && newX < cells.GetLength(0) && newY > 0 && newY < cells.GetLength(1))
                    yield return cells[newX, newY];
            }
        }

        protected virtual void UpdateCells()
        {
            var nextStates = new List<bool>();

            foreach (var cell in cells)
                nextStates.Add(cell.GetNextState());

            int i = 0;
            foreach (var cell in cells)
            {
                cell.IsAlive = nextStates[i];
                i++;
            }
        }

        protected virtual void SetupStartupPattern()
        {
            if (cells == null || cells.Length < 1)
                return;

            for(int x = 0; x < cells.GetLength(0); x++)
            {
                for(int y = 0; y < cells.GetLength(1); y++)
                {
                    cells[x, y].IsAlive = width % (x + 1) > heigh % (y + 1);
                }
            }
        
        }

        private void UpdateGridDefinitions()
        {           
            var columnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < width; i++)
                columnDefinitions.Add(new ColumnDefinition { Width = 7.625f });
            ColumnDefinitions = columnDefinitions;

            var rowDefinitions = new RowDefinitionCollection();
            for (int i = 0; i < heigh; i++)
                rowDefinitions.Add(new RowDefinition { Height = 7.625f });
            RowDefinitions = rowDefinitions;
        }

        private void CreateCells()
        {
            cells = new LifeCell[width, heigh];
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
    }
}
