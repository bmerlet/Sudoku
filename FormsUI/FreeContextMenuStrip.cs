using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsUI
{
    /// <summary>
    /// Trick the context menu strip into displaying something that is not a menu.
    /// </summary>
    public class FreeContextMenuStrip : ContextMenuStrip
    {
        private readonly Control content;
        private readonly Padding NullPadding = new Padding(0, 0, 0, 0);

        public FreeContextMenuStrip(Control content)
        {
            this.content = content;
            content.Location = Point.Empty;

            // Setup the host as hosting the content
            ToolStripControlHost host = new ToolStripControlHost(content);
            host.Padding = NullPadding;
            host.Margin = NullPadding;
            host.Size = new Size(content.Width, content.Height);

            // Setup the context menu
            ShowImageMargin = false;
            ShowCheckMargin = false;
            Margin = GripMargin = Padding = NullPadding;
            BackColor = content.BackColor;
            MaximumSize = new Size(content.Width, content.Height + 2); // + 2 to allow for padding below

            // Add the host to the context menu
            Items.Add(host);
        }

        // Avoid menu-specific indentations
        protected override Padding DefaultPadding => new Padding(0, 1, 0, 1); // Necessary otherwise control does not fit
        protected override Padding DefaultMargin => NullPadding;
        protected override Padding DefaultGripMargin => NullPadding;

        protected override void OnLayout(LayoutEventArgs e)
        {
            content.SetBounds(0, 0, content.Width, content.Height);
            this.Size = new Size(content.Width, content.Height + 2);
            this.SetDisplayedItems();
            this.OnLayoutCompleted(EventArgs.Empty);
            this.Invalidate();
            //Padding = NullPadding;
            //base.OnLayout(e);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            // Make the tab key advance focus, and alt-tab rewind it
            if (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Alt))
            {
                Control lastControl = null;
                foreach (Control ctrl in content.Controls)
                {
                    if (ctrl.Focused)
                    {
                        lastControl = ctrl;
                        break;
                    }
                }

                content.SelectNextControl(lastControl, keyData == Keys.Tab, true, true, true);

                return true;
            }

            // prevent alt from closing the context menu, therefore allowing mnemonics to work
            if (keyData.HasFlag(Keys.Alt))
            {
                return false;
            }

            return base.ProcessDialogKey(keyData);
        }

        // Grab focus when opening
        protected override void OnOpened(EventArgs e)
        {
            content.Focus();
            base.OnOpened(e);
        }
    }
}
