using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GameOfLifeConway
{
    public class LifeCell : BoxView
    {
        private bool isAlive = false;
        private Color aliveColor = Color.White, deadColor = Color.Black;

        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                if (isAlive == value)
                    return;

                isAlive = value;
                BackgroundColor = isAlive ? aliveColor : deadColor; 
            }
        }

        public Color AliveColor
        {
            get { return aliveColor; }
            set
            {
                if (aliveColor == value)
                    return;

                aliveColor = value;
                if (isAlive)
                    BackgroundColor = aliveColor;
            }
        }

        public Color DeadColor
        {
            get { return deadColor; }
            set
            {
                if (deadColor == value)
                    return;

                deadColor = value;
                if (!isAlive)
                    BackgroundColor = deadColor;
            }
        }

        public List<LifeCell> Neighbors { get; private set; }
        public int Row { get; set; }
        public int Collumn { get; set; }

        public LifeCell(int row, int collumn)
        {
            Row = row;
            Collumn = collumn;
            Neighbors = new List<LifeCell>();

            BackgroundColor = DeadColor;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnCellTapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Should this cell still be alive in next update ?
        /// </summary>
        public virtual bool GetNextState()
        {
            if (Neighbors == null || Neighbors.Count < 1)
                return false;

            var aliveNeighborsCount = 0;
            foreach(var cell in Neighbors)
            {
                if (cell.IsAlive)
                    aliveNeighborsCount++;
            }

            if (!IsAlive)
                return aliveNeighborsCount == 3;

            if (aliveNeighborsCount < 2 || aliveNeighborsCount > 3)
                return false;

            return IsAlive;
        }

        protected virtual void OnCellTapped(object _, EventArgs __)
        {
            IsAlive = !IsAlive;
        }
    }
}
