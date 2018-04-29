using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    class Files
    {
        public static void Load_Config()
        {
            try
            {
                byte[] data = File.ReadAllBytes(Global.Config_File);
                Global.msConfig = new MemoryStream(data);
            }
            catch(IOException ex)
            {
                Errors.Manage_Errors(ex);
            }
        }
    }
}
