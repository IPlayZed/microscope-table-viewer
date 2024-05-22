using MicroscopeTableLib.Utilities;
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

        public void UpdateCenterPosition(Position newPosition)
        {
            CenterPositionTextBlock.Text = string.Format(
                    MicroscopeTable.Resources.ControlPanel.CenterPositionTextBlockText,
                    newPosition.X.ToString(Precision),
                    newPosition.Y.ToString(Precision),
                    newPosition.Z.ToString(Precision)
                );
        }


    }
}
