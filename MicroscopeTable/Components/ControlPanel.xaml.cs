using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();
        }

        public void UpdateCenterPosition(double x, double y, double z)
        {
            CenterPositionTextBlock.Text = $"X: {x:F2}, Y: {y:F2}, Z: {z:F2}";
        }
    }
}
