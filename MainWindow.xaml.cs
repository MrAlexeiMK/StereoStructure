using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Accord.Video.FFMPEG;
using HelixToolkit.Wpf;
using System.Windows.Input;
using System.IO;
using System.Threading;

namespace StereoStructure
{
    public partial class MainWindow : Window
    {
        private OpenFileDialog videoFileDialog = new OpenFileDialog();
        private OpenFileDialog modelFileDialog = new OpenFileDialog();
        private SaveFileDialog modelSaveDialog = new SaveFileDialog();
        private Model model = new Model();
        private Thread videoThread, modelThread;
        private int operatorType = 1;
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private void AcceptSettings()
        {
            LogsItem.IsChecked = SettingsListener.Get().openLogs;
            CorItem.IsChecked = SettingsListener.Get().correspondences;
            RU.IsChecked = SettingsListener.Get().lang == LANG.RU;
            EN.IsChecked = SettingsListener.Get().lang == LANG.EN;
            LowAccuracy.IsChecked = SettingsListener.Get().accuracy == ACCURACY.LOW;
            MediumAccuracy.IsChecked = SettingsListener.Get().accuracy == ACCURACY.MEDIUM;
            HighAccuracy.IsChecked = SettingsListener.Get().accuracy == ACCURACY.HIGH;
        }

        private void Load()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            SettingsListener.Load();
            Lang.Load();

            if (SettingsListener.Get().openLogs)
            {
                Logs.Show(this);
            }

            Title = Lang.GUI_PROGRAM_NAME;
            SelectFile.Text = Lang.GUI_SELECT_FILE;
            Model.Text = Lang.GUI_MODEL;
            ScanVideo.Text = Lang.GUI_SCAN_VIDEO;
            Settings.Text = Lang.GUI_SETTINGS;
            About.Text = Lang.GUI_ABOUT;
            saveAsButton.Header = Lang.GUI_SAVE_AS;
            LogsItem.Header = Lang.GUI_LOGS;
            LangItem.Header = Lang.GUI_LANGUAGE;
            RU.Header = Lang.GUI_RUSSIAN;
            EN.Header = Lang.GUI_ENGLISH;
            Author.Header = Lang.GUI_AUTHOR;
            FAQ.Header = Lang.GUI_HOW_TO_USE;
            CorItem.Header = Lang.GUI_CORRESPONDENCES;
            EditItem.Header = Lang.GUI_EDIT;
            AccuracyItem.Header = Lang.GUI_ACCURACY;
            LowAccuracy.Header = Lang.GUI_ACCURACY_LOW;
            MediumAccuracy.Header = Lang.GUI_ACCURACY_MEDIUM;
            HighAccuracy.Header = Lang.GUI_ACCURACY_HIGH;
            AdvancedItem.Header = Lang.GUI_ADVANCED;

            videoFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            videoFileDialog.RestoreDirectory = true;
            videoFileDialog.Filter = Constants.videoFormats;

            modelFileDialog.InitialDirectory = SettingsListener.GetPath()+"models\\";
            modelFileDialog.RestoreDirectory = true;
            modelFileDialog.Filter = Constants.modelFormtats;

            modelSaveDialog.InitialDirectory = SettingsListener.GetPath()+"models\\";
            modelSaveDialog.RestoreDirectory = true;
            modelSaveDialog.Filter = Constants.modelFormtats;

            AcceptSettings();
        }

        public void Restart()
        {
            SettingsListener.Save();
            System.Windows.Forms.Application.Restart();
            Environment.Exit(0);
        }

        private void Update()
        {
            ObjReader objReader = new ObjReader();
            string path = modelFileDialog.FileName;
            try
            {
                model.Save(path);
            }
            catch (Exception ex)
            {
                Logs.Write("Model save error: " + path, LogType.ERROR);
                Logs.Write(ex.Message + ex.StackTrace, LogType.ERROR);
            }
            model3d.Content = objReader.Read(path);
            Logs.Write("Model was saved");
        }

        private void LoadModel(string path)
        {
            string[] spl = path.Split('.');
            string ext = spl[spl.Length - 1].ToLower();
            if(ext == "obj")
            {
                try
                {
                    ObjReader objReader = new ObjReader();
                    model.Load(path);
                    model.Save(path);
                    Dispatcher.Invoke(() =>
                    {
                        saveAsButton.IsEnabled = true;
                        model3d.Content = objReader.Read(path);
                        scene.ZoomExtents();
                    });
                } catch(Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Logs.Write(ex.Message + ex.StackTrace, LogType.ERROR);
                    });
                }
                Dispatcher.Invoke(() =>
                {
                    Logs.Write("Model was loaded");
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    Logs.Write("Not '.obj' format");
                });
            }
            Dispatcher.Invoke(() =>
            {
                EditItem.IsEnabled = true;
                wait.Opacity = 0;
                scene.Cursor = null;
            });
        }

        public void Scan(string path)
        {
            VideoFileReader reader = new VideoFileReader();
            try
            {
                reader.Open(path);

                DirectoryInfo di = new DirectoryInfo(SettingsListener.GetPath() + "frames\\");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Correspondences.Clear();
                Logs.WriteMainThread("Starting proccessing...");
                for (int i = 0; i < reader.FrameCount; ++i)
                {
                    Bitmap frame = reader.ReadVideoFrame();
                    Correspondences.Add(ref frame);
                    frame.Save(SettingsListener.GetPath() + "frames\\frame_" + i + ".jpg");
                    Logs.ReplaceMainThread("Proccessed frames: " + i + "/" + reader.FrameCount);
                    frame.Dispose();
                }

                Dispatcher.Invoke(() => {
                    Correspondences.Init((int)reader.FrameCount);
                    if (SettingsListener.Get().correspondences)
                    {
                        Correspondences.Show();
                        Logs.Write("Correspondences Window was loaded");
                    }
                    else
                    {
                        Logs.Write("Correspondences Window is disabled in Settings", LogType.WARNING);
                    }
                });
            }
            catch (Exception ex)
            {
                Logs.WriteMainThread(ex.Message + ex.StackTrace, LogType.ERROR);
            }
            reader.Close();
            Logs.WriteMainThread("Video File was processed");
            Dispatcher.Invoke(() =>
            {
                wait.Opacity = 0;
                scene.Cursor = null;
            });
        }

        private void AbortThreads()
        {
            if (modelThread != null && modelThread.IsAlive)
            {
                modelThread.Abort();
            }
            if (videoThread != null && videoThread.IsAlive)
            {
                videoThread.Abort();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            SettingsListener.Save();
            if (Logs.IsOpened())
            {
                Logs.Close();
            }
            if(Correspondences.IsOpened())
            {
                Correspondences.Close();
            }
            if(AdvancedListener.IsOpened())
            {
                AdvancedListener.Close();
            }
            AbortThreads();
        }

        private void author_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MrAlexeiMK/");
        }

        private void faq_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MrAlexeiMK/StereoStructure");
        }

        private void LogsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogsItem.IsChecked = !LogsItem.IsChecked;
            SettingsListener.Get().openLogs = LogsItem.IsChecked;
            if(Logs.IsOpened() && !LogsItem.IsChecked)
            {
                Logs.Close();
            }
            else if(!Logs.IsOpened() && LogsItem.IsChecked)
            {
                Logs.Show(this);
            }
        }

        private void CorItem_Click(object sender, RoutedEventArgs e)
        {
            CorItem.IsChecked = !CorItem.IsChecked;
            SettingsListener.Get().correspondences = CorItem.IsChecked;
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (model.IsLoaded())
            {
                EditItem.IsChecked = !EditItem.IsChecked;
                if (EditItem.IsChecked)
                {
                    Logs.Write("Edit mode was ENABLED. Use Mouse Wheel to change mode and Mouse Left Click to Rotate");
                    scene.Cursor = System.Windows.Input.Cursors.SizeWE;
                    scene.ZoomExtents();
                }
                else
                {
                    Logs.Write("Edit mode was DISABLED");
                    scene.Cursor = null;
                }
                scene.IsMoveEnabled = !EditItem.IsChecked;
                scene.IsPanEnabled = !EditItem.IsChecked;
                scene.IsZoomEnabled = !EditItem.IsChecked;
            }
        }

        private void AdvancedItem_Click(object sender, RoutedEventArgs e)
        {
            if(AdvancedListener.IsOpened())
            {
                AdvancedListener.Close();
            }
            AdvancedListener.Show(this);
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            if (modelFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AbortThreads();
                Logs.Write("3D Model was selected: " + modelFileDialog.FileName);
                wait.Opacity = 1;
                scene.Cursor = System.Windows.Input.Cursors.Wait;
                modelThread = new Thread(() => LoadModel(modelFileDialog.FileName));
                modelThread.Start();
            }
        }

        private void scan_Click(object sender, RoutedEventArgs e)
        {
            if (videoFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AbortThreads();
                if (Correspondences.IsOpened()) Correspondences.Close();
                Logs.Write("Video File was selected: " + videoFileDialog.FileName);
                wait.Opacity = 1;
                scene.Cursor = System.Windows.Input.Cursors.Wait;
                videoThread = new Thread(() => Scan(videoFileDialog.FileName));
                videoThread.Start();
            }
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            if (modelSaveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Logs.Write("Selected path:" + modelSaveDialog.FileName);
                try
                {
                    model.Save(modelSaveDialog.FileName);
                } catch(Exception ex)
                {
                    Logs.Write("Model save error: " + modelSaveDialog.FileName, LogType.ERROR);
                    Logs.Write(ex.Message + ex.StackTrace, LogType.ERROR);
                }
                Logs.Write("Model was saved");
            }
        }

        private void RU_Click(object sender, RoutedEventArgs e)
        {
            if(SettingsListener.Get().lang != LANG.RU)
            {
                SettingsListener.Get().lang = LANG.RU;
                Restart();
            }
        }

        private void EN_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsListener.Get().lang != LANG.EN)
            {
                SettingsListener.Get().lang = LANG.EN;
                Restart();
            }
        }

        private void scene_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (EditItem.IsChecked)
            {
                double angle = 90;
                if (e.ChangedButton == MouseButton.Right) angle = -90;
                Matrix O = new Matrix(Constants.operatorTypes[operatorType], angle);
                model.ApplyOperator(O);
                Logs.Write("Model was rotated on "+angle+" degrees");
                Update();
            }
            e.Handled = true;
        }

        private void scene_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (EditItem.IsChecked)
            {
                if (e.Delta > 0)
                {
                    operatorType = (operatorType + 1) % 2;
                }
                else
                {
                    if (operatorType == 0) operatorType = 1;
                    else operatorType = (operatorType - 1) % 2;
                }
                if(operatorType == 1)
                {
                    scene.Cursor = System.Windows.Input.Cursors.SizeWE;
                    Logs.Write("Rotate Axis was changed to X");
                }
                else
                {
                    scene.Cursor = System.Windows.Input.Cursors.SizeNS;
                    Logs.Write("Rotate Axis was changed to Y");
                }
            }
            e.Handled = true;
        }

        private void scene_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == Key.E)
            {
                EditItem_Click(sender, e);
            }
        }

        private void LowAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsListener.Get().accuracy != ACCURACY.LOW)
            {
                SettingsListener.Get().accuracy = ACCURACY.LOW;
                LowAccuracy.IsChecked = true;
                MediumAccuracy.IsChecked = false;
                HighAccuracy.IsChecked = false;
            }
        }

        private void MediumAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsListener.Get().accuracy != ACCURACY.MEDIUM)
            {
                SettingsListener.Get().accuracy = ACCURACY.MEDIUM;
                LowAccuracy.IsChecked = false;
                MediumAccuracy.IsChecked = true;
                HighAccuracy.IsChecked = false;
            }
        }

        private void HighAccuracy_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsListener.Get().accuracy != ACCURACY.HIGH)
            {
                SettingsListener.Get().accuracy = ACCURACY.HIGH;
                LowAccuracy.IsChecked = false;
                MediumAccuracy.IsChecked = false;
                HighAccuracy.IsChecked = true;
            }
        }
    }
}
