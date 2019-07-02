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
using Toolbox.UILogic.Dialogs;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IUIProvider
    {
        #region Private members

        private readonly MainWindowLogic logic;

        #endregion

        #region Constructor

        public MainWindow()
        {
            logic = new MainWindowLogic(this);
            DataContext = logic;

            InitializeComponent();

            Loaded += (s, e) => OnLoaded();
            Closing += (s, e) => OnClosing();
            board.MouseDown += OnMouseDown;
        }

        #endregion

        #region React to UI actions

        private void OnLoaded()
        {
            // Load settings
            var settings = logic.SettingsManager.Load();
            if (settings != null)
            {
                this.Left = settings.Left;
                this.Top = settings.Top;
            }

            // Show startup dialog
            Dispatcher.InvokeAsync(() => logic.OnLoaded());
        }

        private void OnClosing()
        {
            // Save settings
            var settings = new Settings();
            settings.Left = (int)Left;
            settings.Top = (int)Top;

            logic.SettingsManager.Save(settings);
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

        #endregion

        #region IUIProvider implementation

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

        public bool DisplayDialog(LogicDialogBase logic)
        {
            Window dialog = null;

            if (logic is NewGameDialogLogic newGameDialogLogic)
            {
                dialog = new NewGameDialog(newGameDialogLogic);
            }
            else if (logic is YesNoDialogLogic yesNoDialogLogic)
            {
                dialog = new YesNoDialog(yesNoDialogLogic);
            }
            else if (logic is StatsDialogLogic statsDialogLogic)
            {
                dialog = new StatsDialog(statsDialogLogic);
            }

            if (dialog == null)
            {
                throw new InvalidOperationException($"Unknown dialog {logic.GetType()}");
            }

            dialog.Owner = this;
            var result = dialog.ShowDialog();

            return result == true;
        }

        public void RunAsyncOnUIThread(Action action)
        {
            Dispatcher.InvokeAsync(action);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
