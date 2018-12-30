using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;

namespace GameOfLifeConway
{
    public class LifeCell : BoxView
    {
        private bool isAlive = false;
        private Color aliveColor = Color.ForestGreen, deadColor = Color.OrangeRed;

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
            BackgroundColor = DeadColor;
            IsAlive = new Random().Next(0, 4) != 0;
            Row = row;
            Collumn = collumn;
            Neighbors = new List<LifeCell>();
        }

        /// <summary>
        /// Should this cell still be alive in next update ?
        /// </summary>
        public virtual bool GetNextState()
        {
            if (Neighbors == null || Neighbors.Count < 1)
                return false;

            var aliveNeighborsCount = Neighbors.Where(cell => cell.IsAlive).Count();

            if (!IsAlive)
                return aliveNeighborsCount == 3;

            if (aliveNeighborsCount < 2 || aliveNeighborsCount > 3)
                return false;

            return IsAlive;
        }
    }
}
