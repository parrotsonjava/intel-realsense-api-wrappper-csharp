namespace RealSenseExample
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
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.pictureBoxHand = new System.Windows.Forms.PictureBox();
            this.buttonRegisterFaces = new System.Windows.Forms.Button();
            this.buttonUnregisterFaces = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHand)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(21, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(233, 69);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(260, 12);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(233, 69);
            this.buttonStop.TabIndex = 2;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // pictureBoxHand
            // 
            this.pictureBoxHand.Location = new System.Drawing.Point(21, 87);
            this.pictureBoxHand.Name = "pictureBoxHand";
            this.pictureBoxHand.Size = new System.Drawing.Size(472, 356);
            this.pictureBoxHand.TabIndex = 2;
            this.pictureBoxHand.TabStop = false;
            // 
            // buttonRegisterFaces
            // 
            this.buttonRegisterFaces.Enabled = false;
            this.buttonRegisterFaces.Location = new System.Drawing.Point(499, 87);
            this.buttonRegisterFaces.Name = "buttonRegisterFaces";
            this.buttonRegisterFaces.Size = new System.Drawing.Size(161, 37);
            this.buttonRegisterFaces.TabIndex = 3;
            this.buttonRegisterFaces.Text = "Register Faces";
            this.buttonRegisterFaces.UseVisualStyleBackColor = true;
            this.buttonRegisterFaces.Click += new System.EventHandler(this.buttonRegisterFaces_Click);
            // 
            // buttonUnregisterFaces
            // 
            this.buttonUnregisterFaces.Enabled = false;
            this.buttonUnregisterFaces.Location = new System.Drawing.Point(499, 130);
            this.buttonUnregisterFaces.Name = "buttonUnregisterFaces";
            this.buttonUnregisterFaces.Size = new System.Drawing.Size(161, 37);
            this.buttonUnregisterFaces.TabIndex = 4;
            this.buttonUnregisterFaces.Text = "Unregister Faces";
            this.buttonUnregisterFaces.UseVisualStyleBackColor = true;
            this.buttonUnregisterFaces.Click += new System.EventHandler(this.buttonUnregisterFaces_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 452);
            this.Controls.Add(this.buttonUnregisterFaces);
            this.Controls.Add(this.buttonRegisterFaces);
            this.Controls.Add(this.pictureBoxHand);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Name = "MainForm";
            this.Text = "HandsDetection";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.PictureBox pictureBoxHand;
        private System.Windows.Forms.Button buttonRegisterFaces;
        private System.Windows.Forms.Button buttonUnregisterFaces;
    }
}

