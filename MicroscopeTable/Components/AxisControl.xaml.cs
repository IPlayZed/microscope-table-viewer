using MicroscopeTable.Resources;
using MicroscopeTableLib.Components;
using MicroscopeTableLib.Exceptions;
using MicroscopeTableLib.Utilities;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class AxisControl : UserControl
    {
        // TODO: This is magic, so refactor called method to not require actual delta logic.
        private const int SCROLL_UP = 120;
        private const int SCROLL_DOWN = -SCROLL_UP;

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
            if (HandleStep() == null) return;
            HandleAnimation(axis);
            UpdateFields();
        }

        private void OnDecrementClick(object sender, RoutedEventArgs e)
        {
            if(HandleStep(stepChange: StepChange.Decrease) == null) return;
            HandleAnimation(axis, SCROLL_DOWN);
            UpdateFields();
        }

        internal enum Axis {X,Y,Z}

        private double? HandleStep(uint numberOfSteps = 1, StepChange stepChange = StepChange.Increase)
        {
            try
            {
                return GetMotorAxis().StepGear(numberOfSteps, stepChange);
            }
            catch(InvalidPositionException ex)
            {
                MessageBox.Show(ex.Message, MessageWindow.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        internal void UpdateCurrentSteps()
        {
            CurrentStepsTextBox.Text = GetMotorAxis().MotorGear.CurrentStep.ToString();
        }

        // TODO: Moving on the X/Y would require geometry info here from the view panel, which is not structured for it.
        private void HandleAnimation(Axis axis, int delta = SCROLL_UP)
        {
            switch (axis)
            {
                case Axis.X:
                case Axis.Y:
                    MessageBox.Show(
                        MessageWindow.ControlPanelNotImplemented 
                        + " Modify X/Y coordinates by clicking on the visualization panel!",
                        MessageWindow.InfoTitle,
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
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
