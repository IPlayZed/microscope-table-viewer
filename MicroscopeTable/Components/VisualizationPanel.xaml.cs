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
        private Point center;

        private double zCoordinate;
        private double zoomFactor = 1.0;
        private const double zoomIncrement = 0.1;

        private readonly Table microscopeTable;

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
            var position = e.GetPosition(MainCanvas);
            UIUpdateCoordinateText(GetRelativePointFromCenter(position.X, position.Y));
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var rawPosition = Mouse.GetPosition(MainCanvas);

            // Do not update the actual UI coordinate, if it is not possible to move the table there.
            double _zCoordinate = zCoordinate;
            _zCoordinate += e.Delta > 0 ? 1 : -1;

            // If table can't move there, do not update the UI.
            if (UpdateMicroscopeTable(new(microscopeTable.TablePosition.X, microscopeTable.TablePosition.Y, _zCoordinate)) == null) return;

            UIHandleZoom(e.Delta);

            //UIPositionMicroscopeTable();

            var relativePosition = new Point(center.X, center.Y);
            UIUpdateCoordinateText(relativePosition);
            UIUpdateTablePositionInControlPanel(microscopeTable.TablePosition);
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get position raw coordinates on canvas.
            var rawPosition = e.GetPosition(MainCanvas);

            var relativePosition = new Point(rawPosition.X - center.X, center.Y - rawPosition.Y);

            // Get new positions requsted by the user.
            //double newPosX = rawPosition.X - MicroscopeTableRect.Width / 2;
            //double newPosY = rawPosition.Y - MicroscopeTableRect.Height / 2;
            double newPosX = relativePosition.X;
            double newPosY = relativePosition.Y;

            // If table can't move there, do not update the UI.
            if (UpdateMicroscopeTable(new(newPosX, newPosY, microscopeTable.TablePosition.Y)) == null) return;

            UIAnimateMicroscopeTable(rawPosition);            

            UIUpdateTablePositionInControlPanel(microscopeTable.TablePosition);
        }
        private void UpdateClip()
        {
            RectangleGeometry clipGeometry = new(new Rect(0, 0, MainCanvas.ActualWidth, MainCanvas.ActualHeight));
            MainCanvas.Clip = clipGeometry;
        }

        #endregion

        private Point GetRelativePointFromCenter(double X, double Y)
        {
            return new(X - center.X, center.Y - Y);
        }

        internal Position? UpdateMicroscopeTable(Position position)
        {
            try
            {
                // FIXME: position seems to be always out of range here.
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

        private void UIHandleZoom(int delta)
        {
            zCoordinate += delta > 0 ? 1 : -1;

            zoomFactor += delta > 0 ? zoomIncrement : -zoomIncrement;
            zoomFactor = Math.Max(0.1, zoomFactor);

            MicroscopeTableRect.Width = 200 * zoomFactor;
            MicroscopeTableRect.Height = 150 * zoomFactor;
        }
        private void UIUpdateCenter()
        {
            center = new Point(MainCanvas.ActualWidth / 2, MainCanvas.ActualHeight / 2);
            UIUpdateCenterLines();
        }

        private void UIUpdateCenterLines()
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

        private void UIUpdateCoordinateText(Point relativePosition)
        {
            CoordinateTextBlock.Text = $"X: {relativePosition.X:F2}, Y: {relativePosition.Y:F2}, Z: {zCoordinate:F2}";
        }

        private void UIPositionMicroscopeTable()
        {
            // Get new positions.
            double newPosX = center.X - MicroscopeTableRect.Width / 2;
            double newPosY = center.Y - MicroscopeTableRect.Height / 2;

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
                parentWindow.controlpanel.UpdateCenterPosition(position.X, position.Y, position.Z);
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
