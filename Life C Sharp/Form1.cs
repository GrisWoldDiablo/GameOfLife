using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Life_C_Sharp
{
    // Christmas 2021
    public partial class FormBack : Form
    {
        public int _cellSize = 50;
        public List<Cell> Cells = null;
        public List<Cell> Grid = null;
        private int _gridW;
        private int _gridH;
        private Random _random;
        private int _lastCoord;
        private bool _isDown;
        private bool? _setState = null;
        private Cell[] _saveCells;
        private Cell[] _saveGrid;
        private bool _timeStateOnDown;

        public FormBack()
        {
            InitializeComponent();
            _random = new Random((int)DateTime.Now.Ticks);
            UpdateSpeedLabel();
        }

        private void UpdateSpeedLabel()
        {
            SpeedLabel.Text = $"{timer1.Interval}";
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (Cells == null)
            {
                return;
            }

            var myBrush = new SolidBrush(Color.White);
            var gridColor = Color.FromArgb(50, Color.DarkGray);

            var myPen = new Pen(gridColor);
            //foreach (var cell in Grid)
            //{
            //    cell.Draw(myPen, e.Graphics);
            //}
            //myPen.Dispose();

            foreach (var cell in Cells)
            {
                if (cell.IsAlive)
                {
                    cell.DrawFill(myBrush, e.Graphics, _random.Next(2) == 1);
                }
                else
                {
                    cell.Draw(myPen, e.Graphics);
                }
            }
            myBrush.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Cells == null)
            {
                if (int.TryParse(textBox1.Text, out int size))
                {
                    size = Math.Min(Math.Max(size, 3), 300);
                    _cellSize = size;
                }
                else
                {
                    _cellSize = 10;
                }

                Cells = new List<Cell>();
                Grid = new List<Cell>();
                int index = 0;
                _gridH = 0;
                _gridW = 0;
                for (int y = 0; y < DisplayRectangle.Height; y += _cellSize)
                {
                    var oldH = _gridH;
                    for (int x = 0; x < DisplayRectangle.Width; x += _cellSize)
                    {
                        if (true || (x * y) % 2 == 0)
                        {
                            if (oldH == _gridH)
                            {
                                _gridH++;
                            }

                            if (y == 0)
                            {
                                _gridW++;
                            }

                            Cells.Add(new Cell(new Point(x + 1, y + 1), new Size(_cellSize - 2, _cellSize - 2), index));
                            Grid.Add(new Cell(new Point(x, y), new Size(_cellSize, _cellSize), index));
                            index++;
                        }
                    }
                }
                _gridW--;
                _gridH--;

                foreach (var cell in Cells)
                {
                    cell.SetNeighbourghs(GetNeigbourghs(cell.Index));
                }
            }
            else
            {
                MoveStep();
            }

            this.Refresh();
        }

        private void MoveStep()
        {
            if (Cells == null)
            {
                return;
            }

            foreach (var cell in Cells)
            {
                cell.Step(Cells);
            }

            foreach (var cell in Cells)
            {
                cell.Cycle();
            }
        }

        private List<int> GetNeigbourghs(int index)
        {
            var neighbourghs = new List<int>();
            var upl = -1;
            var upm = -1;
            var upr = -1;
            var l = -1;
            var r = -1;
            var dl = -1;
            var dm = -1;
            var dr = -1;
            if (index > _gridW)
            {
                if (index % (_gridW + 1) > 0)
                {
                    upl = index - _gridW - 2;
                    neighbourghs.Add(upl);
                }
                else
                {
                    upl = index - 1;
                    neighbourghs.Add(upl);
                }

                upm = index - _gridW - 1;
                neighbourghs.Add(upm);
                if (index % (_gridW + 1) < _gridW)
                {
                    upr = upm + 1;
                    neighbourghs.Add(upr);
                }
                else
                {
                    upr = upm - _gridW;
                    neighbourghs.Add(upr);
                }
            }
            else
            {
                upm = index + (_gridH * (_gridW + 1));
                neighbourghs.Add(upm);

                if (upm % (_gridW + 1) > 0)
                {
                    upl = upm - 1;
                    neighbourghs.Add(upl);
                }
                else
                {
                    upl = upm + _gridW;
                    neighbourghs.Add(upl);
                }

                if (upm % (_gridW + 1) < _gridW)
                {
                    upr = upm + 1;
                    neighbourghs.Add(upr);
                }
                else
                {
                    upr = upm - _gridW;
                    neighbourghs.Add(upr);
                }
            }

            if (index % (_gridW + 1) > 0)
            {
                l = index - 1;
                neighbourghs.Add(l);
            }
            else
            {
                l = index + _gridW;
                neighbourghs.Add(l);
            }

            if (index % (_gridW + 1) < _gridW)
            {
                r = index + 1;
                neighbourghs.Add(r);
            }
            else
            {
                r = index - _gridW;
                neighbourghs.Add(r);
            }

            if (index < (_gridW + 1) * _gridH)
            {
                if (index % (_gridW + 1) > 0)
                {
                    dl = index + _gridW;
                    neighbourghs.Add(dl);
                }
                else
                {
                    dl = index + (2 * _gridW) + 1;
                    neighbourghs.Add(dl);
                }

                dm = index + _gridW + 1;
                neighbourghs.Add(dm);
                if (index % (_gridW + 1) < _gridW)
                {
                    dr = dm + 1;
                    neighbourghs.Add(dr);
                }
                else
                {
                    dr = dm - _gridW;
                    neighbourghs.Add(dr);
                }
            }
            else
            {
                dm = index - (_gridH * _gridW) - _gridH;
                neighbourghs.Add(dm);

                if (dm % (_gridW + 1) > 0)
                {
                    dl = dm - 1;
                    neighbourghs.Add(dl);
                }
                else
                {
                    dl = dm + _gridW;
                    neighbourghs.Add(dl);
                }

                if (dm % (_gridW + 1) < _gridW)
                {
                    dr = dm + 1;
                    neighbourghs.Add(dr);
                }
                else
                {
                    dr = dm - _gridW;
                    neighbourghs.Add(dr);
                }
            }
            return neighbourghs;
        }

        private void FormBack_ResizeEnd(object sender, EventArgs e)
        {
            Clear();
        }

        private void FormBack_MouseClick(object sender, MouseEventArgs e)
        {
            if (Cells == null)
            {
                return;
            }

            GetCoord(e, out int x, out int y, out int coord);
            //Cells[coord].IsAlive = !Cells[coord].IsAlive;
            LabelClick.Text = $"[{x},{y}]({_gridW}x{_gridH})[{coord}]";
            this.Refresh();
        }

        private void GetCoord(MouseEventArgs e, out int x, out int y, out int coord)
        {
            x = (int)Math.Floor(e.X / (float)_cellSize);
            y = (int)Math.Floor(e.Y / (float)_cellSize);
            coord = y * _gridW + x + y;
        }

        private void FormBack_MouseMove(object sender, MouseEventArgs e)
        {
            LabelCoord.Text = $"({e.X},{e.Y})";
            UpdateDown(e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MoveStep();
            this.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Cells == null)
            {
                return;
            }

            foreach (var cell in Cells)
            {
                cell.IsAlive = _random.Next(2) == 1;
            }

            this.Refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            Cells = null;
            this.Refresh();
        }

        private void FormBack_MouseDown(object sender, MouseEventArgs e)
        {
            _isDown = true;
            UpdateDown(e);
            _timeStateOnDown = timer1.Enabled;
            if (_timeStateOnDown)
            {
                timer1.Stop();
            }
        }

        private void UpdateDown(MouseEventArgs e)
        {
            if (!_isDown || Cells == null)
            {
                return;
            }

            GetCoord(e, out int x, out int y, out var coord);
            if (coord != _lastCoord)
            {
                Cells[coord].IsAlive = _setState ?? !Cells[coord].IsAlive;
                _setState = Cells[coord].IsAlive;
                _lastCoord = coord;
                this.Refresh();
            }
        }

        private void FormBack_MouseUp(object sender, MouseEventArgs e)
        {
            _isDown = false;
            _setState = null;
            if (_timeStateOnDown)
            {
                timer1.Start();
            }

            _lastCoord = -1;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                PlayButton.BackColor = Color.Red;
                timer1.Stop();
            }
            else
            {
                PlayButton.BackColor = Color.Lime;
                timer1.Start();
            }
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void PlusButton_Click(object sender, EventArgs e)
        {
            timer1.Interval += 10;
            UpdateSpeedLabel();
        }

        private void MinusButton_Click(object sender, EventArgs e)
        {
            timer1.Interval -= timer1.Interval > 10 ? 10 : timer1.Interval == 10 ? 9 : 0;
            UpdateSpeedLabel();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            _saveCells = new Cell[Cells.Count];
            for (int i = 0; i < Cells.Count; i++)
            {
                _saveCells[i] = Cells[i].Clone() as Cell;
            }

            _saveGrid = new Cell[Grid.Count];
            for (int i = 0; i < Grid.Count; i++)
            {
                _saveGrid[i] = Grid[i].Clone() as Cell;
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Cells = new List<Cell>();
            for (int i = 0; i < _saveCells.Length; i++)
            {
                Cells.Add(_saveCells[i].Clone() as Cell);
            }

            Grid = new List<Cell>();
            for (int i = 0; i < _saveGrid.Length; i++)
            {
                Grid.Add(_saveGrid[i].Clone() as Cell);
            }

            this.Refresh();
        }

        private void FillButton_Click(object sender, EventArgs e)
        {
            if (Cells != null)
            {
                Cells.ForEach((x) => { x.IsAlive = true; });
            }
            this.Refresh();
        }
    }
}
