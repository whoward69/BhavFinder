using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BhavFinder
{
    static partial class BhavFinderApp
    {
        public static String AppName = "BHAV Finder";
        public static String AppVersion = "1.2b";
        public static String AppCopyright = "CopyRight (c) 2020 - William Howard";

        public static String AppProduct = AppName + " Version " + AppVersion;

        public static String RegistryKey = @"WHoward\BhavFinder";

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BhavFinderForm());
        }
    }
}
