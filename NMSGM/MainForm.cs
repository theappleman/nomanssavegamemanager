using NMSGM.Classes;
using NMSGM.Entities;
using NMSGM.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static NMSGM.Classes.SavegameLocationManager;

namespace NMSGM
{
    public partial class MainForm : Form
    {
        SavegameLocationManager sgLoc;
        public MainForm()
        {
            InitializeComponent();

            if(FindSavegameFolder() && Settings.Default.AutostartWatcherOnLaunch)
            {
                StartWatcher();
            }


            UpdateDbSize();
            UpdateLastProtected();
        }



        public bool FindSavegameFolder()
        {
            try
            {
                sgLoc = new SavegameLocationManager(this);
                var fld = sgLoc.savegameProfile;

                lbPath.Text = sgLoc.savegameProfile.ProfileDirectory.FullName;
                lbPath.Enabled = true;
                lbType.Text = sgLoc.savegameProfile.SaveProfileType.ToString();
                btnStartWatcher.Enabled = true;
                switch (sgLoc.savegameProfile.SaveProfileType)
                {
                    case SavegameType.Steam:
                        button2.Enabled = true;
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while detecting savegame folder: " + e.Message + "\r\n" + e.InnerException);
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (sgLoc.savegameProfile.SaveProfileType)
            {
                case SavegameType.Steam:
                    Process.Start("steam://rungameid/275850");
                    break;

                    //case SavegameType.GoG:
                    //    TODO: Find GalaxyClient/GameExecutable
                    //    ProcessStartInfo sInfo = new ProcessStartInfo(.../GalaxyClient.exe /command=runGame /gameId=?? /path=...);
                    //    Process.Start(sInfo);
                    //    break;
            }
        }

        private void btnStartWatcher_Click(object sender, EventArgs e)
        {
            StartWatcher();
        }

        private void btnStopWatcher_Click(object sender, EventArgs e)
        {
            StopWatcher();
        }


        private void StartWatcher()
        {
            try
            {
                sgLoc.StartWatcher();
                btnStartWatcher.Enabled = false;
                btnStopWatcher.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Error setting up savegame watcher");
            }
        }

        private void StopWatcher()
        {
            try
            {
                sgLoc.StopWatcher();
                btnStartWatcher.Enabled = true;
                btnStopWatcher.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Error stopping savegame watcher");
            }
        }

        public void UpdateLastProtected()
        {
            var lastSaveTimestamp = DatabaseHelpers.GetLastSaveTimestamp();
            lbLastProtected.Text = lastSaveTimestamp == null ? "-" : lastSaveTimestamp.Value.ToShortDateString() + " " + lastSaveTimestamp.Value.ToLongTimeString();
        }
        public void UpdateDbSize()
        {
            var res = (double)DatabaseHelpers.GetSizeOnDisk();
            lbDbSize.Text = Math.Round(res / 1024 / 1024, 2) + " MB";
            lbNumberOfBackups.Text = DatabaseHelpers.GetNumberOfSavegames().ToString();
        }

        // todo: this is ugly
        public SavegameQueueItem GetMostRecentSaveById(uint id)
        {
            return sgLoc.GetMostRecentSaveObject(id);
        }

        // todo: even worse
        public bool PauseWatcher()
        {
            return sgLoc.PauseWatcher();
        }

        // todo: why oh why!?
        public DirectoryInfo GetSavegameRootPath()
        {
            return sgLoc.savegameProfile.ProfileDirectory ?? null;
        }

        public void UnpauseWatcher()
        {
            sgLoc.UnpauseWatcher();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dlg = new DatabaseViewForm(this);
            dlg.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog();
        }

        private void DisplayChangelogForm()
        {
            var clForm = new ChangelogForm();
            clForm.ShowDialog();
        }

        private void lbPath_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("explorer.exe", lbPath.Text);
            Process.Start(sInfo);
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayChangelogForm();
        }

        private void SettingsToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            autostartWatcherOnLaunchToolStripMenuItem.Checked = Settings.Default.AutostartWatcherOnLaunch;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void autostartWatcherOnLaunchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.AutostartWatcherOnLaunch = Settings.Default.AutostartWatcherOnLaunch == true ? false : true;
            Settings.Default.Save();
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/theappleman/nomanssavegamemanager/issues");
            Process.Start(sInfo);
        }
    }
}
