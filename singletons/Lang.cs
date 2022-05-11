using System;
using System.IO;

namespace StereoStructure
{
    static class Lang
    {
        public static string GUI_PROGRAM_NAME = "StereoStructure";
        public static string GUI_MODEL = "Model";
        public static string GUI_SELECT_FILE = "Open";
        public static string GUI_SCAN_VIDEO = "Create from video";
        public static string GUI_SETTINGS = "Settings";
        public static string GUI_ABOUT = "About";
        public static string GUI_SAVE_AS = "Save as...";
        public static string GUI_LOGS = "Logs";
        public static string GUI_LANGUAGE = "Language";
        public static string GUI_RUSSIAN = "Russian";
        public static string GUI_ENGLISH = "English";
        public static string GUI_AUTHOR = "Author";
        public static string GUI_HOW_TO_USE = "How to use";
        public static string GUI_CORRESPONDENCES = "Correspondences";
        public static string GUI_EDIT = "Edit mode";
        public static string GUI_ACCURACY = "Accuracy";
        public static string GUI_ACCURACY_LOW = "Low";
        public static string GUI_ACCURACY_MEDIUM = "Medium";
        public static string GUI_ACCURACY_HIGH = "High";
        public static string GUI_ADVANCED = "Advanced...";
        public static string GUI_GRID_WIDTH = "Grid Width";
        public static string GUI_GRID_LENGTH = "Grid Length";
        public static string GUI_GRID_THICKNESS = "Grid Thickness";
        public static string GUI_DEFAULT_COLOR = "Default color";
        public static string GUI_MAX_MODEL_WIDTH = "Maximum model width";
        public static string GUI_CORRESPONDENCES_CIRCLE_WIDTH = "Correspondences circle width";
        public static string GUI_SKIP_FRAMES_COUNT = "Skip frames count";
        public static string GUI_ROTATE = "Rotate images in video";
        public static string GUI_ROTATE_NONE = "None";
        public static string GUI_ROTATE_90 = "Rotate 90°";
        public static string GUI_ROTATE_180 = "Rotate 180°";
        public static string GUI_ROTATE_270 = "Rotate 270°";

        public static void Load()
        {
            string fileName = "EN.txt";
            if (SettingsListener.Get().lang == LANG.RU) fileName = "RU.txt";
            string path = SettingsListener.GetPath()+"langs\\"+fileName;
            if (File.Exists(path))
            {
                try
                {
                    string[] lines = File.ReadAllLines(path);
                    GUI_PROGRAM_NAME = lines[0];
                    GUI_MODEL = lines[1];
                    GUI_SELECT_FILE = lines[2];
                    GUI_SCAN_VIDEO = lines[3];
                    GUI_SETTINGS = lines[4];
                    GUI_ABOUT = lines[5];
                    GUI_SAVE_AS = lines[6];
                    GUI_LOGS = lines[7];
                    GUI_LANGUAGE = lines[8];
                    GUI_RUSSIAN = lines[9];
                    GUI_ENGLISH = lines[10];
                    GUI_AUTHOR = lines[11];
                    GUI_HOW_TO_USE = lines[12];
                    GUI_CORRESPONDENCES = lines[13];
                    GUI_EDIT = lines[14];
                    GUI_ACCURACY = lines[15];
                    GUI_ACCURACY_LOW = lines[16];
                    GUI_ACCURACY_MEDIUM = lines[17];
                    GUI_ACCURACY_HIGH = lines[18];
                    GUI_ADVANCED = lines[19];
                    GUI_GRID_WIDTH = lines[20];
                    GUI_GRID_LENGTH = lines[21];
                    GUI_GRID_THICKNESS = lines[22];
                    GUI_DEFAULT_COLOR = lines[23];
                    GUI_MAX_MODEL_WIDTH = lines[24];
                    GUI_CORRESPONDENCES_CIRCLE_WIDTH = lines[25];
                    GUI_SKIP_FRAMES_COUNT = lines[26];
                    GUI_ROTATE = lines[27];
                    GUI_ROTATE_NONE = lines[28];
                    GUI_ROTATE_90 = lines[29];
                    GUI_ROTATE_180 = lines[30];
                    GUI_ROTATE_270 = lines[31];
                } catch(Exception ex)
                {
                    Logs.Write(ex.Message+ex.StackTrace, LogType.ERROR);
                }
            }
            else Logs.Write("Language file "+path+" not found", LogType.WARNING);
        }
    }
}
