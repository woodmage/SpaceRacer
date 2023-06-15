namespace SpaceRacer
{
    partial class SpaceRacer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpaceRacer));
            arenaPB = new PictureBox();
            infoPB = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)arenaPB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)infoPB).BeginInit();
            SuspendLayout();
            // 
            // arenaPB
            // 
            arenaPB.BackgroundImage = Properties.Resources.space;
            arenaPB.Location = new Point(0, 0);
            arenaPB.Margin = new Padding(0);
            arenaPB.Name = "arenaPB";
            arenaPB.Size = new Size(600, 800);
            arenaPB.TabIndex = 0;
            arenaPB.TabStop = false;
            arenaPB.MouseLeave += FocusLost;
            arenaPB.MouseMove += HandleMouse;
            // 
            // infoPB
            // 
            infoPB.Location = new Point(600, 0);
            infoPB.Margin = new Padding(0);
            infoPB.Name = "infoPB";
            infoPB.Size = new Size(400, 800);
            infoPB.TabIndex = 1;
            infoPB.TabStop = false;
            // 
            // SpaceRacer
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 801);
            Controls.Add(infoPB);
            Controls.Add(arenaPB);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SpaceRacer";
            Text = "Space Racer";
            Load += HandleLoad;
            KeyDown += HandleKeyDown;
            KeyUp += HandleKeyUp;
            Leave += FocusLost;
            Resize += HandleResize;
            ((System.ComponentModel.ISupportInitialize)arenaPB).EndInit();
            ((System.ComponentModel.ISupportInitialize)infoPB).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox arenaPB;
        private PictureBox infoPB;
    }
}