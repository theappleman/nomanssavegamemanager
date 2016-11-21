using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace NMSGM
{
    public static class NMSGMSettings
    {
        public static string DbFilePath
        {
            get
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var dbPath = Path.Combine(appData, "NMSGM");

                var dirObject = Directory.CreateDirectory(dbPath);
                return Path.Combine(dirObject.FullName, "nmsgmdb.db");
            }
        }

        public static Version GetDeploymentVersion()
        {
            if (ApplicationDeployment.IsNetworkDeployed)
                return ApplicationDeployment.CurrentDeployment.CurrentVersion;
            else
                return null;
        }
    }
}
