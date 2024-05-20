using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MicroscopeTable.Components
{
    public partial class VisualizationPanel : UserControl
    {
        private Point center;
        private double zCoordinate;
        private double zoomFactor = 1.0;
        private const double zoomIncrement = 0.1;

        public VisualizationPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MainCanvas.MouseMove += OnMouseMove;
            MainCanvas.MouseWheel += OnMouseWheel;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateCenter();
            PositionMicroscopeTable();
            UpdateClip();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCenter();
            PositionMicroscopeTable();
            UpdateClip();
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
            UpdateCoordinateText(relativePosition);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            zCoordinate += e.Delta > 0 ? 1 : -1; // Update Z coordinate on scroll

            zoomFactor += e.Delta > 0 ? zoomIncrement : -zoomIncrement;
            zoomFactor = Math.Max(0.1, zoomFactor); // Prevent zooming out too much

            MicroscopeTableRect.Width = 200 * zoomFactor;
            MicroscopeTableRect.Height = 150 * zoomFactor;

            PositionMicroscopeTable();

            var position = Mouse.GetPosition(MainCanvas);
            var relativePosition = new Point(position.X - center.X, center.Y - position.Y);
            UpdateCoordinateText(relativePosition);
        }

        private void UpdateCoordinateText(Point relativePosition)
        {
            CoordinateTextBlock.Text = $"X: {relativePosition.X:F2}, Y: {relativePosition.Y:F2}, Z: {zCoordinate:F2}";
        }

        private void PositionMicroscopeTable()
        {
            Canvas.SetLeft(MicroscopeTableRect, center.X - MicroscopeTableRect.Width / 2);
            Canvas.SetTop(MicroscopeTableRect, center.Y - MicroscopeTableRect.Height / 2);
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var targetPosition = e.GetPosition(MainCanvas);
            AnimateMicroscopeTable(targetPosition);
        }

        private void AnimateMicroscopeTable(Point targetPosition)
        {
            double targetLeft = targetPosition.X - MicroscopeTableRect.Width / 2;
            double targetTop = targetPosition.Y - MicroscopeTableRect.Height / 2;

            DoubleAnimation animX = new DoubleAnimation(Canvas.GetLeft(MicroscopeTableRect), targetLeft, TimeSpan.FromSeconds(0.5));
            DoubleAnimation animY = new DoubleAnimation(Canvas.GetTop(MicroscopeTableRect), targetTop, TimeSpan.FromSeconds(0.5));

            MicroscopeTableRect.BeginAnimation(Canvas.LeftProperty, animX);
            MicroscopeTableRect.BeginAnimation(Canvas.TopProperty, animY);
        }

        private void UpdateClip()
        {
            RectangleGeometry clipGeometry = new RectangleGeometry(new Rect(0, 0, MainCanvas.ActualWidth, MainCanvas.ActualHeight));
            MainCanvas.Clip = clipGeometry;
        }
    }
}
