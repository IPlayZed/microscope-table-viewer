using MicroscopeTable.Resources;
using MicroscopeTableLib.Utilities;
using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    
    public partial class ControlPanel : UserControl
    {
        private readonly static string Precision = "F3";
        public ControlPanel()
        {
            InitializeComponent();

            InitializeAxisControls();
        }

        private void InitializeAxisControls()
        {
            XAxisControl.axis = AxisControl.Axis.X;
            YAxisControl.axis = AxisControl.Axis.Y;
            ZAxisControl.axis = AxisControl.Axis.Z;
        }

        internal void UpdateCurrentStepsInAxisControls()
        {
            XAxisControl.UpdateCurrentSteps();
            YAxisControl.UpdateCurrentSteps();
            ZAxisControl.UpdateCurrentSteps();
        }
        internal void UpdateCenterPosition(Position newPosition)
        {
            CenterPositionTextBlock.Text = string.Format(
                    MicroscopeTable.Resources.ControlPanel.CenterPositionTextBlockText,
                    newPosition.X.ToString(Precision),
                    newPosition.Y.ToString(Precision),
                    newPosition.Z.ToString(Precision)
                );
        }

        private void OnSimulationSpeedClick(object sender, RoutedEventArgs e)
        {
            try
            {
                double newSpeed = Convert.ToDouble(SimulationSpeed.Text);
                GetVisualizationPanel().simulationStepSpeed = newSpeed;
                MessageBox.Show(string.Format(MessageWindow.ControlPanelSpeedUpdated, newSpeed),
                    MessageWindow.InfoTitle,
                    MessageBoxButton.OK, MessageBoxImage.Information
                    );
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                MessageBox.Show(MessageWindow.ControlPanelInvalidInput,
                    MessageWindow.ErrorTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error
                    );
            }

        }

        // TODO: Implement starter table position.
        private void OnUpdateClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox
                .Show(MessageWindow.ControlPanelResetEnvironment,
                MessageWindow.WarningTitle,
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning) == MessageBoxResult.Cancel) return;

                StepperConfiguration newStepperConfig = new(
                CreateGearConfigForAxis(XAxisControl),
                CreateGearConfigForAxis(YAxisControl),
                CreateGearConfigForAxis(ZAxisControl)
                );

                TableConfiguration newTableConfiguration = new(newStepperConfig, new());

                GetVisualizationPanel().ResetEnvironment(newTableConfiguration);
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                MessageBox.Show(MessageWindow.ControlPanelInvalidInput,
                    MessageWindow.ErrorTitle,
                    MessageBoxButton.OK, MessageBoxImage.Error 
                    );
            }
        }

        private static GearConfiguration CreateGearConfigForAxis(AxisControl axisControl)
        {
            uint maxSteps = Convert.ToUInt32(axisControl.MaxStepsTextBox.Text);
            double diameter = Convert.ToDouble(axisControl.DiameterTextBox.Text);
            return new(maxSteps, diameter);
        }

        // Refactor this so it can be merged with the one is AxisControl.
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
