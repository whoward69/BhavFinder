using BhavFinder.Utils.Persistence;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BhavFinder
{
    public partial class BhavFinderConfig : Form
    {
        public BhavFinderConfig()
        {
            InitializeComponent();

            selectPathDialog = new CommonOpenFileDialog();
        }

        private void OnConfigOkClicked(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            RegistryTools.SaveSetting(BhavFinderApp.RegistryKey, "Sims2Path", textSims2Path.Text);
            RegistryTools.SaveSetting(BhavFinderApp.RegistryKey, "SimPEPath", textSimPEPath.Text);

            this.Close();
        }

        private void OnSelectSim2PathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSims2Path.Text;

            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSims2Path.Text = selectPathDialog.FileName;
            }
        }

        private void OnSelectSimPEPathClicked(object sender, EventArgs e)
        {
            selectPathDialog.InitialDirectory = textSimPEPath.Text;
            
            if (selectPathDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textSimPEPath.Text = selectPathDialog.FileName;
            }
        }

        private void OnConfigLoad(object sender, EventArgs e)
        {
            textSims2Path.Text = (String) RegistryTools.GetSetting(BhavFinderApp.RegistryKey, "Sims2Path", "");
            textSimPEPath.Text = (String) RegistryTools.GetSetting(BhavFinderApp.RegistryKey, "SimPEPath", "");
        }
    }
}
