using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace computershop.View
{
    /// <summary>
    /// Interaction logic for HomeWindowUserControl.xaml
    /// </summary>
    public partial class HomeWindowUserControl : UserControl
    {
        public HomeWindowUserControl()
        {
            InitializeComponent();
        }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = chartPoint.ChartView as PieChart;
            foreach(PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }
            var selectedSeries = chartPoint.SeriesView as PieSeries;
            selectedSeries.PushOut = 15;
        }
    }
}
