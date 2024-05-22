using MicroscopeTableLib.Utilities;
using System.Windows.Controls;

// TODO: Use fields defined in the XAML.
namespace MicroscopeTable.Components
{
    public partial class ControlPanel : UserControl
    {
        public ControlPanel()
        {
            InitializeComponent();
        }

        public void UpdateCenterPosition(Position newPosition)
        {
            CenterPositionTextBlock.Text = $"X: {newPosition.X:F2}, Y: {newPosition.Y:F2}, Z: {newPosition.Z:F2}";
        }
    }
}
