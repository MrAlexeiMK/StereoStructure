using System;
using System.Windows;

namespace StereoStructure
{
    public partial class LogsWindow : Window
    {
        private MainWindow main;
        public LogsWindow(MainWindow main)
        {
            this.main = main;
            Title = Lang.GUI_LOGS;
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if(main.IsLoaded && main.LogsItem.IsChecked)
            {
                main.Close();
            }
        }
    }
}
