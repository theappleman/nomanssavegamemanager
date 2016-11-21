using NMSGM.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMSGM
{
    public partial class ChangelogForm : Form
    {
        private bool _hasAccepted = false;

        public ChangelogForm()
        {
            InitializeComponent();
            rtbChangelog.Text = Resources.ChangelogInformation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _hasAccepted = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            rtbChangelog.SelectionLength = 0;
        }

        internal bool HasAccepted()
        {
            return _hasAccepted;
        }

        
    }
}
