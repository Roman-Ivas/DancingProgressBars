using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DancingProgressBars
{
    public partial class MainWindow : Window
    {
        private List<ProgressBar> progressBars;
        private int numBars;

        public MainWindow()
        {
            InitializeComponent();
            progressBars = new List<ProgressBar>();
            numBars = 0;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            numBars++;
            ProgressBar bar = new ProgressBar();
            bar.Height = 30;
            bar.Margin = new Thickness(0, 10, 0, 0);
            progressBarsPanel.Children.Add(bar);
            progressBars.Add(bar);
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (numBars == 0)
            {
                MessageBox.Show("Please add at least one progress bar.");
                return;
            }
            for (int i = 0; i < numBars; i++)
            {
                ProgressBar bar = progressBars[i];
                bar.Value=0;
            }
                startButton.IsEnabled = false;
            await FillProgressBarsAsync();
            startButton.IsEnabled = true;
        }
        private async Task FillProgressBarsAsync()
        {
            Random rand = new Random();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < numBars; i++)
            {
                ProgressBar bar = progressBars[i];
                int value = (int)bar.Value;

                Task task = Task.Run(() =>
                {
                    do
                    {
                        value += rand.Next(5, 15);
                        if (value > 100) value = 100;

                        bar.Dispatcher.Invoke(() =>
                            {
                                bar.Value = value;
                                bar.Foreground = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
                            });
                        Thread.Sleep(500);
                    } while (value<100);
                });
                tasks.Add(task);
            }
            await Task.WhenAll(tasks);
        }
    }
}
