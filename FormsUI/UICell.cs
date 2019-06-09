using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Sudoku.UILogic;
using Sudoku.Game;

namespace FormsUI
{
    public partial class UICell : UserControl
    {
        private readonly UICellLogic logic;
        private readonly BoardLogic boardLogic;
        private readonly Position position;

        public UICell(UICellLogic logic, BoardLogic boardLogic, Position position)
        {
            this.logic = logic;
            this.boardLogic = boardLogic;
            this.position = position;

            InitializeComponent();

            // Initial state
            labelNumber.Dock = DockStyle.Fill;
            labelNumber.ForeColor = (Color)logic.Foreground;
            BackColor = (Color)logic.Background;
            labelNumber.Text = logic.Number;
            labelPossibles.ForeColor = (Color)logic.PossiblesForeground;
            labelPossibles.Text = logic.Possibles;

            // Draw borders
            tableLayoutPanelBorders.CellPaint += OnCellPaint;

            // Listen to logic changes
            logic.PropertyChanged += OnPropertyChanged;

            // Listen to mouse clicks
            panelCell.MouseDown += OnMouseDown;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            bool showContextMenu = false;

            if (e.Button == MouseButtons.Left)
            {
                showContextMenu = boardLogic.OnMouseLeft(position.Row, position.Column);
            }
            else if (e.Button == MouseButtons.Right)
            {
                showContextMenu = boardLogic.OnMouseRight(position.Row, position.Column);
            }

            if (showContextMenu)
            {
                var numberPicker = new NumberPicker(boardLogic);
                var contextMenuStrip = new FreeContextMenuStrip(numberPicker);
                numberPicker.ParentContextMenuStrip = contextMenuStrip;
                var point = Cursor.Position;
                point.X += 10;
                point.Y += 10;
                contextMenuStrip.Show(point);
            }
        }

        private void OnCellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            const float bigMargin = 3.0f;
            const float medMargin = 1.0f;
            const float smallMargin = 0.5f;

            // Leave the middle cell alone
            if (e.Row == 1 && e.Column == 1)
            {
                return;
            }

            // Grab info from the event
            var g = e.Graphics;
            float cLeft = e.CellBounds.Left;
            float cTop = e.CellBounds.Top;
            float cWidth = e.CellBounds.Width;
            float cHeight = e.CellBounds.Height;
            var brush = Brushes.Black;

            // Determine the border sizes
            float borderLeftWidth = position.Column == 0 ? bigMargin : (position.Column % Position.GRID_SIZE == 0 ? medMargin : smallMargin);
            float borderTopHeight = position.Row == 0 ? bigMargin : (position.Row % Position.GRID_SIZE == 0 ? medMargin : smallMargin);
            float borderRightWidth = position.Column == Position.ROW_COL_SEC_SIZE - 1 ? bigMargin : (position.Column % Position.GRID_SIZE == 2 ? medMargin : smallMargin);
            float borderBottomHeight = position.Row == Position.ROW_COL_SEC_SIZE - 1 ? bigMargin : (position.Row % Position.GRID_SIZE == 2 ? medMargin : smallMargin);

            if (e.Row == 0)
            {
                // Top border
                e.Graphics.FillRectangle(brush, cLeft, cTop, cWidth, borderTopHeight);
            }
            else if (e.Row == 2)
            {
                // Bottom border
                e.Graphics.FillRectangle(brush, cLeft, cTop + cHeight - borderBottomHeight, cWidth, borderBottomHeight);
            }

            if (e.Column == 0)
            {
                // Left border
                e.Graphics.FillRectangle(brush, cLeft, cTop, borderLeftWidth, cHeight);
            }
            else if (e.Column == 2)
            {
                // Top border
                e.Graphics.FillRectangle(brush, cLeft + cWidth - borderRightWidth, cTop, borderRightWidth, cHeight);
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case "Foreground":
                    labelNumber.ForeColor = (Color)logic.Foreground;
                    break;
                case "Background":
                    BackColor = (Color)logic.Background;
                    break;
                case "Number":
                    labelNumber.Text = logic.Number;
                    break;
                case "PossiblesForeground":
                    labelPossibles.ForeColor = (Color)logic.PossiblesForeground;
                    break;
                case "Possibles":
                    labelPossibles.Text = logic.Possibles;
                    break;
            }
        }
    }
}
