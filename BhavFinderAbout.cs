using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BhavFinder
{
    partial class BhavFinderAbout : Form
    {
        public BhavFinderAbout()
        {
            InitializeComponent();
        }

        private void OnAboutOkClicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
