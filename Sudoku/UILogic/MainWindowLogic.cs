using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sudoku.Game;
using Toolbox;

namespace Sudoku.UILogic
{
    /// <summary>
    /// Model for the main window
    /// </summary>
    public class MainWindowLogic : LogicBase
    {
        #region Construction

        // Class that knows how to do UI stuff, provided by the UI layer 
        private readonly IUIProvider uiProvider;

        // Constructor
        public MainWindowLogic(IUIProvider uiProvider)
        {
            this.uiProvider = uiProvider;

            // Create the board
            BoardLogic = new BoardLogic(uiProvider);

            // Create the settings manager
            SettingsManager = new SettingsManager<Settings>("Sarabande, Inc.", "Sudoku");

            // Get notified when a puzzle is solved
            BoardLogic.PuzzleSolved += OnPuzzleSolved;

            // Create the commands
            NewGame = new CommandBase(OnNewGame);
            Help = new CommandBase(OnHelp);
        }

        #endregion

        #region UI properties

        // Command for the "New game" button
        public CommandBase NewGame { get; }

        // Command for the "help" button
        public CommandBase Help { get; }

        // Model for the board
        public BoardLogic BoardLogic { get; }

        // Settings manager
        public SettingsManager<Settings> SettingsManager { get; }

        #endregion

        #region Actions

        // Generate new puzzle on load
        public void OnLoaded()
        {
            GenerateNewPuzzle(true);
        }

        // Generate new puzzle when "new game" button is pressed
        private void OnNewGame()
        {
            GenerateNewPuzzle(false);
        }

        private void GenerateNewPuzzle(bool exitOnCancel)
        {
            // Get difficulty
            var newGameDialogLogic = new NewGameDialogLogic();
            bool result = uiProvider.DisplayDialog(newGameDialogLogic);
            if (result)
            {
                // Show new puzzle
                BoardLogic.OnGeneratePuzzle(newGameDialogLogic.Difficulty);
            }
            else if (exitOnCancel)
            {
                uiProvider.Exit();
            }
        }

        // Ask the player if play again om resolution
        private void OnPuzzleSolved(object sender, EventArgs e)
        {
            uiProvider.RunAsyncOnUIThread(OnPuzzleSolvedAsync);
        }

        private void OnPuzzleSolvedAsync()
        {
            var logic = new YesNoDialogLogic("Solved", "Congratulations, you solved the puzzle!\n\nPlay again?", true);
            bool result = uiProvider.DisplayDialog(logic);
            if (result)
            {
                GenerateNewPuzzle(false);
            }
            else
            {
                uiProvider.Exit();
            }
        }

        // User wants help
        private void OnHelp()
        {
            var logic = new InfoDialogLogic("Sudoku help", HELP);
            uiProvider.DisplayDialog(logic);
        }

        #endregion

        #region Help string

        private const string HELP =
            "Welcome to Sudoku.\n\n" +
            "To fill in the cells, left click and select the desired number, or use the arrow keys to navigate to the\n" +
            "desired cell and use the numerical keys 1 through 9.\n\n" +
            "To erase a cell, left click and select 'CLEAR', or use the arrow keys to navigate to the desired cell and use\n" +
            "either the zero numerical key or the delete key.\n\n" +
            "To mark or unmark a possible value for a cell, right click and select the desired number, or use the arrow\n" + 
            "keys to navigate to the desired cell and use the functions key F1 to F9.\n\n" +
            "To undo a move use ^Z. To redo a move, use ^Y.\n" +
            "To check your deductions, use ^C; Incorrect deductions will become red.\n" +
            "To get a hint of which cell can be deduced next, use ^H\n" +
            "To get a detailed explanation of which cell can be deduced next, use ^Q\n";

        #endregion
    }
}
