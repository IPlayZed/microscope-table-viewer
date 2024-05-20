using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MicroscopeTable.Components
{
    public partial class VisualizationPanel : UserControl
    {
        private Point center;

        public VisualizationPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MainCanvas.MouseMove += OnMouseMove;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateCenter();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCenter();
        }

        private void UpdateCenter()
        {
            center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            UpdateCenterLines();
        }

        private void UpdateCenterLines()
        {
            HorizontalLine.X1 = 0;
            HorizontalLine.Y1 = center.Y;
            HorizontalLine.X2 = MainCanvas.ActualWidth;
            HorizontalLine.Y2 = center.Y;

            VerticalLine.X1 = center.X;
            VerticalLine.Y1 = 0;
            VerticalLine.X2 = center.X;
            VerticalLine.Y2 = MainCanvas.ActualHeight;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(MainCanvas);
            var relativePosition = new Point(position.X - center.X, center.Y - position.Y);
            CoordinateTextBlock.Text = $"X: {relativePosition.X:F2}, Y: {relativePosition.Y:F2}";
        }
    }
}
