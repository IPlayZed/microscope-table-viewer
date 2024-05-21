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

#pragma warning disable CA1822 // Mark members as static
        // Reason: will work on this.
        private void UpdateStepperMotor(TextBox stepTextBox, int stepChange)
#pragma warning restore CA1822 // Mark members as static
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
