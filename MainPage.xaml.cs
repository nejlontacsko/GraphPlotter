using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GraphPlotter
{
    public sealed partial class MainPage : Page
    {
        Plot p;
        double i = 0;
        int sampleCount = 4;
        public MainPage()
        {
            this.InitializeComponent();
            double[] wave = new double[sampleCount];
            for (; i < sampleCount; i++)
            {
                wave[(int)i] = Math.Sin(i * 6) + 2;
            }
            p = new Plot(canvas: canv,
                datas: new List<double>(wave),
                posX: 10, posY: 10, width: 1200, height: 300, offsetX: 0, offsetY: 0, stepX: 2.3, stepY: 1,
                stepCountX: sampleCount, stepCountY: 40, patternX: AxisType.Bode, patternY: AxisType.Linear);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            p.Datas.Add(Math.Sin(i * 6) + 2);
            p.X.IncrementCount(1);
            i++;
            canv.Children.Clear();
            p.DrawBoth();
        }
    }
}
