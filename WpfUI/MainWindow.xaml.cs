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

using Sudoku.UILogic;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUIProvider
    {
        private MainWindowLogic logic;
        public MainWindow()
        {
            logic = new MainWindowLogic(this);
            DataContext = logic;

            InitializeComponent();

            Loaded += (s, e) => logic.OnLoaded();
            board.MouseDown += OnMouseDown;

            logic.BoardLogic.PuzzleSolved += OnPuzzleSolved;

        }

        private void OnPuzzleSolved(object sender, EventArgs e)
        {
            Dispatcher.InvokeAsync(() => MessageBox.Show("Congratulations, game won!"));
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var point = Mouse.GetPosition(board);

            uint row = 0;
            uint col = 0;
            double accumulatedHeight = 0.0;
            double accumulatedWidth = 0.0;

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

            bool showpicker = false;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                showpicker = logic.BoardLogic.OnMouseLeft(row, col);
                e.Handled = true;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                showpicker = logic.BoardLogic.OnMouseRight(row, col);
                e.Handled = true;
            }

            if (showpicker)
            {
                // Get menu
                ContextMenu contextMenu = FindResource("numberPickerContextMenu") as ContextMenu;

                // Show the context menu
                if (contextMenu != null)
                {
                    //ContextMenu.PlacementTarget = canvas;
                    contextMenu.DataContext = logic.BoardLogic;
                    contextMenu.IsOpen = true;
                }
            }
        }


        private void ClickNumber(object sender, RoutedEventArgs e)
        {
            if (e.Source is Button button && button.Content is string content)
            {
                uint number = uint.Parse(content);
                ClickNumber(number);
            }
        }

        private void ClickClear(object sender, RoutedEventArgs e)
        {
            ClickNumber(0);
        }

        private void ClickNumber(uint number)
        {
            ContextMenu contextMenu = FindResource("numberPickerContextMenu") as ContextMenu;
            contextMenu.IsOpen = logic.BoardLogic.OnSetNumber(number);
        }

        public object GetBrush(EColors color)
        {
            switch(color)
            {
                case EColors.NormalBackground: return Brushes.Transparent;
                case EColors.GivenBackground: return Brushes.LightGray;
                case EColors.SelectedBackground: return Brushes.LightBlue;
                case EColors.NormalForeground: return Brushes.Black;
                case EColors.ErrorForeground: return Brushes.Red;
            }

            return Brushes.Turquoise;
        }
    }
}
