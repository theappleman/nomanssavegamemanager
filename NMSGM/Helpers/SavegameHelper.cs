using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NMSGM.Helpers
{
    public static class SavegameHelper
    {
        public static uint GetProfileFromFilename(string filename)
        {
            if (filename == "save.hg")
            {
                return 0;
            }

            Regex rx = new Regex("save(?<slot>[2-9]|10)\\.hg", RegexOptions.Compiled);
            MatchCollection matches = rx.Matches(filename);
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                return System.Convert.ToUInt32(groups["slot"].Value) - 1;
            }
            throw new InvalidDataException();
        }

        public static string GetStorageFilenameFromProfileId(uint id)
        {
            if (id == 0)
            {
                return "save.hg";
            } else if ( 2 <= id && id <= 10 )
            {
                return string.Concat("save", id + 1, ".hg");
            }
            throw new InvalidDataException();
        }
    }
}
