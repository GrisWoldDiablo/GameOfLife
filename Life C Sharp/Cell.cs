using System;
using System.Collections.Generic;
using System.Drawing;

namespace Life_C_Sharp
{
    public class Cell : ICloneable
    {
        public Point Position;
        public bool IsAlive;
        public bool NextState;

        private Rectangle _rect;
        private List<int> _neighbourghs;

        public int Index { get; private set; }

        public Cell(Point position, Size size, int index)
        {
            Position = position;
            IsAlive = false;
            _rect = new Rectangle(position, size);
            Index = index;
        }

        public Cell(Cell cell)
        {
            Position = cell.Position;
            IsAlive = cell.IsAlive;
            _rect = cell._rect;
            Index = cell.Index;
            _neighbourghs = cell._neighbourghs;
        }

        public void DrawFill(Brush b, Graphics g, bool isSquare = true)
        {
            if (IsAlive)
            {
                if (isSquare)
                {
                    g.FillRectangle(b, _rect);

                }
                else
                {
                    g.FillEllipse(b, _rect);
                }
            }
        }

        public void Draw(Pen p, Graphics g)
        {
            g.DrawRectangle(p, _rect);
        }

        public void Step(List<Cell> cells)
        {
            int aliveNeighbourghs = 0;
            foreach (var neighbourgh in _neighbourghs)
            {
                if (cells[neighbourgh].IsAlive)
                {
                    aliveNeighbourghs++;
                }
            }
            NextState = IsAlive && aliveNeighbourghs == 2 || aliveNeighbourghs == 3;
        }

        public void Cycle()
        {
            IsAlive = NextState;
        }

        public void SetNeighbourghs(List<int> neighbourgs)
        {
            _neighbourghs = neighbourgs;
        }

        public object Clone()
        {
            return new Cell(this);
        }
    }
}
