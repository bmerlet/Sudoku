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
using System.Windows.Navigation;
using System.Windows.Shapes;

using Sudoku;
using WpfUI.Logic;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowLogic logic;
        public MainWindow()
        {
            logic = new MainWindowLogic();
            DataContext = logic;

            InitializeComponent();

            Loaded += (s, e) => logic.OnLoaded();
            board.MouseDown += OnMouseDown;

        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(board);

            uint row = 0;
            uint col = 0;
            double accumulatedHeight = board.Margin.Top;
            double accumulatedWidth = board.Margin.Left;

            // calc row mouse was over
            foreach (var rowDefinition in board.RowDefinitions)
            {
                accumulatedHeight += rowDefinition.ActualHeight;
                if (accumulatedHeight >= point.Y)
                    break;
                row++;
            }

            // calc col mouse was over
            foreach (var columnDefinition in board.ColumnDefinitions)
            {
                accumulatedWidth += columnDefinition.ActualWidth;
                if (accumulatedWidth >= point.X)
                    break;
                col++;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                logic.BoardLogic.OnMouseLeft(row, col);
                e.Handled = true;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                logic.BoardLogic.OnMouseRight(row, col);
                e.Handled = true;
            }
        }
    }
}
