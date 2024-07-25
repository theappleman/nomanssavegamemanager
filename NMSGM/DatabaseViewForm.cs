using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NMSGM;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static NMSGM.SaveStorage;
using System.Diagnostics;
using NMSGM.Entities;
using NMSGM.Helpers;
using NMSGM.Classes;

namespace NMSGM
{
    public partial class DatabaseViewForm : Form
    {
        MainForm _main;

        public DatabaseViewForm(MainForm main)
        {
            _main = main;
            InitializeComponent();
            ReloadObjectsFromDb();
        }

        private void ReloadObjectsFromDb()
        {
            using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
            {
                var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                var objects = saveIndex.FindAll().OrderByDescending(p => p.commitedTimeStamp);

                olvBackups.Items.Clear();
                olvBackups.SetObjects(objects);
                olvBackups.Sort(olvBackupsTimestamp, SortOrder.Descending);
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            ParseSavegame();
        }


        private void ExportSavegame()
        {
            if(olvBackups.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select exactly one row to export");
            }
            else
            {
                try
                {
                    var entryToExport = (SavegameDatabaseEntry)olvBackups.SelectedObject;

                    // todo: replace by check and consent
                    MessageBox.Show("Existing items will not be overwritten!");

                    var dstFldPicker = new FolderBrowserDialog();
                    dstFldPicker.Description = "Choose folder for export";
                    var pickerResult = dstFldPicker.ShowDialog();

                    if (pickerResult == DialogResult.OK)
                    {
                        var dstFolder = dstFldPicker.SelectedPath;

                        using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
                        {
                            var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                            var objects = saveIndex.FindById(entryToExport.Id);

                            var sto = db.FileStorage.FindById(objects.stBlobId);
                            var mf = db.FileStorage.FindById(objects.mfBlobId);

                            sto.SaveAs(Path.Combine(dstFolder, sto.Filename), false);
                            mf.SaveAs(Path.Combine(dstFolder, mf.Filename), false);
                        }

                        MessageBox.Show("Export successful");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error on export: " + ex.Message);
                }
            }
        }

        private void DeleteSavegames()
        {
            if (olvBackups.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select one or more rows to delete");
            }
            else
            {
                try
                {
                    var itemsToDelete = olvBackups.SelectedObjects.Cast<SavegameDatabaseEntry>();

                    var deleteWarningResult = MessageBox.Show(string.Format("You are about to delete {0} savegames. Proceed?", itemsToDelete.Count()), "Really delete savegames?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if(deleteWarningResult == DialogResult.Yes)
                    {
                        using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
                        {
                            var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");

                            foreach(var itm in itemsToDelete)
                            {
                                var mfBlobId = itm.mfBlobId;
                                var stBlobId = itm.stBlobId;

                                db.FileStorage.Delete(mfBlobId);
                                db.FileStorage.Delete(stBlobId);
                                saveIndex.Delete(itm.Id);
                            }

                            //todo: review if fixed: https://github.com/mbdavid/LiteDB/issues/249
                            //db.Shrink();
                            _main.Invoke(new Action(() => _main.UpdateDbSize()));
                        }
                        ReloadObjectsFromDb();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error on deletion: " + ex.Message);
                }
            }
        }



        private void ParseSavegame()
        {
            // not yet my dear
        }



        private void UpdateSgDbObject(SavegameDatabaseEntry sgo)
        {
            using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
            {
                var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                saveIndex.Update(sgo);
            }
            ReloadObjectsFromDb();
        }

        private void olvBackups_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
        {
            if (e.RowObject.GetType() == typeof(SavegameDatabaseEntry))
            {
                UpdateSgDbObject((SavegameDatabaseEntry)e.RowObject);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportSavegame();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteSavegames();
        }

        private void olvBackups_SubItemChecking(object sender, BrightIdeasSoftware.SubItemCheckingEventArgs e)
        {
            if (e.RowObject.GetType() == typeof(SavegameDatabaseEntry) && e.Column.AspectName == "onHold")
            {
                var obj = (SavegameDatabaseEntry)e.RowObject;
                obj.onHold = e.NewValue == CheckState.Checked ? true : false;
                UpdateSgDbObject(obj);
            }

        }

        //VERY ugly

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (olvBackups.SelectedItems.Count != 1)
            {
                MessageBox.Show("Please select exactly one row to export");
            }
            else
            {
                var entryToRestore = (SavegameDatabaseEntry)olvBackups.SelectedObject;

                var nmsProc = Process.GetProcesses().Count(p => p.ProcessName.ToLower() == "nms");

                if (nmsProc != 0)
                {
                    MessageBox.Show("No Man's Sky is currently running.\r\n " +
                        "There is an experimental feature that lets you restore savegames even when NMS is running. If you are using this you will have to re-open the ingame menu to see the restored savegame selectable for loading.\r\n\r\n" +
                        "The restored savegame will show up as the most recent (current) save\r\n\r\n" +
                        "In case this is not working for you please close NMS before restoring");
                }

                // get most recent save (which will be overwritten)
                // we have to use the correct index!!!
                uint id;

                using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
                {
                    var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                    var objects = saveIndex.FindById(entryToRestore.Id);

                    var sto = db.FileStorage.FindById(objects.stBlobId);
                    id = SavegameHelper.GetProfileFromFilename(sto.Filename);
                }


                var recentSave = _main.GetMostRecentSaveById(id);
                bool preRestoreSaveSuccess = false;

                if (recentSave != null)
                {
                    try
                    {
                        var db = new SaveStorage();
                        db.SaveFileToDb(recentSave, _main);
                        preRestoreSaveSuccess = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Automatic backup of most recent save before overwriting by restore failed. Please try again: " + ex.Message);
                    }
                }
                else
                {
                    // hacky, we need this so restores to empty savegame dirs are possible
                    preRestoreSaveSuccess = true;
                }

                if (preRestoreSaveSuccess)
                {
                    try
                    {
                        var watcherPaused = _main.PauseWatcher();
                        var savegameProfilePath = _main.GetSavegameRootPath();

                        using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
                        {
                            var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                            var objects = saveIndex.FindById(entryToRestore.Id);

                            var sto = db.FileStorage.FindById(objects.stBlobId);
                            var mf = db.FileStorage.FindById(objects.mfBlobId);

                            var stoPath = Path.Combine(savegameProfilePath.FullName, sto.Filename);
                            var mfPath = Path.Combine(savegameProfilePath.FullName, mf.Filename);

                            sto.SaveAs(stoPath, true);
                            mf.SaveAs(mfPath, true);

                            File.SetLastWriteTime(stoPath, DateTime.Now);
                            File.SetLastWriteTime(mfPath, DateTime.Now);
                        }

                        if (watcherPaused)
                            _main.UnpauseWatcher();

                        MessageBox.Show("Restore successful");
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("Restore failed: " + exc.Message);
                    }
                }
                ReloadObjectsFromDb();
            }
        }
    }
}
