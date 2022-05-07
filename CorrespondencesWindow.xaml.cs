using System;
using System.Windows;

namespace StereoStructure
{
    public partial class CorrespondencesWindow : Window
    {
        private int index;
        public CorrespondencesWindow()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {
            Title = Lang.GUI_CORRESPONDENCES;
            index = 1;
            groupBox.Header = index + "/" + Correspondences.GetFramesCount();
            if (Correspondences.GetFramesCount() != 0)
            {
                left.Source = Correspondences.GetLeft(index);
                right.Source = Correspondences.GetRight(index);
            }
        }

        private void ToLeft()
        {
            --index;
            left.Source = Correspondences.GetLeft(index);
            right.Source = Correspondences.GetRight(index);
            groupBox.Header = index + "/" + Correspondences.GetFramesCount();
        }

        private void ToRight()
        {
            ++index;
            left.Source = Correspondences.GetLeft(index);
            right.Source = Correspondences.GetRight(index);
            groupBox.Header = index + "/" + Correspondences.GetFramesCount();
        }

        private void left_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(index > 1)
            {
                ToLeft();
            }
        }

        private void right_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(index < Correspondences.GetFramesCount())
            {
                ToRight();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Logs.Write("Correspondences Window was closed");
        }
    }
}
