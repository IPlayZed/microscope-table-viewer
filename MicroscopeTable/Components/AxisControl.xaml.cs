using MicroscopeTable.Resources;
using MicroscopeTableLib.Components;
using MicroscopeTableLib.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class AxisControl : UserControl
    {
        // TODO: Maybe refactor the called function, so no "magic number" is needed for zooming.
        private static readonly int SCROLL_UP = 120;
        private static readonly int SCROLL_DOWN = -120;

        internal Axis axis;
        public AxisControl()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateFields();
        }

        private void UpdateFields()
        {
            var motorAxis = GetMotorAxis();
            MaxStepsTextBox.Text = motorAxis.MotorGear.NumberOfSteps.ToString();
            DiameterTextBox.Text = motorAxis.MotorGear.Diameter.ToString();
            CurrentStepsTextBox.Text = motorAxis.MotorGear.CurrentStep.ToString();
        }

        private void OnIncrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear();
            HandleAnimation(axis);
            UpdateFields();
        }

        private void OnDecrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear(stepChange: StepChange.Decrease);
            HandleAnimation(axis, -120);
            UpdateFields();
        }

        private void UpdateStepperMotor(TextBox stepTextBox, int stepChange)
        {
            
        }

        internal enum Axis {X,Y,Z}

        private void HandleAnimation(Axis axis, int delta = 120, Point UIPoint = new())
        {
            switch (axis)
            {
                case Axis.X:
                case Axis.Y:
                    GetVisualizationPanel().UIHandleMovement(UIPoint);
                    break;
                case Axis.Z:
                    GetVisualizationPanel().UIHandleZoom(delta);
                    break;
                default:
                    throw HandleUnknownAxis(axis);
            }
        }

        private MotorAxis GetMotorAxis()
        {
            Table table = GetMicroscopeTableFromVisualizationPanel();
            return axis switch
            {
                Axis.X => table.MotorAxisX,
                Axis.Y => table.MotorAxisY,
                Axis.Z => table.MotorAxisZ,
                _ => throw HandleUnknownAxis(axis),
            };
        }

        private static InvalidOperationException HandleUnknownAxis(Axis axis)
        {
            MessageBox.Show(
                        MessageWindow.ControlPanelAxisNotSetCorrectly,
                        MessageWindow.ErrorTitle,
                        MessageBoxButton.OK, MessageBoxImage.Error
                        );
            throw new InvalidOperationException(
                String.Format(MessageWindow.ControlPanelAxisNotSetCorrectly, axis)
                );
        }

        private Table GetMicroscopeTableFromVisualizationPanel()
        {
            return GetVisualizationPanel().microscopeTable;
        }
        
        // FIXME: Why is this thrown at close? (Debugging doesn't seem to break here.)
        private VisualizationPanel GetVisualizationPanel()
        {
            if (Window.GetWindow(this) is MainWindow parentWindow)
            {
                return parentWindow.visualizationPanel;
            }
            else
            {
                MessageBox.Show(MessageWindow.VisualizationPanelCommunicationError, MessageWindow.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                throw new InvalidOperationException(String.Format(Exceptions.ParentWindowNull,
                    " Unable to get the microscope table object from visualization panel."));
            }
        }
    }
}
