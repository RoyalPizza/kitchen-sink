using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<IrisParquet> _irisParquetCollection = new ObservableCollection<IrisParquet>();
        private CancellationTokenSource? _controlsCancellationTokenSource;
        private CancellationTokenSource _canvasCancellationTokenSource = new CancellationTokenSource();

        private int _exampleButtonCounter = 0;
        private int _asyncProgressBarCounter = 0;
        private bool _asyncWorkerRunning = false;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var text = File.ReadAllText("iris-parquet.json"); // oh yeah
            var collection = JsonSerializer.Deserialize<IrisParquetCollection>(text) ?? new IrisParquetCollection();
            foreach (var item in collection.Data)
                _irisParquetCollection.Add(item);

            ExampleListView.ItemsSource = _irisParquetCollection; // could bind instead

            SetupCanvas();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void ExampleButton_Click(object sender, RoutedEventArgs e)
        {
            _exampleButtonCounter++;
            ExampleButton_Label.Content = $"Clicked {_exampleButtonCounter}";
        }

        private void AsyncControlButton_Click(object sender, RoutedEventArgs e)
        {
            if (_asyncWorkerRunning)
            {
                _controlsCancellationTokenSource?.Cancel();
                _controlsCancellationTokenSource?.Dispose();
                _asyncWorkerRunning = false;
            }
            else
            {
                _controlsCancellationTokenSource = new CancellationTokenSource();
                _ = AsyncControlsTask(_controlsCancellationTokenSource.Token);
                _asyncWorkerRunning = true;
            }

            AsyncControlButton.Content = _asyncWorkerRunning ? "Stop" : "Start";
        }

        private async Task AsyncControlsTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _asyncProgressBarCounter += 1;
                if (_asyncProgressBarCounter >= 100)
                    _asyncProgressBarCounter = 0;
                AsyncProgressBar.Value = _asyncProgressBarCounter;

                if (_asyncProgressBarCounter > 50)
                {
                    AsyncRectagle.Fill = new SolidColorBrush(Colors.DarkOrange);
                    AsyncEllipse.Fill = new SolidColorBrush(Colors.DarkBlue);
                }
                else
                {
                    AsyncRectagle.Fill = new SolidColorBrush(Colors.DarkBlue);
                    AsyncEllipse.Fill = new SolidColorBrush(Colors.DarkOrange);
                }

                    await Task.Delay(20, cancellationToken);
            }
        }

        private void SetupCanvas()
        {
            ExampleCanvas.Children.Clear();

            for (int i = 0; i < 50; i++)
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Fill = (i % 2 == 0) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Blue);
                rectangle.Width = 50;
                rectangle.Height = 50;

                rectangle.RenderTransform = new RotateTransform(0);

                ExampleCanvas.Children.Add(rectangle);
                Canvas.SetTop(rectangle, i * 5);
                Canvas.SetLeft(rectangle, i * 5);
            }

            _ = AsyncCanvasTask(_canvasCancellationTokenSource.Token);
        }

        private async Task AsyncCanvasTask(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (UIElement item in ExampleCanvas.Children)
                {
                    var left = Canvas.GetLeft(item);
                    left = (left >= 400) ? 0 : left + 5;
                    Canvas.SetLeft(item, left);

                    RotateTransform rotateTransform = (RotateTransform)item.RenderTransform;
                    rotateTransform.CenterY = 0;
                    rotateTransform.CenterX = 0;
                    rotateTransform.Angle += 1;
                    
                }
                await Task.Delay(20);
            }
        }

        private void ExampleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            PasswordLabel.FontSize = e.NewValue;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = ((PasswordBox)sender).Password;
            PasswordLabel.Content = $"Secret: {password}";
        }
    }
}