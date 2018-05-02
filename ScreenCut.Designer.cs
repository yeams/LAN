namespace BS
{
    partial class ScreenCut
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
            this.SuspendLayout();
            // 
            // ScreenCut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "ScreenCut";
            this.Text = "ScreenCut";
            this.Load += new System.EventHandler(this.ScreenCut_Load);
            this.DoubleClick += new System.EventHandler(this.ScreenCut_DoubleClick);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ScreenCut_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScreenCut_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ScreenCut_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ScreenCut_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}