using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormsUI
{
    class FreeContextMenuStrip : ContextMenuStrip
    {
        private readonly Control content;

        public FreeContextMenuStrip(Control content)
        {
            this.content = content;
            content.Location = Point.Empty; // ZZZ???

            // Setup the host as hosting the content
            ToolStripControlHost host = new ToolStripControlHost(content);
            //host.AutoSize = false;
            host.Padding = new Padding(0, 0, 0, 0);
            host.Margin = new Padding(0, 0, 0, 0);
            host.Size = new Size(content.Width, content.Height);
            //host.Width = content.Width;
            //host.Height = content.Height;

            // Setup the context menu
            ShowImageMargin = false;
            ShowCheckMargin = false;
            Padding = new Padding(0, 0, 0, 0);
            Margin = new Padding(0, 0, 0, 0);
            BackColor = content.BackColor;
            //AutoSize = true;
            //Width = content.Width;
            //Height = content.Height;

            // Add the host to the context menu
            Items.Add(host);
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
