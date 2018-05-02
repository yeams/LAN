namespace BS
{
    partial class SendPic
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
            this.bt_SendPic = new System.Windows.Forms.Button();
            this.bt_CancelPic = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_showPic)).BeginInit();
            this.SuspendLayout();
            // 
            // picBox_showPic
            // 
            this.picBox_showPic.Location = new System.Drawing.Point(0, 0);
            this.picBox_showPic.Name = "picBox_showPic";
            this.picBox_showPic.Size = new System.Drawing.Size(392, 260);
            this.picBox_showPic.TabIndex = 0;
            this.picBox_showPic.TabStop = false;
            this.picBox_showPic.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBox_showPic_MouseMove);
            // 
            // bt_SendPic
            // 
            this.bt_SendPic.Location = new System.Drawing.Point(76, 266);
            this.bt_SendPic.Name = "bt_SendPic";
            this.bt_SendPic.Size = new System.Drawing.Size(75, 23);
            this.bt_SendPic.TabIndex = 1;
            this.bt_SendPic.Text = "发送";
            this.bt_SendPic.UseVisualStyleBackColor = true;
            this.bt_SendPic.Click += new System.EventHandler(this.bt_SendPic_Click);
            // 
            // bt_CancelPic
            // 
            this.bt_CancelPic.Location = new System.Drawing.Point(199, 266);
            this.bt_CancelPic.Name = "bt_CancelPic";
            this.bt_CancelPic.Size = new System.Drawing.Size(75, 23);
            this.bt_CancelPic.TabIndex = 2;
            this.bt_CancelPic.Text = "取消";
            this.bt_CancelPic.UseVisualStyleBackColor = true;
            this.bt_CancelPic.Click += new System.EventHandler(this.bt_CancelPic_Click);
            // 
            // SendPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 294);
            this.Controls.Add(this.bt_CancelPic);
            this.Controls.Add(this.bt_SendPic);
            this.Controls.Add(this.picBox_showPic);
            this.Name = "SendPic";
            this.Text = "SendPic";
            this.Load += new System.EventHandler(this.SendPic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_showPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picBox_showPic;
        private System.Windows.Forms.Button bt_SendPic;
        private System.Windows.Forms.Button bt_CancelPic;
    }
}