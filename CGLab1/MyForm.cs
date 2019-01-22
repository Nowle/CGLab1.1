using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CGLab1
{
    class MyForm : Form
    {
        private static readonly Size PictureBoxSize = new Size(862, 431);

        private static readonly PictureBox PictureBox = new PictureBox
            {Location = new Point(10, 10), Size = PictureBoxSize};

        public MyForm()
        {
            var buttonBr = new Button {Text = "Брез", Size = new Size(100, 25), Location = new Point(180, 460)};
            buttonBr.Click += ButtonClickBr;
            var buttonDda = new Button {Text = "Цда", Size = new Size(100, 25), Location = new Point(380, 460)};
            buttonDda.Click += ButtonClickDda;
            Controls.Add(buttonBr);
            Controls.Add(buttonDda);
            PictureBox.Paint += DrawFigureBr;
            PictureBox.Paint += DrawFigureDda;
            Controls.Add(PictureBox);
        }
        private static void ButtonClickBr(object sender, EventArgs e)
        {
            PictureBox.Paint += DrawFigureBr;
            PictureBox.Refresh();
        }
        private static void ButtonClickDda(object sender, EventArgs e)
        {
            PictureBox.Paint += DrawFigureDda;
            PictureBox.Refresh();
        }

        private static void DrawFigureBr(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            var pixels = GetNet(g);
            var lines = GetLines();


            foreach (var line in lines)
            {
                DrawBresenham(g, pixels, line.FirstPoint, line.LastPoint);
            }

        }

        private static void DrawFigureDda(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Color.White);
            var pixels = GetNet(g);
            var lines = GetLines();

            foreach (var line in lines)
            {
                DrawDDA(g, pixels, line.FirstPoint, line.LastPoint);
            }
        }

        private static Rectangle[,] GetNet(Graphics g)
        {
            var startPoint = new Point(0, 0);
            var xSize = PictureBoxSize.Width - 1;
            var ySize = PictureBoxSize.Width - 1;
            var rects = new Rectangle[xSize, ySize];
            var p = startPoint;
            for (var y = 0; y < ySize; y++)
            {
                p.X = startPoint.X;
                for (var x = 0; x < xSize; x++)
                {
                    rects[x, y] = new Rectangle(p, new Size(5, 5));
                    p.X += 5;
                    g.FillRectangle(Brushes.WhiteSmoke, rects[x, y]);
                    g.DrawRectangle(new Pen(Brushes.Black), rects[x, y]);
                }

                p.Y += 5;
            }

            return rects;
        }

        private static List<Line> GetLines()
        {
            var listOfLines = new List<Line>
            {
                new Line(new Point(43, 7), new Point(69, 50)),
                new Line(new Point(69, 50), new Point(17, 50)),
                new Line(new Point(17, 50), new Point(43, 7)),
                new Line(new Point(43, 69), new Point(17, 26)),
                new Line(new Point(17, 26), new Point(69, 26)),
                new Line(new Point(69, 26), new Point(43, 69)),
            };

            return listOfLines;
        }



        private static void DrawBresenham(Graphics g, Rectangle[,] pixels, Point firstPoint, Point lastPoint)
        {
            var a = lastPoint.Y - firstPoint.Y;
            var b = firstPoint.X - lastPoint.X;
            var signA = 1;
            var signB = 1;
            if (a < 0)
                signA = -1;
            if (b < 0)
                signB = -1;
            var f = 0;
            var point = new Point(firstPoint.X, firstPoint.Y);
            g.FillRectangle(Brushes.Blue, pixels[point.X, point.Y]);
            g.DrawRectangle(new Pen(Brushes.Black), pixels[point.X, point.Y]);
            while (point.X != lastPoint.X || point.Y != lastPoint.Y)
            {
                if (Math.Abs(a) < Math.Abs(b))
                {
                    f += a * signA;
                    if (f > 0)
                    {
                        f -= b * signB;
                        point.Y += signA;
                    }

                    point.X -= signB;
                    g.FillRectangle(Brushes.Blue, pixels[point.X, point.Y]);
                    g.DrawRectangle(new Pen(Brushes.Black), pixels[point.X, point.Y]);
                }
                else
                {
                    f += b * signB;
                    if (f > 0)
                    {
                        f -= a * signA;
                        point.X -= signB;
                    }

                    point.Y += signA;
                    g.FillRectangle(Brushes.Blue, pixels[point.X, point.Y]);
                    g.DrawRectangle(new Pen(Brushes.Black), pixels[point.X, point.Y]);
                }
            }
        }

        private static void DrawDDA(Graphics g, Rectangle[,] pixels, Point firstPoint, Point lastPoint)
        {
            var x= new double[1000];
            var y= new double[1000];
           double xstart = firstPoint.X;
            xstart = Math.Round(xstart);
           double ystart = firstPoint.Y;
            ystart = Math.Round(ystart);
           double xend = lastPoint.X;
            xend = Math.Round(xend);
           double yend = lastPoint.Y;
            yend = Math.Round(yend);
            var l = Math.Max(Math.Abs(xend-xstart), Math.Abs(yend-ystart));
            var dX = (lastPoint.X-firstPoint.X) / l;
            var dY = (lastPoint.Y-firstPoint.Y) / l;
            var i = 0;
            x[i] = firstPoint.X;
            y[i] = firstPoint.Y;
            i++;
            while (i < l)
            {
                x[i] = x[i-1] + dX;
                y[i] = y[i-1] + dY;
                i++;
            }
            x[i] = lastPoint.X;
            y[i] = lastPoint.Y;

            i = 0;

            while (i <= l)
            {
                g.FillRectangle(Brushes.Blue, pixels[(int)x[i], (int)y[i]]);
                g.DrawRectangle(new Pen(Brushes.Black), pixels[(int)x[i], (int)y[i]]);
                i++;
            }

        }
    }
}
