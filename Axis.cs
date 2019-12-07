using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GraphPlotter
{
    enum AxisType
    {
        Linear,
        Bode
    }

    enum AxisDirection
    {
        Horizontal,
        Vertical
    }

    class Axis
    {
        double scalar, offset, length;
        int count;
        AxisType type;
        AxisDirection direction;
        bool fromOrigo;

        public int Count => count;
        public double Length => length;

        public AxisType Type => type;

        public Axis(double scalar, int count, double offset, double length, AxisType type, AxisDirection direction, bool fromOrigo)
        {
            this.scalar = scalar;
            this.count = count;
            this.offset = offset;
            this.length = length;
            this.type = type;
            this.direction = direction;
            this.fromOrigo = fromOrigo;
        }

        public void IncrementCount(int amount) => count += amount;

        public void Draw(Canvas canvas, double x, double y, double anotherAxisLength)
        {
            Brush b = new SolidColorBrush(Colors.LightGray);
            double step, log = Math.Log10(count), w = Math.Sqrt(75);
            Line[] lines = new Line[count];
            Polygon triangle = new Polygon();

            for (int i = 0; i < count - 1; i++)
            {
                lines[i] = new Line();
                lines[i].Stroke = b;
                lines[i].StrokeThickness = 1;

                switch (type)
                {
                    case AxisType.Linear:
                        step = (i + 1) * length / count;
                        switch (direction)
                        {
                            case AxisDirection.Vertical:
                                lines[i].X1 = x + 2;
                                lines[i].X2 = x + anotherAxisLength;
                                lines[i].Y1 = lines[i].Y2 = step + y;
                                break;
                            case AxisDirection.Horizontal:
                                lines[i].X1 = lines[i].X2 = step + x;
                                lines[i].Y1 = y + 2;
                                lines[i].Y2 = y + anotherAxisLength;
                                break;
                        }
                        break;
                    case AxisType.Bode:
                        step = Math.Log10(i + 1) * length / log;
                        switch (direction)
                        {
                            case AxisDirection.Vertical:
                                lines[i].X1 = x + 2;
                                lines[i].X2 = x + anotherAxisLength;
                                lines[i].Y1 = lines[i].Y2 = step + y;
                                break;
                            case AxisDirection.Horizontal:
                                lines[i].X1 = lines[i].X2 = step + x;
                                lines[i].Y1 = y + 2;
                                lines[i].Y2 = y + anotherAxisLength;
                                break;
                        }
                        break;
                }

                canvas.Children.Add(lines[i]);
            }

            b = new SolidColorBrush(Colors.Black);
            triangle.Fill = b;
            lines[count - 1] = new Line();
            lines[count - 1].Stroke = b;
            lines[count - 1].StrokeThickness = 2;

            switch (direction)
            {
                case AxisDirection.Vertical:
                    triangle.Points = new PointCollection()
                    {
                        new Point(x, y),
                        new Point(x + 5, y + w),
                        new Point(x - 5, y + w)
                    };

                    lines[count - 1].X1 = lines[count - 1].X2 = x;
                    lines[count - 1].Y1 = y;
                    lines[count - 1].Y2 = y + length * (fromOrigo ? 2 : 1);
                    break;
                case AxisDirection.Horizontal:
                    triangle.Points = new PointCollection()
                    {
                        new Point(x + length, y + anotherAxisLength),
                        new Point(y + length - 15 + w, x + anotherAxisLength + 5),
                        new Point(y + length - 15 + w, x + anotherAxisLength - 5)
                    };

                    lines[count - 1].X1 = x;
                    lines[count - 1].X2 = x + length * (fromOrigo ? 2 : 1);
                    lines[count - 1].Y1 = lines[count - 1].Y2 = y + anotherAxisLength;
                    break;
            }

            canvas.Children.Add(triangle);
            canvas.Children.Add(lines[count - 1]);

        }
    }
}