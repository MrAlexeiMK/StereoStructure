using System;
using System.IO;

namespace StereoStructure
{
    static class SettingsListener
    {
        private static Settings settings;
        private static string path;
        private static string projectPath;
        private static string clientPath;
        public static void Load()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\StereoStructure\\";
            projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\";
            clientPath = AppDomain.CurrentDomain.BaseDirectory + "\\";
            Directory.CreateDirectory(path);
            Directory.CreateDirectory(path+"models\\");
            Directory.CreateDirectory(path+"langs\\");
            Directory.CreateDirectory(path+"frames\\");
            try
            {
                settings = JSON.ReadFromJsonFile<Settings>(path + "settings.json");
            }
            catch
            {
                settings = new Settings();
            }

            LoadLangs(projectPath);
            LoadLangs(clientPath);
            LoadModels(projectPath);
            LoadModels(clientPath);
        }

        private static void LoadLangs(string from)
        {
            try
            {
                string src = SettingsListener.GetPath() + "langs\\EN.txt";
                if (!File.Exists(src))
                {
                    File.Copy(from + "langs\\EN.txt", src);
                }
                src = SettingsListener.GetPath() + "langs\\RU.txt";
                if (!File.Exists(src))
                {
                    File.Copy(from + "langs\\RU.txt", src);
                }
            }
            catch { }
        }

        private static void LoadModels(string from)
        {
            string src;
            try
            {
                string[] filePaths = Directory.GetFiles(from + "models\\");
                foreach (string filePath in filePaths)
                {
                    string[] spl = filePath.Split('\\');
                    string fileName = spl[spl.Length - 1];
                    src = path + "models\\" + fileName;
                    if (!File.Exists(src))
                    {
                        File.Copy(filePath, src);
                    }
                }
            }
            catch { }
        }

        public static void Save()
        {
            JSON.WriteToJsonFile(path+"settings.json", settings);
        }

        public static Settings Get()
        {
            return settings;
        }

        public static string GetPath()
        {
            return path;
        }
    }
}
