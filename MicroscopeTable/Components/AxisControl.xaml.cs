using MicroscopeTable.Resources;
using MicroscopeTableLib.Components;
using MicroscopeTableLib.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class AxisControl : UserControl
    {
        internal Axis axis;
        public AxisControl()
        {
            InitializeComponent();
        }

        private void OnIncrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear();
        }

        private void OnDecrementClick(object sender, RoutedEventArgs e)
        {
            GetMotorAxis().StepGear(stepChange: StepChange.Decrease);
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

        internal enum Axis
        {
            X,Y,Z
        }

        private MotorAxis GetMotorAxis()
        {
            Table table = GetMicroscopeTableFromVisualizationPanel();
            switch (axis)
            {
                case Axis.X: return table.MotorAxisX;
                case Axis.Y: return table.MotorAxisY;
                case Axis.Z: return table.MotorAxisZ;
                default:
                    MessageBox.Show(
                        MessageWindow.ControlPanelAxisNotSetCorrectly,
                        MessageWindow.ErrorTitle,
                        MessageBoxButton.OK, MessageBoxImage.Error 
                        );
                    throw new InvalidOperationException(
                        String.Format(MessageWindow.ControlPanelAxisNotSetCorrectly, axis)
                        );
            }
        }

        private Table GetMicroscopeTableFromVisualizationPanel()
        {
            if (Window.GetWindow(this) is MainWindow parentWindow)
            {
                return parentWindow.visualizationPanel.microscopeTable;
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
