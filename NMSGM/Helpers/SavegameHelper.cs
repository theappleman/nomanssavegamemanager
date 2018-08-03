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
                case "save.hg":
                    return 0;
                case "save2.hg":
                    return 1;
                default:
                    throw new InvalidDataException();
            }
        }

        public static string GetStorageFilenameFromProfileId(uint id)
        {
            switch (id)
            {
                case 0:
                    return "save.hg";
                case 1:
                    return "save2.hg";
                default:
                    throw new InvalidDataException();
            }
        }
    }
}
