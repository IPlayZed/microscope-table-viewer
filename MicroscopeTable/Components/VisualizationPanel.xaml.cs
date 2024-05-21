using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace MicroscopeTable.Components
{
    public partial class VisualizationPanel : UserControl
    {
        private Point center;
        private double zCoordinate;
        private double zoomFactor = 1.0;
        private const double zoomIncrement = 0.1;

        /// <summary>
        /// This user control is responsible for visualizing the microscope table.
        /// By clicking in the user control the table can be moved via an animation.
        /// The mouse position is shown in the user control.
        /// By scrolling up via the middle mouse button, the view area is zoomed into, by scrolling down, the view area is zoomed out of.
        /// </summary>
        public VisualizationPanel()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MainCanvas.MouseMove += OnMouseMove;
            MainCanvas.MouseWheel += OnMouseWheel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateCenter();
            PositionMicroscopeTable();
            UpdateClip();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCenter();
            PositionMicroscopeTable();
            UpdateClip();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCenter()
        {
            center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            UpdateCenterLines();
            UpdateControlPanelPosition(center.X, center.Y, zCoordinate);
        }

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(MainCanvas);
            var relativePosition = new Point(position.X - center.X, center.Y - position.Y);
            UpdateCoordinateText(relativePosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePosition"></param>
        private void UpdateCoordinateText(Point relativePosition)
        {
            CoordinateTextBlock.Text = $"X: {relativePosition.X:F2}, Y: {relativePosition.Y:F2}, Z: {zCoordinate:F2}";
        }

        /// <summary>
        /// 
        /// </summary>
        private void PositionMicroscopeTable()
        {
            Canvas.SetLeft(MicroscopeTableRect, center.X - MicroscopeTableRect.Width / 2);
            Canvas.SetTop(MicroscopeTableRect, center.Y - MicroscopeTableRect.Height / 2);
            UpdateControlPanelPosition(center.X, center.Y, zCoordinate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var targetPosition = e.GetPosition(MainCanvas);
            AnimateMicroscopeTable(targetPosition);
            UpdateControlPanelPosition(center.X, center.Y, zCoordinate);
        }

        /// <summary>
        /// Animate the movement of the rectangle representing the microscope table.
        /// </summary>
        /// <param name="targetPosition"> The new center position to move the microscope table to. </param>
        private void AnimateMicroscopeTable(Point targetPosition, double animationStepSpeed = 0.5)
        {
            double targetLeft = targetPosition.X - MicroscopeTableRect.Width / 2;
            double targetTop = targetPosition.Y - MicroscopeTableRect.Height / 2;

            DoubleAnimation animX = new(Canvas.GetLeft(MicroscopeTableRect), targetLeft, TimeSpan.FromSeconds(animationStepSpeed));
            DoubleAnimation animY = new(Canvas.GetTop(MicroscopeTableRect), targetTop, TimeSpan.FromSeconds(animationStepSpeed));

            animX.Completed += (s, e) => UpdateCenterAfterAnimation(targetPosition);
            animY.Completed += (s, e) => UpdateCenterAfterAnimation(targetPosition);

            MicroscopeTableRect.BeginAnimation(Canvas.LeftProperty, animX);
            MicroscopeTableRect.BeginAnimation(Canvas.TopProperty, animY);
        }

        /// <summary>
        /// Make sure that the rectangle representing the microscope table is not shown outside the viewport.
        /// </summary>
        private void UpdateClip()
        {
            RectangleGeometry clipGeometry = new(new Rect(0, 0, MainCanvas.ActualWidth, MainCanvas.ActualHeight));
            MainCanvas.Clip = clipGeometry;
        }

        private void UpdateControlPanelPosition(double x, double y, double z)
        {
            if (Window.GetWindow(this) is MainWindow parentWindow)
            {
                parentWindow.controlpanel.UpdateCenterPosition(x, y, z);
            }
            else
            {
                // Show error message to the user
                MessageBox.Show("Could not communicate with control panel.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Throw an exception
                throw new InvalidOperationException("Parent window is null. Unable to update ControlPanel position.");
            }
        }

        private void UpdateCenterAfterAnimation(Point newCenter)
        {
            center = newCenter;
            UpdateControlPanelPosition(center.X, center.Y, zCoordinate);
        }
    }
}
