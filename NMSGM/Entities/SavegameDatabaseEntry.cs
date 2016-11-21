using NMSGM.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMSGM.Entities
{
    public class SavegameDatabaseEntry
    {
        public int Id { get; set; }
        public string mfBlobId { get; set; }
        public string stBlobId { get; set; }
        public DateTime commitedTimeStamp { get; set; }
        public bool onHold { get; set; }
        public SavegameType Type { get; set; }
        public string comment { get; set; }
        public string decryptionSeed { get; set; }
    }
}
