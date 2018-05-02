namespace BS
{
    partial class DisplayPic
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
            this.picBox_showPic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_showPic)).BeginInit();
            this.SuspendLayout();
            // 
            // picBox_showPic
            // 
            this.picBox_showPic.Location = new System.Drawing.Point(-4, 1);
            this.picBox_showPic.Name = "picBox_showPic";
            this.picBox_showPic.Size = new System.Drawing.Size(288, 261);
            this.picBox_showPic.TabIndex = 0;
            this.picBox_showPic.TabStop = false;
            // 
            // DisplayPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.picBox_showPic);
            this.Name = "DisplayPic";
            this.Text = "DisplayPic";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DisplayPic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_showPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBox_showPic;
    }
}