using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

using MicroscopeTable.Components;

namespace MicroscopeTable.Components
{
    public partial class VisualizationPanel : UserControl
    {
        private Table table;
        public VisualizationPanel()
        {
            InitializeComponent();

            table = new Table();
        }
    }
}
