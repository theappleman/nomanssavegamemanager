using NMSGM.Entities;
using NMSGM.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NMSGM.Classes
{
    public class SavegameLocationManager
    {
        public SavegamePathInformation savegameProfile;
        MainForm _main;
        Queue<SavegameQueueItem> storeQ = new Queue<SavegameQueueItem>();
        Task DbLoopTask;
        FileSystemWatcher fsWatcher;


        public SavegameLocationManager(MainForm main)
        {
            _main = main;
            FindSavegameFolder();
        }

        private void FindSavegameFolder()
        {
            var saveRootDir = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HelloGames", "NMS"));

            if(!saveRootDir.Exists)
            {
                throw new FileNotFoundException("Save Root directory was not found");
            }
            else
            {
                var profileFolders = saveRootDir.EnumerateDirectories();

                if (profileFolders.Count() > 1)
                    throw new NotSupportedException("Multiple Savegame Profiles found. This is not yet supported. Please ensure " + saveRootDir.FullName + " does only contain one folder");

                if (profileFolders.Count() < 1)
                    throw new NotSupportedException("No Savegame Profiles found. Please ensure " + saveRootDir.FullName + " does actually contain Steam/Gog folders");

                var chosenProfileFolder = profileFolders.SingleOrDefault();

                savegameProfile = new SavegamePathInformation();
                savegameProfile.ProfileDirectory = chosenProfileFolder;

                if (chosenProfileFolder.Name.StartsWith("st_"))
                {
                    savegameProfile.SaveProfileType = SavegameType.Steam;
                    savegameProfile.EncryptionSeed = chosenProfileFolder.Name.Replace("st_", "");
                }
                else if (chosenProfileFolder.Name == "DefaultUser")
                {
                    savegameProfile.SaveProfileType = SavegameType.GoG;
                    savegameProfile.EncryptionSeed = "DefaultUser";
                }
                else
                {
                    throw new Exception("Save profile type cannot be determined");
                }
            }

        }

        
        public void StartWatcher()
        {
            if (savegameProfile == null)
                throw new Exception("Savegame path not confirmed.");

            fsWatcher = new FileSystemWatcher();
            fsWatcher.Path = savegameProfile.ProfileDirectory.FullName;
            fsWatcher.Filter = "save*.hg";
            fsWatcher.Changed += FsWatch_Changed;
            fsWatcher.Created += FsWatch_Changed;
            fsWatcher.Deleted += FsWatch_Changed;

            fsWatcher.EnableRaisingEvents = true;

            DbLoopTask = Task.Run(() => DbLoop());
        }

        public bool PauseWatcher()
        {
            if (fsWatcher.EnableRaisingEvents == true)
            {
                fsWatcher.EnableRaisingEvents = false;
                return true;
            }
            return false;
        }

        public void UnpauseWatcher()
        {
            if (fsWatcher.EnableRaisingEvents == false)
                fsWatcher.EnableRaisingEvents = true;
        }


        internal void StopWatcher()
        {
            fsWatcher.Dispose();            
        }


        private void FsWatch_Changed(object sender, FileSystemEventArgs e)
        {
            if(isValidSavegameName(e.Name))
            {
                if (storeQ.Count(p => p.SaveStoragePath == e.FullPath) == 0)
                {
                    var queueItem = new SavegameQueueItem()
                    {
                        DetectionTimestamp = DateTime.Now,
                        WriteRetries = 0,
                        SaveStoragePath = e.FullPath,
                        SaveMetafilePath = Path.Combine(Path.GetDirectoryName(e.FullPath), "mf_" + e.Name),
                        decryptionSeed = savegameProfile.EncryptionSeed,
                        Type = savegameProfile.SaveProfileType,
                    };

                    storeQ.Enqueue(queueItem);
                }
            }
        }

        public SavegameQueueItem GetMostRecentSaveObject(uint profileId)
        {
            var recentStorageFile = savegameProfile.ProfileDirectory.GetFiles(SavegameHelper.GetStorageFilenameFromProfileId(profileId), SearchOption.TopDirectoryOnly).OrderByDescending(p => p.LastWriteTime).FirstOrDefault();
            if (recentStorageFile != null)
            {
                var queueItem = new SavegameQueueItem()
                {
                    DetectionTimestamp = DateTime.Now,
                    WriteRetries = 0,
                    SaveStoragePath = recentStorageFile.FullName,
                    SaveMetafilePath = Path.Combine(Path.GetDirectoryName(recentStorageFile.FullName), "mf_" + recentStorageFile.Name),
                    decryptionSeed = savegameProfile.EncryptionSeed,
                    Type = savegameProfile.SaveProfileType,
                    Comment = "[NMSGM] Automatic backup of savegame slot overwritten during restore."

                };
                return queueItem;
            }
            else
                return null;
        }

        private bool isValidSavegameName(string filename)
        {
            var regexValidName = new Regex("^(mf_)?save2?.hg$");
            return regexValidName.IsMatch(filename);
        }

        private async void DbLoop()
        {
            while (1==1)
            {

                if(storeQ.Count > 0)
                {
                    var db = new SaveStorage();
                    while (storeQ.Count > 0)
                    {
                        var itm = storeQ.Dequeue();

                        try
                        {
                            db.SaveFileToDb(itm, _main);
                        }
                        catch (FileNotFoundException e)
                        {
                            // this happens if the file is actually not present (e.g. watcher has been triggered by cut/delete/move operation
                            // we just do nothing here which will keep the item dequeued
                        }
                        catch (Exception e)
                        {
                            // this will likeley only happen if the NMS process still has a lock on the file. We are retrying several times
                            if(itm.WriteRetries <= 4)
                            {
                                itm.WriteRetries++;
                                storeQ.Enqueue(itm);
                                Thread.Sleep(500);
                            }
                            else
                            {
                                itm.WriteRetries++;
                            }
                        }

                    }
                }
                Thread.Sleep(2000);
            }
        }

        public class SavegameQueueItem
        {
            public string SaveMetafilePath { get; set; }
            public string SaveStoragePath { get; set; }
            public DateTime DetectionTimestamp { get; set; }
            public int WriteRetries { get; set; }
            public string decryptionSeed { get; set; }
            public SavegameType Type { get; set; }
            public string Comment { get; set; }
        }
    }

    public class SavegamePathInformation
    {
        public DirectoryInfo ProfileDirectory { get; set; }
        public SavegameType SaveProfileType { get; set; }
        public string EncryptionSeed { get; set; }
    }


}
