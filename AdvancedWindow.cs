using System;
using System.Windows.Forms;

namespace StereoStructure
{
    public partial class AdvancedWindow : Form
    {
        private MainWindow main;
        public AdvancedWindow(MainWindow main)
        {
            this.main = main;
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            t1.Text = SettingsListener.Get().gridWidth.ToString();
            t2.Text = SettingsListener.Get().gridLength.ToString();
            t3.Text = SettingsListener.Get().gridThickness.ToString();
            t4.Text = SettingsListener.Get().defaultColor.ToString();
            t5.Text = SettingsListener.Get().maxWidth.ToString();
            t6.Text = SettingsListener.Get().circleWidth.ToString();
            t7.Text = SettingsListener.Get().skipFrames.ToString();
            t8.Items.Add(Lang.GUI_ROTATE_NONE);
            t8.Items.Add(Lang.GUI_ROTATE_90);
            t8.Items.Add(Lang.GUI_ROTATE_180);
            t8.Items.Add(Lang.GUI_ROTATE_270);
            t8.SelectedIndex = (int)SettingsListener.Get().rotationImages;

            Text = Lang.GUI_SETTINGS;
            l1.Text = Lang.GUI_GRID_WIDTH;
            l2.Text = Lang.GUI_GRID_LENGTH;
            l3.Text = Lang.GUI_GRID_THICKNESS;
            l4.Text = Lang.GUI_DEFAULT_COLOR;
            l5.Text = Lang.GUI_MAX_MODEL_WIDTH;
            l6.Text = Lang.GUI_CORRESPONDENCES_CIRCLE_WIDTH;
            l7.Text = Lang.GUI_SKIP_FRAMES_COUNT;
            l8.Text = Lang.GUI_ROTATE;
        }

        private void AdvancedWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            AdvancedListener.SetAdvancedWindowOpened(false);
        }

        private void t1_TextChanged(object sender, System.EventArgs e)
        {
            if (t1.Text.Length > 0)
            {
                SettingsListener.Get().gridWidth = Int32.Parse(t1.Text);
                main.Grid.Width = SettingsListener.Get().gridWidth;
            }
        }

        private void t1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t2_TextChanged(object sender, EventArgs e)
        {
            if (t2.Text.Length > 0)
            {
                SettingsListener.Get().gridLength = Int32.Parse(t2.Text);
                main.Grid.Length = SettingsListener.Get().gridLength;
            }
        }

        private void t2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t3_TextChanged(object sender, EventArgs e)
        {
            if (t3.Text.Length > 0)
            {
                SettingsListener.Get().gridThickness = Double.Parse(t3.Text);
                main.Grid.Thickness = SettingsListener.Get().gridThickness;
            }
        }

        private void t3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && 
                !(e.KeyChar == '.' && !t3.Text.Contains(".") && t3.Text.Length > 0);
        }

        private void t4_TextChanged(object sender, EventArgs e)
        {
            if (t4.Text.Length > 0)
            {
                string[] spl = t4.Text.Split(',');
                if (spl.Length == 3 && spl[2] != "")
                {
                    Color color = new Color(128, 128, 128);
                    color.R = Byte.Parse(spl[0]);
                    color.G = Byte.Parse(spl[1]);
                    color.B = Byte.Parse(spl[2]);
                    SettingsListener.Get().defaultColor = color;
                }
            }
        }

        private void t4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string[] spl = t4.Text.Split(',');
            char last = '0';
            string color = "";
            if (t4.Text.Length > 0)
            {
                last = t4.Text[t4.Text.Length - 1];
                color = spl[spl.Length - 1];
            }
            e.Handled = !(char.IsDigit(e.KeyChar) && Int32.Parse(color+e.KeyChar) < 256) && !char.IsControl(e.KeyChar) &&
                !(e.KeyChar == ',' && spl.Length < 3 && last != ',');
        }

        private void t5_TextChanged(object sender, EventArgs e)
        {
            if (t5.Text.Length > 0)
            {
                SettingsListener.Get().maxWidth = Int32.Parse(t5.Text);
            }
        }

        private void t5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t6_TextChanged(object sender, EventArgs e)
        {
            if (t6.Text.Length > 0)
            {
                SettingsListener.Get().circleWidth = Int32.Parse(t6.Text);
            }
        }

        private void t6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t7_TextChanged(object sender, EventArgs e)
        {
            if (t7.Text.Length > 0)
            {
                SettingsListener.Get().skipFrames = Int32.Parse(t7.Text);
            }
        }

        private void t7_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t8_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsListener.Get().rotationImages = (Rotation)t8.SelectedIndex;
        }
    }
}
