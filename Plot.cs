using System;
using System.Collections.Generic;
using System.Linq;

using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphPlotter
{
    class Plot
    {
        Canvas canvas;
        List<double> datas;
        Point position, size;
        Axis x, y;

        public List<double> Datas => datas;

        public Axis X => x;
        public Axis Y => y;

        public Plot(Canvas canvas, List<double> datas, int posX, int posY, int width, int height, double offsetX, double offsetY, double stepX, double stepY, int stepCountX, int stepCountY, AxisType patternX, AxisType patternY)
        {
            this.canvas = canvas;
            this.datas = datas;
            this.position = new Point(posX, posY);
            this.size = new Point(width, height);
            this.x = new Axis(stepX, stepCountX, offsetX, width, patternX, AxisDirection.Horizontal, false);
            this.y = new Axis(stepY, stepCountY, offsetY, height, patternY, AxisDirection.Vertical, false);
        }

        public void DrawCartesian()
        {
            x.Draw(canvas, position.X, position.Y, y.Length);
            y.Draw(canvas, position.Y, position.X, x.Length);
        }

        public void DrawCurve()
        {
            Brush b = new SolidColorBrush(Colors.MediumVioletRed);
            List<double> tmpList = datas.GetRange(1, datas.Count - 1);
            Line[] lines = new Line[tmpList.Count];
            double maxValue = datas.Max();
            for (int i = 0; i < tmpList.Count; i++)
            {
                lines[i] = new Line();
                lines[i].Stroke = b;
                lines[i].StrokeThickness = 3;
                switch (x.Type)
                {
                    case AxisType.Linear:
                        lines[i].X1 = position.X + i * size.X / x.Count;
                        lines[i].X2 = position.X + (i + 1) * size.X / x.Count;
                        break;
                    case AxisType.Bode:
                        lines[i].X1 = position.X + Math.Log10(i + 1) * size.X / Math.Log10(x.Count);
                        lines[i].X2 = position.X + Math.Log10(i + 2) * size.X / Math.Log10(x.Count);
                        break;
                }
                lines[i].Y1 = position.Y + size.Y - (datas[i] * size.Y / maxValue);
                lines[i].Y2 = position.Y + size.Y - (tmpList[i] * size.Y / maxValue);
                canvas.Children.Add(lines[i]);
            }
        }

        public void DrawBoth()
        {
            DrawCartesian();
            DrawCurve();
        }
    }
}
