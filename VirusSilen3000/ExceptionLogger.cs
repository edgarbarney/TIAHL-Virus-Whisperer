using System;
using System.IO;
using CryptoStuffNamespace;

namespace VirusSilen3000
{
    class ExceptionLogger
    {
        public string pass = "theres_no_real_pass_its_just_for_protection_from_unwanted";
        public void ExcepionMessageCalc(Exception ex)
        {
            string exceptMessage = ex.ToString();
            string exceptLabel = ("Critical Error:" + ex.GetType().ToString());
            string[] gonnaWrite = { exceptMessage, Environment.NewLine, exceptLabel };
            ErrorWindow ew = new ErrorWindow(exceptMessage, exceptLabel, true);
            ew.ShowDialog();
            string dateStr = DateTime.Now.ToString("yyyy-dd-M_HH-mm-ss");
            string fileDir = (AppDomain.CurrentDomain.BaseDirectory + "error_" + dateStr + ".debug");
            string tempDir = (Path.GetTempPath() + "error_" + dateStr + ".debug");
            using (StreamWriter sWriter = new System.IO.StreamWriter(tempDir))
            {
                foreach (string line in gonnaWrite)
                {
                    sWriter.WriteLine();
                }
            }
            CryptoStuff.EncryptFile(pass, tempDir, fileDir);
            return;
        }
    }
}
