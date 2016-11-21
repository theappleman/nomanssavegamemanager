using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSGM.Helpers
{
    public static class SavegameHelper
    {
        public static uint GetProfileFromFilename(string filename)
        {
            switch (filename)
            {
                case "storage.hg":
                    return 0;
                case "storage2.hg":
                    return 1;
                case "storage3.hg":
                    return 2;
                default:
                    throw new InvalidDataException();
            }
        }

        public static string GetStorageFilenameFromProfileId(uint id)
        {
            switch (id)
            {
                case 0:
                    return "storage.hg";
                case 1:
                    return "storage2.hg";
                case 2:
                    return "storage3.hg";
                default:
                    throw new InvalidDataException();
            }
        }
    }
}
