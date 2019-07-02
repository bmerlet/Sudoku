using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Sudoku.UILogic;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for StatsDialog.xaml
    /// </summary>
    public partial class StatsDialog : Window
    {
        public StatsDialog(StatsDialogLogic logic)
        {
            this.DataContext = logic;

            // Tell the logic how to close this dialog
            logic.CloseView = result => DialogResult = result;

            InitializeComponent();
        }
    }
}
