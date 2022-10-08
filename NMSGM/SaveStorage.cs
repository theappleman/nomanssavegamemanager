using LiteDB;
using NMSGM.Classes;
using NMSGM.Entities;
using System;

using static NMSGM.Classes.SavegameLocationManager;

namespace NMSGM
{
    public class SaveStorage
    {
        private MainForm _main;

        public void SaveFileToDb(SavegameQueueItem itm, MainForm main)
        {
            _main = main;
            using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
            {
                var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");


                var guid = Guid.NewGuid();
                var mfBlobInfo = db.FileStorage.Upload(Guid.NewGuid().ToString(), itm.SaveMetafilePath);
                var stBlobInfo = db.FileStorage.Upload(Guid.NewGuid().ToString(), itm.SaveStoragePath);

                var entry = new SavegameDatabaseEntry()
                {
                    mfBlobId = mfBlobInfo.Id,
                    stBlobId = stBlobInfo.Id,
                    commitedTimeStamp = DateTime.Now,
                    onHold = false,
                    decryptionSeed = itm.decryptionSeed,
                    Type = itm.Type,
                };

                if (itm.Comment != null)
                    entry.comment = itm.Comment;

                saveIndex.Insert(entry);
            }

            _main.Invoke(new Action(() => _main.UpdateLastProtected()));
            _main.Invoke(new Action(() => _main.UpdateDbSize()));

        }





    }
}
