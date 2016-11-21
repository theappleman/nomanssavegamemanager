using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMSGM
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            lbVersion.Text = NMSGMSettings.GetDeploymentVersion() == null ? "not deployed" : NMSGMSettings.GetDeploymentVersion().ToString();

            Dictionary<string, string> Oss = new Dictionary<string, string>();

            Oss.Add("LiteDB", "https://github.com/mbdavid/LiteDB");
            Oss.Add("NewtonsoftJson", "http://www.newtonsoft.com/json");
            Oss.Add("ObjectListView", "http://objectlistview.sourceforge.net/");

            tlpOss.ColumnCount = 2;
            tlpOss.RowCount = Oss.Count;

            int pos = 0;

            foreach(var oss in Oss)
            {
                tlpOss.Controls.Add(new Label() { Text = oss.Key , Margin = new Padding(5), AutoSize = true}, 0, pos);
                tlpOss.Controls.Add(new LinkLabel() { Text = oss.Value, Margin = new Padding(5), AutoSize = true}, 1, pos);
                pos++;
            }

            foreach(var ll in tlpOss.Controls.OfType<LinkLabel>())
            {
                ll.Click += ((e, a) => { Process.Start(ll.Text); });
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("mailto:"+linkLabel1.Text);
            Process.Start(sInfo);
        }


    }
}
