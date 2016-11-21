using LiteDB;
using NMSGM.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSGM
{
    public static class DatabaseHelpers
    {
        public static long GetSizeOnDisk()
        {
            try
            {
                return new FileInfo(NMSGMSettings.DbFilePath).Length;
            }
            catch
            {
                return 0;
            }
        }

        public static int GetNumberOfSavegames()
        {
            using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
            {
                var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");
                return saveIndex.Count();
            }
        }

        internal static DateTime? GetLastSaveTimestamp()
        {
            using (var db = new LiteDatabase(NMSGMSettings.DbFilePath))
            {
                var saveIndex = db.GetCollection<SavegameDatabaseEntry>("SavegameIndexV1");

                try
                {
                    var last = saveIndex.Find(Query.All("commitedTimeStamp", Query.Descending), 0, 1).FirstOrDefault().commitedTimeStamp;
                    return Convert.ToDateTime(last);
                }
                catch // the above will fail if the DB is empty
                {
                    return null;
                }
            }
        }
    }
}
