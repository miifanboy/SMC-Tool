using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smctool
{
    class ModFiles
    {
        public void CopyFiles(string targetpath)
        {
            string [] files = Directory.GetFiles(Directory.GetCurrentDirectory()+ "\\data\\");
            
            foreach(string file in files)
            {
                File.Copy(file, targetpath + Path.GetFileName(file),true);
            }
        }
        public void SetupVPK(string filepath)
        {
            string dest = Directory.GetCurrentDirectory() + "\\pak01\\scripts\\items\\items_game_csgo.txt";
            
            File.Copy(filepath, dest,true);
            
        }
        public void Move(string source,string dest)
        {
            if (File.Exists(@dest))
            {
                File.Delete(@dest);
            }
            File.Move(source,dest);
        }
        public void CreateVPK(string filepath)
        {
            string vpk = "\"" + filepath + "\\bin\\vpk.exe" + "\"";
            string pak01 = Directory.GetCurrentDirectory() + "\\pak01\\";
            string mod = filepath + "\\smc\\csgo\\addons\\";
            var process = Process.Start(@vpk, "-M " + @pak01);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.WaitForExit();
            Move(Directory.GetCurrentDirectory() + "\\pak01_000.vpk", filepath + "\\smc\\csgo\\pak01_000.vpk");
            Move(Directory.GetCurrentDirectory() + "\\pak01_dir.vpk", filepath + "\\smc\\csgo\\pak01_dir.vpk");
            var process2 = Process.Start(@vpk , "-v "+ Directory.GetCurrentDirectory() + "\\addons\\m_smcmod\\");
            process2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process2.WaitForExit();
            
            Move(Directory.GetCurrentDirectory() + "\\addons\\m_smcmod.vpk", mod + "m_smcmod.vpk");
            File.Copy(Directory.GetCurrentDirectory() + "\\addons\\m_smcmod\\scripts\\items\\items_game_smcmod.txt", filepath + "\\smc\\csgo\\addons\\m_smcmod\\scripts\\items\\items_game_smcmod.txt",true);
        }
        public void LaunchMod()
        {
            string steamPath = (string)Registry.GetValue("HKEY_CURRENT_USER\\Software\\Valve\\Steam", "SteamPath", "");
            Process.Start(steamPath + "\\steam.exe", "  -applaunch 730 -game smc/csgo");
        }
    }
}
