namespace PlaytechUI
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
                StopService(gameServiceController);
                StopService(learningServiceController);


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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.scriptsFolderInput = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stopRButton = new System.Windows.Forms.RadioButton();
            this.gameRButton = new System.Windows.Forms.RadioButton();
            this.learnRButton = new System.Windows.Forms.RadioButton();
            this.gameServiceController = new System.ServiceProcess.ServiceController();
            this.learningServiceController = new System.ServiceProcess.ServiceController();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.scriptsFolderInput);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 52);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scripts folder";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(294, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // scriptsFolderInput
            // 
            this.scriptsFolderInput.Location = new System.Drawing.Point(6, 19);
            this.scriptsFolderInput.Name = "scriptsFolderInput";
            this.scriptsFolderInput.Size = new System.Drawing.Size(282, 20);
            this.scriptsFolderInput.TabIndex = 0;
            this.scriptsFolderInput.Text = "C:\\cvd\\";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.stopRButton);
            this.groupBox2.Controls.Add(this.gameRButton);
            this.groupBox2.Controls.Add(this.learnRButton);
            this.groupBox2.Location = new System.Drawing.Point(12, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(360, 51);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mode";
            // 
            // stopRButton
            // 
            this.stopRButton.AutoSize = true;
            this.stopRButton.Location = new System.Drawing.Point(283, 19);
            this.stopRButton.Name = "stopRButton";
            this.stopRButton.Size = new System.Drawing.Size(47, 17);
            this.stopRButton.TabIndex = 2;
            this.stopRButton.TabStop = true;
            this.stopRButton.Text = "Stop";
            this.stopRButton.UseVisualStyleBackColor = true;
            this.stopRButton.CheckedChanged += new System.EventHandler(this.stopRButton_CheckedChanged);
            // 
            // gameRButton
            // 
            this.gameRButton.AutoSize = true;
            this.gameRButton.Location = new System.Drawing.Point(30, 19);
            this.gameRButton.Name = "gameRButton";
            this.gameRButton.Size = new System.Drawing.Size(53, 17);
            this.gameRButton.TabIndex = 1;
            this.gameRButton.TabStop = true;
            this.gameRButton.Text = "Game";
            this.gameRButton.UseVisualStyleBackColor = true;
            this.gameRButton.CheckedChanged += new System.EventHandler(this.gameRButton_CheckedChanged);
            // 
            // learnRButton
            // 
            this.learnRButton.AutoSize = true;
            this.learnRButton.Location = new System.Drawing.Point(157, 19);
            this.learnRButton.Name = "learnRButton";
            this.learnRButton.Size = new System.Drawing.Size(66, 17);
            this.learnRButton.TabIndex = 0;
            this.learnRButton.TabStop = true;
            this.learnRButton.Text = "Learning";
            this.learnRButton.UseVisualStyleBackColor = true;
            this.learnRButton.CheckedChanged += new System.EventHandler(this.learnRButton_CheckedChanged);
            // 
            // gameServiceController
            // 
            this.gameServiceController.ServiceName = "GameService";
            // 
            // learningServiceController
            // 
            this.learningServiceController.ServiceName = "LearningService";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 129);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(384, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 2;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 384;
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 151);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 190);
            this.MinimumSize = new System.Drawing.Size(400, 190);
            this.Name = "MainForm";
            this.Text = "Cards Vision Detection";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox scriptsFolderInput;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton gameRButton;
        private System.Windows.Forms.RadioButton learnRButton;
        private System.ServiceProcess.ServiceController gameServiceController;
        private System.ServiceProcess.ServiceController learningServiceController;
        private System.Windows.Forms.RadioButton stopRButton;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.Timer timer;
    }
}

