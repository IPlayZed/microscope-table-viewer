using MicroscopeTableLib.Utilities;
using System.Windows.Controls;

namespace MicroscopeTable.Components
{
    public partial class ControlPanel : UserControl
    {
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

        public void UpdateCenterPosition(Position newPosition)
        {
            CenterPositionTextBlock.Text = String.Format(
                    MicroscopeTable.Resources.ControlPanel.CenterPositionTextBlockText,
                    newPosition.X,
                    newPosition.Y,
                    newPosition.Z
                );
        }


    }
}
