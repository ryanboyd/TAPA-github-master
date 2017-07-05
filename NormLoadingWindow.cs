using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAPA
{
    public partial class NormLoadingWindow : Form
    {
        public NormLoadingWindow()
        {
            InitializeComponent();
        }

        public uint ProgressValueUpdate
        {
            set { LoadingProgressBar.Value = Convert.ToInt32(value); }
        }

        public int ProgressBarMaxSet
        {
            set { LoadingProgressBar.Maximum = value; }
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }

    }
}
