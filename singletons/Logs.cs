using System;
using System.IO;

namespace StereoStructure
{
    static class Logs
    {
        private static LogsWindow logs;

        public static void Show(MainWindow main, string initStr = "StereoStructures loaded successfully!")
        {
            File.WriteAllText(SettingsListener.GetPath() + "logs.txt", String.Empty);
            logs = new LogsWindow(main);
            logs.Show();
            Write(initStr);
        }

        public static void Close()
        {
            logs.Close();
        }

        public static bool IsOpened()
        {
            if (logs == null) return false;
            return logs.IsLoaded;
        }

        public static void Write(string s, LogType logType = LogType.INFO)
        {
            string type = "[INFO]";
            if (logType == LogType.WARNING) type = "[WARNING]";
            else if (logType == LogType.ERROR) type = "[ERROR]";
            string time = DateTime.Now.ToString("HH:mm");
            string res = time + ": " + type + " " + s + "\n";
            File.AppendAllText(SettingsListener.GetPath() + "logs.txt", res);
            logs.logs.Text = res + logs.logs.Text;
        }

        public static void WriteMainThread(string s, LogType logType = LogType.INFO)
        {
            logs.Dispatcher.Invoke(() =>
            {
                Write(s, logType);
            });
        }

        public static void ReplaceMainThread(string s, LogType logType = LogType.INFO)
        {
            PopMainThread();
            WriteMainThread(s, logType);
        }

        public static void Pop()
        {
            string[] spl = logs.logs.Text.Split('\n');
            string res = "";
            for(int i = 1; i < spl.Length; ++i)
            {
                res += spl[i]+"\n";
            }
            logs.logs.Text = res;
        }

        public static void PopMainThread()
        {
            logs.Dispatcher.Invoke(() =>
            {
                Pop();
            });
        }

        public static void Replace(string s, LogType logType = LogType.INFO)
        {
            Pop();
            Write(s, logType);
        }
    }
}
