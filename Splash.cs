using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TAPA_Load
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }


        static public void ShowSplashScreen()
        {
            // Make sure it is only launched once.

            
        }

    }
}
