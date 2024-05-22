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

        private void OnIncrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear();
            HandleAnimation(axis);
        }

        private void OnDecrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear(stepChange: StepChange.Decrease);
            HandleAnimation(axis, -120);
        }

        private void UpdateStepperMotor(TextBox stepTextBox, int stepChange)
        {
            if (int.TryParse(stepTextBox.Text, out int currentStep))
            {
                currentStep += stepChange;
                stepTextBox.Text = currentStep.ToString();
            }
            else
            {
                stepTextBox.Text = "0";
            }
        }

        internal enum Axis {X,Y,Z}

        private void HandleAnimation(Axis axis, int delta = 120)
        {
            switch (axis)
            {
                case Axis.X:
                case Axis.Y:
                    GetVisualizationPanel().UIHandleMovement(new());
                    break;
                case Axis.Z:
                    GetVisualizationPanel().UIHandleZoom(delta);
                    break;
                default:
                    break;
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
