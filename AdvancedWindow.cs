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
            t9.Text = SettingsListener.Get().siftSigmaMin.ToString();
            t10.Text = SettingsListener.Get().siftSigmaMax.ToString();
            t11.Text = SettingsListener.Get().siftSigmaStep.ToString();
            t12.Text = SettingsListener.Get().siftScalesCount.ToString();
            t13.Checked = SettingsListener.Get().applyMedianFilterOnLoad;
            t14.Text = SettingsListener.Get().medianFilterSize.ToString();
            t15.Checked = SettingsListener.Get().applyHessianOperator;
            t16.Text = SettingsListener.Get().siftHessianR.ToString();
            t17.Text = SettingsListener.Get().framesStep.ToString();
            t18.Items.Add(Lang.GUI_SIFT_SOBEL);
            t18.Items.Add(Lang.GUI_SIFT_SHAR);
            if(SettingsListener.Get().bordersOperator.first == OperatorType.SOBEL_X)
            {
                t18.SelectedIndex = 0;
            }
            else if(SettingsListener.Get().bordersOperator.first == OperatorType.SHAR_X)
            {
                t18.SelectedIndex = 1;
            }
            t19.Text = SettingsListener.Get().imageWidth.ToString();
            t20.Items.Add(Lang.GUI_KEYPOINTS_ALG_ORB);
            t20.Items.Add(Lang.GUI_KEYPOINTS_ALG_SIFT);
            t20.Items.Add(Lang.GUI_KEYPOINTS_ALG_FAST);
            t20.SelectedIndex = (int)SettingsListener.Get().cAlg;
            t21.Text = SettingsListener.Get().fastRadius.ToString();
            t22.Text = SettingsListener.Get().fastT.ToString();
            t23.Text = SettingsListener.Get().fastN.ToString();

            Text = Lang.GUI_SETTINGS;
            l1.Text = Lang.GUI_GRID_WIDTH;
            l2.Text = Lang.GUI_GRID_LENGTH;
            l3.Text = Lang.GUI_GRID_THICKNESS;
            l4.Text = Lang.GUI_DEFAULT_COLOR;
            l5.Text = Lang.GUI_MAX_MODEL_WIDTH;
            l6.Text = Lang.GUI_CORRESPONDENCES_CIRCLE_WIDTH;
            l7.Text = Lang.GUI_SKIP_FRAMES_COUNT;
            l8.Text = Lang.GUI_ROTATE;
            l9.Text = Lang.GUI_SIFT_SIGMA_MIN;
            l10.Text = Lang.GUI_SIFT_SIGMA_MAX;
            l11.Text = Lang.GUI_SIFT_SIGMA_STEP;
            l12.Text = Lang.GUI_SIFT_SCALES_COUNT;
            l13.Text = Lang.GUI_SIFT_MEDIAN_FILTER;
            l14.Text = Lang.GUI_SIFT_MEDIAN_FILTER_SIZE;
            l15.Text = Lang.GUI_SIFT_HESSIAN_OPERATOR;
            l16.Text = Lang.GUI_SIFT_HESSIAN_R;
            l17.Text = Lang.GUI_SIFT_FRAME_STEP;
            l18.Text = Lang.GUI_SIFT_BORDERS_OPERATOR;
            l19.Text = Lang.GUI_SIFT_IMAGE_WIDTH;
            l20.Text = Lang.GUI_KEYPOINTS_ALG;
            l21.Text = Lang.GUI_FAST_RADIUS;
            l22.Text = Lang.GUI_FAST_T_VALUE;
            l23.Text = Lang.GUI_FAST_N_VALUE;
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

        private void t9_TextChanged(object sender, EventArgs e)
        {
            if (t9.Text.Length > 0)
            {
                SettingsListener.Get().siftSigmaMin = Double.Parse(t9.Text);
            }
        }

        private void t9_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                !(e.KeyChar == '.' && !t9.Text.Contains(".") && t9.Text.Length > 0);
        }

        private void t10_TextChanged(object sender, EventArgs e)
        {
            if (t10.Text.Length > 0)
            {
                SettingsListener.Get().siftSigmaMax = Double.Parse(t10.Text);
            }
        }

        private void t10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                !(e.KeyChar == '.' && !t10.Text.Contains(".") && t10.Text.Length > 0);
        }

        private void t11_TextChanged(object sender, EventArgs e)
        {
            if (t11.Text.Length > 0)
            {
                SettingsListener.Get().siftSigmaStep = Double.Parse(t11.Text);
            }
        }

        private void t11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                !(e.KeyChar == '.' && !t11.Text.Contains(".") && t11.Text.Length > 0);
        }

        private void t12_TextChanged(object sender, EventArgs e)
        {
            if (t12.Text.Length > 0)
            {
                SettingsListener.Get().siftScalesCount = Int32.Parse(t12.Text);
            }
        }

        private void t12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t13_CheckedChanged(object sender, EventArgs e)
        {
            SettingsListener.Get().applyMedianFilterOnLoad = t13.Checked;
        }

        private void t14_TextChanged(object sender, EventArgs e)
        {
            if (t14.Text.Length > 0)
            {
                SettingsListener.Get().medianFilterSize = Int32.Parse(t14.Text);
            }
        }

        private void t14_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t15_CheckedChanged(object sender, EventArgs e)
        {
            SettingsListener.Get().applyHessianOperator = t15.Checked;
        }

        private void t16_TextChanged(object sender, EventArgs e)
        {
            if (t16.Text.Length > 0)
            {
                SettingsListener.Get().siftHessianR = Double.Parse(t16.Text);
            }
        }

        private void t16_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) &&
                !(e.KeyChar == '.' && !t16.Text.Contains(".") && t16.Text.Length > 0);
        }

        private void t17_TextChanged(object sender, EventArgs e)
        {
            if (t17.Text.Length > 0)
            {
                SettingsListener.Get().framesStep = Int32.Parse(t17.Text);
            }
        }

        private void t17_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(t18.SelectedIndex == 0)
            {
                SettingsListener.Get().bordersOperator = new Pair<OperatorType, OperatorType>(OperatorType.SOBEL_X, OperatorType.SOBEL_Y);
            }
            else if(t18.SelectedIndex == 1)
            {
                SettingsListener.Get().bordersOperator = new Pair<OperatorType, OperatorType>(OperatorType.SHAR_X, OperatorType.SHAR_Y);
            }
        }

        private void t19_TextChanged(object sender, EventArgs e)
        {
            if (t19.Text.Length > 0)
            {
                SettingsListener.Get().imageWidth = Int32.Parse(t19.Text);
            }

        }

        private void t19_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t20_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsListener.Get().cAlg = (CorrespondencesAlg)t20.SelectedIndex;
        }

        private void t21_TextChanged(object sender, EventArgs e)
        {
            if (t21.Text.Length > 0)
            {
                SettingsListener.Get().fastRadius = Int32.Parse(t21.Text);
            }
        }

        private void t21_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t22_TextChanged(object sender, EventArgs e)
        {
            if (t22.Text.Length > 0)
            {
                SettingsListener.Get().fastT = Int32.Parse(t22.Text);
            }
        }

        private void t22_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void t23_TextChanged(object sender, EventArgs e)
        {
            if (t23.Text.Length > 0)
            {
                SettingsListener.Get().fastN = Int32.Parse(t23.Text);
            }
        }

        private void t23_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
