namespace StereoStructure
{
    partial class AdvancedWindow
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.l1 = new System.Windows.Forms.Label();
            this.t1 = new System.Windows.Forms.TextBox();
            this.l2 = new System.Windows.Forms.Label();
            this.l3 = new System.Windows.Forms.Label();
            this.t2 = new System.Windows.Forms.TextBox();
            this.t3 = new System.Windows.Forms.TextBox();
            this.l4 = new System.Windows.Forms.Label();
            this.t4 = new System.Windows.Forms.TextBox();
            this.l5 = new System.Windows.Forms.Label();
            this.t5 = new System.Windows.Forms.TextBox();
            this.l6 = new System.Windows.Forms.Label();
            this.t6 = new System.Windows.Forms.TextBox();
            this.l7 = new System.Windows.Forms.Label();
            this.t7 = new System.Windows.Forms.TextBox();
            this.l8 = new System.Windows.Forms.Label();
            this.t8 = new System.Windows.Forms.TextBox();
            this.panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel.Controls.Add(this.tableLayoutPanel1);
            this.panel.Location = new System.Drawing.Point(12, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(476, 454);
            this.panel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.Controls.Add(this.t7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.t6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.t5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.t4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.t3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.t2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.t1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.l1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.l2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.l3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.l4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.l5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.l6, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.l7, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.l8, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.t8, 0, 7);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(468, 507);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // l1
            // 
            this.l1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l1.AutoSize = true;
            this.l1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l1.Location = new System.Drawing.Point(265, 22);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(78, 18);
            this.l1.TabIndex = 0;
            this.l1.Text = "Grid Width";
            this.l1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t1
            // 
            this.t1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t1.Location = new System.Drawing.Point(20, 20);
            this.t1.MaxLength = 4;
            this.t1.Name = "t1";
            this.t1.Size = new System.Drawing.Size(100, 22);
            this.t1.TabIndex = 1;
            this.t1.TabStop = false;
            this.t1.Text = "500";
            this.t1.TextChanged += new System.EventHandler(this.t1_TextChanged);
            this.t1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t1_KeyPress);
            // 
            // l2
            // 
            this.l2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l2.AutoSize = true;
            this.l2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l2.Location = new System.Drawing.Point(262, 83);
            this.l2.Name = "l2";
            this.l2.Size = new System.Drawing.Size(84, 18);
            this.l2.TabIndex = 2;
            this.l2.Text = "Grid Length";
            this.l2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // l3
            // 
            this.l3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l3.AutoSize = true;
            this.l3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l3.Location = new System.Drawing.Point(250, 144);
            this.l3.Name = "l3";
            this.l3.Size = new System.Drawing.Size(108, 18);
            this.l3.TabIndex = 3;
            this.l3.Text = "Grid Thickness";
            this.l3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t2
            // 
            this.t2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t2.Location = new System.Drawing.Point(20, 81);
            this.t2.MaxLength = 4;
            this.t2.Name = "t2";
            this.t2.Size = new System.Drawing.Size(100, 22);
            this.t2.TabIndex = 4;
            this.t2.TabStop = false;
            this.t2.Text = "500";
            this.t2.TextChanged += new System.EventHandler(this.t2_TextChanged);
            this.t2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t2_KeyPress);
            // 
            // t3
            // 
            this.t3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t3.Location = new System.Drawing.Point(20, 142);
            this.t3.MaxLength = 4;
            this.t3.Name = "t3";
            this.t3.Size = new System.Drawing.Size(100, 22);
            this.t3.TabIndex = 5;
            this.t3.TabStop = false;
            this.t3.Text = "0.3";
            this.t3.TextChanged += new System.EventHandler(this.t3_TextChanged);
            this.t3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t3_KeyPress);
            // 
            // l4
            // 
            this.l4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l4.AutoSize = true;
            this.l4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l4.Location = new System.Drawing.Point(195, 205);
            this.l4.Name = "l4";
            this.l4.Size = new System.Drawing.Size(218, 18);
            this.l4.TabIndex = 6;
            this.l4.Text = "Default model color on first load";
            this.l4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t4
            // 
            this.t4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t4.Location = new System.Drawing.Point(20, 203);
            this.t4.MaxLength = 11;
            this.t4.Name = "t4";
            this.t4.Size = new System.Drawing.Size(100, 22);
            this.t4.TabIndex = 7;
            this.t4.TabStop = false;
            this.t4.Text = "128,128,128";
            this.t4.TextChanged += new System.EventHandler(this.t4_TextChanged);
            this.t4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t4_KeyPress);
            // 
            // l5
            // 
            this.l5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l5.AutoSize = true;
            this.l5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l5.Location = new System.Drawing.Point(226, 266);
            this.l5.Name = "l5";
            this.l5.Size = new System.Drawing.Size(156, 18);
            this.l5.TabIndex = 8;
            this.l5.Text = "Maximum model width";
            this.l5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t5
            // 
            this.t5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t5.Location = new System.Drawing.Point(20, 264);
            this.t5.MaxLength = 3;
            this.t5.Name = "t5";
            this.t5.Size = new System.Drawing.Size(100, 22);
            this.t5.TabIndex = 9;
            this.t5.TabStop = false;
            this.t5.Text = "50";
            this.t5.TextChanged += new System.EventHandler(this.t5_TextChanged);
            this.t5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t5_KeyPress);
            // 
            // l6
            // 
            this.l6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l6.AutoSize = true;
            this.l6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l6.Location = new System.Drawing.Point(202, 327);
            this.l6.Name = "l6";
            this.l6.Size = new System.Drawing.Size(204, 18);
            this.l6.TabIndex = 10;
            this.l6.Text = "Correspondences circle width";
            this.l6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t6
            // 
            this.t6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t6.Location = new System.Drawing.Point(20, 325);
            this.t6.MaxLength = 2;
            this.t6.Name = "t6";
            this.t6.Size = new System.Drawing.Size(100, 22);
            this.t6.TabIndex = 11;
            this.t6.TabStop = false;
            this.t6.Text = "10";
            this.t6.TextChanged += new System.EventHandler(this.t6_TextChanged);
            this.t6.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t6_KeyPress);
            // 
            // l7
            // 
            this.l7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l7.AutoSize = true;
            this.l7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l7.Location = new System.Drawing.Point(275, 388);
            this.l7.Name = "l7";
            this.l7.Size = new System.Drawing.Size(57, 18);
            this.l7.TabIndex = 12;
            this.l7.Text = "Epsilon";
            this.l7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t7
            // 
            this.t7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t7.Location = new System.Drawing.Point(20, 386);
            this.t7.MaxLength = 10;
            this.t7.Name = "t7";
            this.t7.Size = new System.Drawing.Size(100, 22);
            this.t7.TabIndex = 13;
            this.t7.TabStop = false;
            this.t7.Text = "0.0000001";
            this.t7.TextChanged += new System.EventHandler(this.t7_TextChanged);
            this.t7.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t7_KeyPress);
            // 
            // l8
            // 
            this.l8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.l8.AutoSize = true;
            this.l8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.l8.Location = new System.Drawing.Point(221, 458);
            this.l8.Name = "l8";
            this.l8.Size = new System.Drawing.Size(165, 18);
            this.l8.TabIndex = 14;
            this.l8.Text = "Gauss-Blur sigma value";
            this.l8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // t8
            // 
            this.t8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.t8.Location = new System.Drawing.Point(20, 456);
            this.t8.MaxLength = 6;
            this.t8.Name = "t8";
            this.t8.Size = new System.Drawing.Size(100, 22);
            this.t8.TabIndex = 15;
            this.t8.TabStop = false;
            this.t8.Text = "50.0";
            this.t8.TextChanged += new System.EventHandler(this.t8_TextChanged);
            this.t8.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.t8_KeyPress);
            // 
            // AdvancedWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 476);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "AdvancedWindow";
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdvancedWindow_FormClosed);
            this.panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox t1;
        private System.Windows.Forms.Label l1;
        private System.Windows.Forms.TextBox t2;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.Label l3;
        private System.Windows.Forms.TextBox t3;
        private System.Windows.Forms.TextBox t4;
        private System.Windows.Forms.Label l4;
        private System.Windows.Forms.TextBox t5;
        private System.Windows.Forms.Label l5;
        private System.Windows.Forms.TextBox t6;
        private System.Windows.Forms.Label l6;
        private System.Windows.Forms.TextBox t7;
        private System.Windows.Forms.Label l7;
        private System.Windows.Forms.Label l8;
        private System.Windows.Forms.TextBox t8;
    }
}