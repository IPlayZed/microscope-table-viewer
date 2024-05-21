using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class AxisControl : UserControl
    {
        public AxisControl()
        {
            InitializeComponent();
        }

        private void OnIncrementClick(object sender, RoutedEventArgs e)
        {
            UpdateStepperMotor(CurrentStepTextBox, 1);
        }

        private void OnDecrementClick(object sender, RoutedEventArgs e)
        {
            UpdateStepperMotor(CurrentStepTextBox, -1);
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
    }
}
