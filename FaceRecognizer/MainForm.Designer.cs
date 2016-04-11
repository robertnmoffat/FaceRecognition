namespace FaceRecognizer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pb_main = new System.Windows.Forms.PictureBox();
            this.pb_lib = new System.Windows.Forms.PictureBox();
            this.sb_lib = new System.Windows.Forms.HScrollBar();
            this.bt_avg = new System.Windows.Forms.Button();
            this.rb_reg = new System.Windows.Forms.RadioButton();
            this.rb_dif = new System.Windows.Forms.RadioButton();
            this.rb_eig = new System.Windows.Forms.RadioButton();
            this.bt_recon = new System.Windows.Forms.Button();
            this.lb_person = new System.Windows.Forms.Label();
            this.lb_distance = new System.Windows.Forms.Label();
            this.bt_load = new System.Windows.Forms.Button();
            this.lb_faceSpace = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_main)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_lib)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_main
            // 
            this.pb_main.Location = new System.Drawing.Point(12, 12);
            this.pb_main.Name = "pb_main";
            this.pb_main.Size = new System.Drawing.Size(256, 256);
            this.pb_main.TabIndex = 0;
            this.pb_main.TabStop = false;
            // 
            // pb_lib
            // 
            this.pb_lib.Location = new System.Drawing.Point(280, 12);
            this.pb_lib.Name = "pb_lib";
            this.pb_lib.Size = new System.Drawing.Size(256, 256);
            this.pb_lib.TabIndex = 1;
            this.pb_lib.TabStop = false;
            // 
            // sb_lib
            // 
            this.sb_lib.LargeChange = 2;
            this.sb_lib.Location = new System.Drawing.Point(280, 271);
            this.sb_lib.Maximum = 1;
            this.sb_lib.Name = "sb_lib";
            this.sb_lib.Size = new System.Drawing.Size(256, 20);
            this.sb_lib.TabIndex = 2;
            this.sb_lib.Scroll += new System.Windows.Forms.ScrollEventHandler(this.sb_lib_Scroll);
            // 
            // bt_avg
            // 
            this.bt_avg.Location = new System.Drawing.Point(542, 85);
            this.bt_avg.Name = "bt_avg";
            this.bt_avg.Size = new System.Drawing.Size(75, 23);
            this.bt_avg.TabIndex = 3;
            this.bt_avg.Text = "AvgFace";
            this.bt_avg.UseVisualStyleBackColor = true;
            this.bt_avg.Click += new System.EventHandler(this.bt_avg_Click);
            // 
            // rb_reg
            // 
            this.rb_reg.AutoSize = true;
            this.rb_reg.Location = new System.Drawing.Point(542, 115);
            this.rb_reg.Name = "rb_reg";
            this.rb_reg.Size = new System.Drawing.Size(62, 17);
            this.rb_reg.TabIndex = 4;
            this.rb_reg.TabStop = true;
            this.rb_reg.Text = "Regular";
            this.rb_reg.UseVisualStyleBackColor = true;
            this.rb_reg.CheckedChanged += new System.EventHandler(this.rb_reg_CheckedChanged);
            // 
            // rb_dif
            // 
            this.rb_dif.AutoSize = true;
            this.rb_dif.Location = new System.Drawing.Point(542, 139);
            this.rb_dif.Name = "rb_dif";
            this.rb_dif.Size = new System.Drawing.Size(101, 17);
            this.rb_dif.TabIndex = 5;
            this.rb_dif.TabStop = true;
            this.rb_dif.Text = "Difference Face";
            this.rb_dif.UseVisualStyleBackColor = true;
            this.rb_dif.CheckedChanged += new System.EventHandler(this.rb_dif_CheckedChanged);
            // 
            // rb_eig
            // 
            this.rb_eig.AutoSize = true;
            this.rb_eig.Location = new System.Drawing.Point(542, 163);
            this.rb_eig.Name = "rb_eig";
            this.rb_eig.Size = new System.Drawing.Size(79, 17);
            this.rb_eig.TabIndex = 6;
            this.rb_eig.TabStop = true;
            this.rb_eig.Text = "Eigen Face";
            this.rb_eig.UseVisualStyleBackColor = true;
            this.rb_eig.CheckedChanged += new System.EventHandler(this.rb_eig_CheckedChanged);
            // 
            // bt_recon
            // 
            this.bt_recon.Location = new System.Drawing.Point(623, 85);
            this.bt_recon.Name = "bt_recon";
            this.bt_recon.Size = new System.Drawing.Size(75, 23);
            this.bt_recon.TabIndex = 7;
            this.bt_recon.Text = "Reconstruct";
            this.bt_recon.UseVisualStyleBackColor = true;
            this.bt_recon.Click += new System.EventHandler(this.bt_recon_Click);
            // 
            // lb_person
            // 
            this.lb_person.AutoSize = true;
            this.lb_person.Location = new System.Drawing.Point(542, 187);
            this.lb_person.Name = "lb_person";
            this.lb_person.Size = new System.Drawing.Size(49, 13);
            this.lb_person.TabIndex = 8;
            this.lb_person.Text = "Person : ";
            // 
            // lb_distance
            // 
            this.lb_distance.AutoSize = true;
            this.lb_distance.Location = new System.Drawing.Point(277, 302);
            this.lb_distance.Name = "lb_distance";
            this.lb_distance.Size = new System.Drawing.Size(58, 13);
            this.lb_distance.TabIndex = 9;
            this.lb_distance.Text = "Distance : ";
            // 
            // bt_load
            // 
            this.bt_load.Location = new System.Drawing.Point(545, 12);
            this.bt_load.Name = "bt_load";
            this.bt_load.Size = new System.Drawing.Size(75, 23);
            this.bt_load.TabIndex = 10;
            this.bt_load.Text = "Load";
            this.bt_load.UseVisualStyleBackColor = true;
            this.bt_load.Click += new System.EventHandler(this.bt_load_Click);
            // 
            // lb_faceSpace
            // 
            this.lb_faceSpace.AutoSize = true;
            this.lb_faceSpace.Location = new System.Drawing.Point(545, 42);
            this.lb_faceSpace.Name = "lb_faceSpace";
            this.lb_faceSpace.Size = new System.Drawing.Size(71, 13);
            this.lb_faceSpace.TabIndex = 11;
            this.lb_faceSpace.Text = "Face Space: ";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 324);
            this.Controls.Add(this.lb_faceSpace);
            this.Controls.Add(this.bt_load);
            this.Controls.Add(this.lb_distance);
            this.Controls.Add(this.lb_person);
            this.Controls.Add(this.bt_recon);
            this.Controls.Add(this.rb_eig);
            this.Controls.Add(this.rb_dif);
            this.Controls.Add(this.rb_reg);
            this.Controls.Add(this.bt_avg);
            this.Controls.Add(this.sb_lib);
            this.Controls.Add(this.pb_lib);
            this.Controls.Add(this.pb_main);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_main)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_lib)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_main;
        private System.Windows.Forms.PictureBox pb_lib;
        private System.Windows.Forms.HScrollBar sb_lib;
        private System.Windows.Forms.Button bt_avg;
        private System.Windows.Forms.RadioButton rb_reg;
        private System.Windows.Forms.RadioButton rb_dif;
        private System.Windows.Forms.RadioButton rb_eig;
        private System.Windows.Forms.Button bt_recon;
        private System.Windows.Forms.Label lb_person;
        private System.Windows.Forms.Label lb_distance;
        private System.Windows.Forms.Button bt_load;
        private System.Windows.Forms.Label lb_faceSpace;
    }
}

