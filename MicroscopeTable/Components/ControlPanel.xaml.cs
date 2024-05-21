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

        public void UpdateCenterPosition(double x, double y)
        {
            CenterPositionTextBlock.Text = $"X: {x:F2}, Y: {y:F2}";
        }
    }
}
