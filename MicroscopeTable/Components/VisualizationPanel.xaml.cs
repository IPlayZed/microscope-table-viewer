using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;
using MicroscopeTableLib.Components;
using MicroscopeTableLib.Utilities;
using MicroscopeTable.Resources;
using MicroscopeTableLib.Exceptions;

namespace MicroscopeTable.Components
{
    public partial class VisualizationPanel : UserControl
    {
        private Point viewPortCenter;
        private double viewPortHeight;

        private double zoomFactor = 1.0;
        private const double zoomIncrement = 0.1;

        internal double simulationStepSpeed = 0.5;

        internal readonly Table microscopeTable;

        public VisualizationPanel()
        {
            InitializeComponent();

            RegisterCallbacks();

            microscopeTable = new Table();
        }

        #region MFVCallbacks

        private void RegisterCallbacks()
        {
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            MainCanvas.MouseMove += OnMouseMove;
            MainCanvas.MouseWheel += OnMouseWheel;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UIUpdateCenter();
            UIPositionMicroscopeTable();
            UpdateClip();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UIUpdateCenter();
            UIPositionMicroscopeTable();
            UpdateClip();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var rawPosition = e.GetPosition(MainCanvas);
            UIUpdateCoordinateText(GetRelativePointFromCenter(rawPosition.X, rawPosition.Y));
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Do not update the actual UI coordinate, if it is not possible to move the table there.
            double z = microscopeTable.TablePosition.Z;
            z += e.Delta > 0 ? 1 : -1;

            HandleZoom(z);

            UIHandleZoom(e.Delta);

            var rawPosition = e.GetPosition(MainCanvas);
            UIUpdateCoordinateText(GetRelativePointFromCenter(rawPosition.X, rawPosition.Y));
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get position raw coordinates on canvas.
            var rawPosition = e.GetPosition(MainCanvas);

            HandleMovement(rawPosition);
        }
        private void UpdateClip()
        {
            RectangleGeometry clipGeometry = new(new Rect(0, 0, MainCanvas.ActualWidth, MainCanvas.ActualHeight));
            MainCanvas.Clip = clipGeometry;
        }

        #endregion

        internal void HandleZoom(double zChange)
        {  
            // If table can't move there, do not update the UI.
            if (UpdateMicroscopeTable(new(microscopeTable.TablePosition.X, microscopeTable.TablePosition.Y, zChange)) == null) return;
        }

        internal void HandleMovement(Point rawPosition)
        {
            var relativePosition = new Point(rawPosition.X - viewPortCenter.X, viewPortCenter.Y - rawPosition.Y);

            // Get new positions requsted by the user.
            double newPosX = relativePosition.X;
            double newPosY = relativePosition.Y;

            // If table can't move there, do not update the UI.
            if (UpdateMicroscopeTable(new(newPosX, newPosY, microscopeTable.TablePosition.Y)) == null) return;

            UIHandleMovement(rawPosition);
        }

        private Point GetRelativePointFromCenter(double X, double Y)
        {
            return new(X - viewPortCenter.X, viewPortCenter.Y - Y);
        }

        internal Position? UpdateMicroscopeTable(Position position)
        {
            try
            {
                microscopeTable.MoveTableTo(position);
                return new(microscopeTable.TablePosition.X, microscopeTable.TablePosition.Y, microscopeTable.TablePosition.Z);
            }
            catch (InvalidPositionException ex)
            {
                MessageBox.Show(ex.Message, MessageWindow.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        #region UI
        internal void UIHandleMovement(Point rawPosition)
        {
            UIAnimateMicroscopeTable(rawPosition);

            UIUpdateTablePositionInControlPanel(microscopeTable.TablePosition);
        }
        internal void UIHandleZoom(int delta)
        {
            viewPortHeight += delta > 0 ? 1 : -1;

            zoomFactor += delta > 0 ? zoomIncrement : -zoomIncrement;
            zoomFactor = Math.Max(0.1, zoomFactor);

            MicroscopeTableRect.Width = 200 * zoomFactor;
            MicroscopeTableRect.Height = 150 * zoomFactor;

            UIPositionMicroscopeTable();

            UIUpdateTablePositionInControlPanel(microscopeTable.TablePosition);
        }
        private void UIUpdateCenter()
        {
            viewPortCenter = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            UIUpdateCenterLines();
        }

        private void UIUpdateCenterLines()
        {
            HorizontalLine.X1 = 0;
            HorizontalLine.Y1 = viewPortCenter.Y;
            HorizontalLine.X2 = MainCanvas.ActualWidth;
            HorizontalLine.Y2 = viewPortCenter.Y;

            VerticalLine.X1 = viewPortCenter.X;
            VerticalLine.Y1 = 0;
            VerticalLine.X2 = viewPortCenter.X;
            VerticalLine.Y2 = MainCanvas.ActualHeight;
        }

        private void UIUpdateCoordinateText(Point relativePosition)
        {
            CoordinateTextBlock.Text = $"X: {relativePosition.X:F2}, Y: {relativePosition.Y:F2}, Z: {viewPortHeight:F2}";
        }

        private void UIPositionMicroscopeTable()
        {
            // Get new positions.
            double newPosX = viewPortCenter.X - MicroscopeTableRect.Width / 2;
            double newPosY = viewPortCenter.Y - MicroscopeTableRect.Height / 2;

            // Update the graphics.
            Canvas.SetLeft(MicroscopeTableRect, newPosX);
            Canvas.SetTop(MicroscopeTableRect, newPosY);
        }

        private void UIAnimateMicroscopeTable(Point targetPosition, double animationStepSpeed = 0.5)
        {
            // Get new positions requsted by the user.
            double newPosX = targetPosition.X - MicroscopeTableRect.Width / 2;
            double newPosY = targetPosition.Y - MicroscopeTableRect.Height / 2;

            DoubleAnimation animX = new(Canvas.GetLeft(MicroscopeTableRect), newPosX, TimeSpan.FromSeconds(animationStepSpeed));
            DoubleAnimation animY = new(Canvas.GetTop(MicroscopeTableRect), newPosY, TimeSpan.FromSeconds(animationStepSpeed));

            MicroscopeTableRect.BeginAnimation(Canvas.LeftProperty, animX);
            MicroscopeTableRect.BeginAnimation(Canvas.TopProperty, animY);
        }

        private void UIUpdateTablePositionInControlPanel(Position position)
        {
            if (Window.GetWindow(this) is MainWindow parentWindow)
            {
                parentWindow.controlpanel.UpdateCenterPosition(position);
            }
            else
            {
                MessageBox.Show(MessageWindow.ControlPanelCommunicationError, MessageWindow.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                throw new InvalidOperationException(String.Format(Exceptions.ParentWindowNull,
                    " Unable to update table position on ControlPanel."));
            }
        }

        #endregion
    }
}
