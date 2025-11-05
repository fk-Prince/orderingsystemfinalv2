using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media;
using Guna.UI2.WinForms;
using LiveCharts;
using LiveCharts.Wpf;
using OrderingSystem.Repository.Reports;
using OrderingSystem.Services;
using Axis = LiveCharts.Wpf.Axis;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;
using SeriesCollection = LiveCharts.SeriesCollection;

namespace OrderingSystem.CashierApp.Layout
{
    public partial class Dashboard : Form
    {
        private readonly ReportServices ir;

        public Dashboard()
        {
            InitializeComponent();
            ir = new ReportServices(new ReportRepository());
            refresh.Start();
        }

        private void Dashboard_Load(object sender, System.EventArgs e)
        {
            fetchData();
        }

        public void fetchData()
        {
            try
            {
                Tuple<string, string> trans = ir.getTransactionByDate(date.Value);
                displayPercentage(transactionT, transactionP, transactionI, trans);
                if (double.TryParse(trans.Item2, out double percent))
                {
                    double stars = (percent / 100) * 5;
                    rating.Value = (float)stars;
                    if (stars >= 4)
                    {
                        rating.FillColor = Color.LimeGreen;
                        rating.RatingColor = Color.LimeGreen;
                    }
                    else if (stars >= 2.5)
                    {
                        rating.FillColor = Color.Gold;
                        rating.RatingColor = Color.Gold;
                    }
                    else
                    {
                        rating.FillColor = Color.Red;
                        rating.RatingColor = Color.Red;
                    }
                }

                Tuple<string, string> totalOrder = ir.getTotalOrderByType(date.Value, "");
                display(orderT, orderP, orderI, totalOrder);

                Tuple<string, string> totalComplate = ir.getTotalOrderByType(date.Value, "paid");
                display(comT, comP, comI, totalComplate);

                Tuple<string, string> totalCancelled = ir.getTotalOrderByType(date.Value, "Cancelled");
                display(coT, coP, coI, totalCancelled, true);

                Tuple<string, string> totalPending = ir.getTotalOrderByType(DateTime.Now, "pending");
                display(poT, poP, poI, totalPending);

                loadChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private LineSeries getLine(double[] t, string title, Brush b, Geometry c = null)
        {
            var series = new LineSeries();
            series.Title = title;
            series.Values = new ChartValues<double>(t);
            series.PointGeometry = c ?? DefaultGeometries.Circle;
            series.PointGeometrySize = 8;
            series.Fill = Brushes.Transparent;
            series.Stroke = b;
            series.StrokeThickness = 3;
            series.LabelPoint = point => point.Y.ToString("N0");
            return series;
        }

        private void loadChart()
        {
            try
            {
                chart.Series.Clear();
                chart.AxisX.Clear();
                chart.AxisY.Clear();


                var dailyOrders = ir.getOrderTotal();

                if (dailyOrders == null || !dailyOrders.Any())
                    return;

                var date = dailyOrders.Select(x => x.Item1.ToString("MMM dd")).ToArray();
                var totalOrders = dailyOrders.Select(x => (double)x.Item2).ToArray();
                var paidOrders = dailyOrders.Select(x => (double)x.Item3).ToArray();
                var cancelledOrder = dailyOrders.Select(x => (double)x.Item4).ToArray();

                var dailySeries = getLine(totalOrders, "Total Orders", Brushes.DodgerBlue);
                var paidSeries = getLine(paidOrders, "Paid Orders", Brushes.Gold);
                var cancelledSeries = getLine(cancelledOrder, "Cancelled Orders", Brushes.Yellow);

                chart.Series.Add(dailySeries);
                chart.Series.Add(paidSeries);
                chart.Series.Add(cancelledSeries);

                chart.Series = new SeriesCollection
                {
                    getLine(totalOrders, "Total Orders", Brushes.DodgerBlue,DefaultGeometries.Circle),
                    getLine(paidOrders, "Paid Orders", Brushes.Gold, DefaultGeometries.Diamond),
                    getLine(cancelledOrder, "Cancelled Orders", Brushes.Yellow, DefaultGeometries.Square)
                };

                chart.AxisX.Add(new Axis
                {
                    Title = "Date",
                    Labels = date,
                    Separator = new Separator
                    {
                        Step = 1,
                        IsEnabled = false
                    },
                    LabelsRotation = 45
                });

                var maxValue = Math.Ceiling(Math.Max(totalOrders.Max(), Math.Max(paidOrders.Max(), cancelledOrder.Max())) / 10) * 10;
                chart.AxisY.Add(new Axis
                {
                    Title = "Number of Orders",
                    LabelFormatter = value => value == maxValue ? value.ToString("N0") : "",
                    MinValue = 0,
                    MaxValue = maxValue,
                });

                chart.LegendLocation = LegendLocation.Top;
                chart.DataTooltip = new DefaultTooltip();
                chart.Hoverable = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void displayPercentage(Label total, Label percentLabel, Guna2PictureBox picture, Tuple<string, string> trans)
        {
            if (trans == null) return;
            total.Text = trans.Item1;

            if (!double.TryParse(trans.Item2, out double percent))
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
                return;
            }

            if (percent > 0)
            {
                percentLabel.Text = $"+{percent:N0}%";
                percentLabel.ForeColor = Color.Green;
                picture.Image = Properties.Resources.increase;
            }
            else if (percent < 0)
            {
                percentLabel.Text = $"{percent:N0}%";
                percentLabel.ForeColor = Color.Red;
                picture.Image = Properties.Resources.decrease;
            }
            else
            {
                percentLabel.Text = "0%";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
            }
        }
        private void display(Label total, Label percentLabel, Guna2PictureBox picture, Tuple<string, string> trans, bool reverse = false)
        {
            if (trans == null) return;
            total.Text = trans.Item1;

            if (!double.TryParse(trans.Item2, out double percent))
            {
                percentLabel.Text = "0";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
                return;
            }

            if (percent > 0)
            {
                percentLabel.Text = $"+{percent:N0}";
                percentLabel.ForeColor = reverse ? Color.Red : Color.Green;
                picture.Image = reverse ? Properties.Resources.decrease : Properties.Resources.increase;
            }
            else if (percent < 0)
            {
                percentLabel.Text = $"{percent:N0}";
                percentLabel.ForeColor = reverse ? Color.Red : Color.Green;
                picture.Image = reverse ? Properties.Resources.decrease : Properties.Resources.increase;
            }
            else
            {
                percentLabel.Text = "0";
                percentLabel.ForeColor = Color.Gray;
                picture.Image = Properties.Resources.even;
            }
        }

        private void date_ValueChanged(object sender, EventArgs e)
        {
            fetchData();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            refresh?.Stop();
            refresh?.Dispose();
            base.OnFormClosing(e);
        }

        private void refresh_Tick(object sender, EventArgs e)
        {
            fetchData();
        }
    }
}