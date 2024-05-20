using System.Windows;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class GearControlPanel : UserControl
    {
        public GearControlPanel()
        {
            InitializeComponent();
        }

        // Event handler for button clicks
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Example action for button click
            MessageBox.Show("Button clicked in GearControlPanel!");
        }
    }
}
