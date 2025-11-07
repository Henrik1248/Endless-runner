namespace NEA
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.coinCounterLabel = new System.Windows.Forms.Label();
            this.scoreLabel = new System.Windows.Forms.Label();
            this.ActivePowerUpsLabel = new System.Windows.Forms.Label();
            this.obstaclesPassedLabel = new System.Windows.Forms.Label();
            this.powerUpsPickedUpLabel = new System.Windows.Forms.Label();
            this.gameOverLabel = new System.Windows.Forms.Label();
            this.mainMenuButton = new System.Windows.Forms.Button();
            this.playAgainButton = new System.Windows.Forms.Button();
            this.viewStatisticsButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.endlessRunnerLabel = new System.Windows.Forms.Label();
            this.highScoreLabel = new System.Windows.Forms.Label();
            this.playButton = new System.Windows.Forms.Button();
            this.quitButton = new System.Windows.Forms.Button();
            this.playerDistanceFromChaserLabel = new System.Windows.Forms.Label();
            this.saveMapButton = new System.Windows.Forms.Button();
            this.selectSlotLabel = new System.Windows.Forms.Label();
            this.slot1Button = new System.Windows.Forms.Button();
            this.slot3Button = new System.Windows.Forms.Button();
            this.slot2Button = new System.Windows.Forms.Button();
            this.loadMapButton = new System.Windows.Forms.Button();
            this.mapInfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 25;
            this.gameTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // coinCounterLabel
            // 
            this.coinCounterLabel.AutoSize = true;
            this.coinCounterLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.coinCounterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coinCounterLabel.Location = new System.Drawing.Point(500, 155);
            this.coinCounterLabel.Name = "coinCounterLabel";
            this.coinCounterLabel.Size = new System.Drawing.Size(200, 55);
            this.coinCounterLabel.TabIndex = 3;
            this.coinCounterLabel.Text = "Coins: 0";
            this.coinCounterLabel.Click += new System.EventHandler(this.coinCounterLabel_Click);
            // 
            // scoreLabel
            // 
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scoreLabel.Location = new System.Drawing.Point(500, 88);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(203, 55);
            this.scoreLabel.TabIndex = 4;
            this.scoreLabel.Text = "Score: 0";
            this.scoreLabel.Click += new System.EventHandler(this.scoreLabel_Click);
            // 
            // ActivePowerUpsLabel
            // 
            this.ActivePowerUpsLabel.AutoSize = true;
            this.ActivePowerUpsLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.ActivePowerUpsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActivePowerUpsLabel.Location = new System.Drawing.Point(1632, 88);
            this.ActivePowerUpsLabel.Name = "ActivePowerUpsLabel";
            this.ActivePowerUpsLabel.Size = new System.Drawing.Size(419, 55);
            this.ActivePowerUpsLabel.TabIndex = 5;
            this.ActivePowerUpsLabel.Text = "Active Power-Ups:";
            this.ActivePowerUpsLabel.Click += new System.EventHandler(this.ActivePowerUpsLabel_Click);
            // 
            // obstaclesPassedLabel
            // 
            this.obstaclesPassedLabel.AutoSize = true;
            this.obstaclesPassedLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.obstaclesPassedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.obstaclesPassedLabel.Location = new System.Drawing.Point(500, 358);
            this.obstaclesPassedLabel.Name = "obstaclesPassedLabel";
            this.obstaclesPassedLabel.Size = new System.Drawing.Size(464, 55);
            this.obstaclesPassedLabel.TabIndex = 6;
            this.obstaclesPassedLabel.Text = "Obstacles Passed: 0";
            this.obstaclesPassedLabel.Click += new System.EventHandler(this.obstaclesPassedLabel_Click);
            // 
            // powerUpsPickedUpLabel
            // 
            this.powerUpsPickedUpLabel.AutoSize = true;
            this.powerUpsPickedUpLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.powerUpsPickedUpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerUpsPickedUpLabel.Location = new System.Drawing.Point(500, 291);
            this.powerUpsPickedUpLabel.Name = "powerUpsPickedUpLabel";
            this.powerUpsPickedUpLabel.Size = new System.Drawing.Size(545, 55);
            this.powerUpsPickedUpLabel.TabIndex = 7;
            this.powerUpsPickedUpLabel.Text = "Power Ups Picked Up: 0";
            this.powerUpsPickedUpLabel.Click += new System.EventHandler(this.powerUpsPickedUpLabel_Click);
            // 
            // gameOverLabel
            // 
            this.gameOverLabel.AutoSize = true;
            this.gameOverLabel.BackColor = System.Drawing.Color.Maroon;
            this.gameOverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameOverLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.gameOverLabel.Location = new System.Drawing.Point(1168, 237);
            this.gameOverLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gameOverLabel.Name = "gameOverLabel";
            this.gameOverLabel.Size = new System.Drawing.Size(426, 82);
            this.gameOverLabel.TabIndex = 8;
            this.gameOverLabel.Text = "Game Over!";
            // 
            // mainMenuButton
            // 
            this.mainMenuButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.mainMenuButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainMenuButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.mainMenuButton.Location = new System.Drawing.Point(1168, 358);
            this.mainMenuButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.mainMenuButton.Name = "mainMenuButton";
            this.mainMenuButton.Size = new System.Drawing.Size(246, 94);
            this.mainMenuButton.TabIndex = 9;
            this.mainMenuButton.Text = "Main Menu";
            this.mainMenuButton.UseVisualStyleBackColor = false;
            this.mainMenuButton.Click += new System.EventHandler(this.mainMenuButton_Click);
            // 
            // playAgainButton
            // 
            this.playAgainButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.playAgainButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playAgainButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.playAgainButton.Location = new System.Drawing.Point(1168, 462);
            this.playAgainButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.playAgainButton.Name = "playAgainButton";
            this.playAgainButton.Size = new System.Drawing.Size(246, 94);
            this.playAgainButton.TabIndex = 10;
            this.playAgainButton.Text = "Play Again";
            this.playAgainButton.UseVisualStyleBackColor = false;
            this.playAgainButton.Click += new System.EventHandler(this.playAgainButton_Click);
            // 
            // viewStatisticsButton
            // 
            this.viewStatisticsButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.viewStatisticsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewStatisticsButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.viewStatisticsButton.Location = new System.Drawing.Point(1168, 566);
            this.viewStatisticsButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewStatisticsButton.Name = "viewStatisticsButton";
            this.viewStatisticsButton.Size = new System.Drawing.Size(246, 94);
            this.viewStatisticsButton.TabIndex = 11;
            this.viewStatisticsButton.Text = "View Statistics";
            this.viewStatisticsButton.UseVisualStyleBackColor = false;
            this.viewStatisticsButton.Click += new System.EventHandler(this.viewStatisticsButton_Click);
            // 
            // backButton
            // 
            this.backButton.BackColor = System.Drawing.Color.Navy;
            this.backButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.backButton.Location = new System.Drawing.Point(890, 358);
            this.backButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(246, 94);
            this.backButton.TabIndex = 12;
            this.backButton.Text = "Back";
            this.backButton.UseVisualStyleBackColor = false;
            this.backButton.Click += new System.EventHandler(this.backButton_Click);
            // 
            // endlessRunnerLabel
            // 
            this.endlessRunnerLabel.AutoSize = true;
            this.endlessRunnerLabel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.endlessRunnerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endlessRunnerLabel.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.endlessRunnerLabel.Location = new System.Drawing.Point(486, 222);
            this.endlessRunnerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.endlessRunnerLabel.Name = "endlessRunnerLabel";
            this.endlessRunnerLabel.Size = new System.Drawing.Size(724, 108);
            this.endlessRunnerLabel.TabIndex = 13;
            this.endlessRunnerLabel.Text = "Endless Runner";
            this.endlessRunnerLabel.Click += new System.EventHandler(this.endlessRunnerLabel_Click);
            // 
            // highScoreLabel
            // 
            this.highScoreLabel.AutoSize = true;
            this.highScoreLabel.BackColor = System.Drawing.Color.DarkCyan;
            this.highScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highScoreLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.highScoreLabel.Location = new System.Drawing.Point(696, 398);
            this.highScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.highScoreLabel.Name = "highScoreLabel";
            this.highScoreLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.highScoreLabel.Size = new System.Drawing.Size(315, 55);
            this.highScoreLabel.TabIndex = 14;
            this.highScoreLabel.Text = "High Score: 0";
            // 
            // playButton
            // 
            this.playButton.BackColor = System.Drawing.Color.DarkCyan;
            this.playButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playButton.Location = new System.Drawing.Point(696, 462);
            this.playButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(315, 103);
            this.playButton.TabIndex = 15;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = false;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // quitButton
            // 
            this.quitButton.BackColor = System.Drawing.Color.DarkCyan;
            this.quitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quitButton.Location = new System.Drawing.Point(696, 686);
            this.quitButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.quitButton.Name = "quitButton";
            this.quitButton.Size = new System.Drawing.Size(315, 103);
            this.quitButton.TabIndex = 16;
            this.quitButton.Text = "Quit";
            this.quitButton.UseVisualStyleBackColor = false;
            this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
            // 
            // playerDistanceFromChaserLabel
            // 
            this.playerDistanceFromChaserLabel.AutoSize = true;
            this.playerDistanceFromChaserLabel.BackColor = System.Drawing.Color.LightSlateGray;
            this.playerDistanceFromChaserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerDistanceFromChaserLabel.Location = new System.Drawing.Point(500, 223);
            this.playerDistanceFromChaserLabel.Name = "playerDistanceFromChaserLabel";
            this.playerDistanceFromChaserLabel.Size = new System.Drawing.Size(558, 55);
            this.playerDistanceFromChaserLabel.TabIndex = 17;
            this.playerDistanceFromChaserLabel.Text = "Distance From Chaser: 0";
            // 
            // saveMapButton
            // 
            this.saveMapButton.BackColor = System.Drawing.Color.DarkSlateGray;
            this.saveMapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveMapButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveMapButton.Location = new System.Drawing.Point(1168, 669);
            this.saveMapButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.saveMapButton.Name = "saveMapButton";
            this.saveMapButton.Size = new System.Drawing.Size(246, 94);
            this.saveMapButton.TabIndex = 18;
            this.saveMapButton.Text = "Save Map";
            this.saveMapButton.UseVisualStyleBackColor = false;
            this.saveMapButton.Click += new System.EventHandler(this.saveMapButton_Click);
            // 
            // selectSlotLabel
            // 
            this.selectSlotLabel.AutoSize = true;
            this.selectSlotLabel.BackColor = System.Drawing.Color.DarkCyan;
            this.selectSlotLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectSlotLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.selectSlotLabel.Location = new System.Drawing.Point(884, 468);
            this.selectSlotLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.selectSlotLabel.Name = "selectSlotLabel";
            this.selectSlotLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.selectSlotLabel.Size = new System.Drawing.Size(359, 55);
            this.selectSlotLabel.TabIndex = 19;
            this.selectSlotLabel.Text = "Select Map Slot";
            // 
            // slot1Button
            // 
            this.slot1Button.BackColor = System.Drawing.Color.DarkCyan;
            this.slot1Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slot1Button.Location = new System.Drawing.Point(890, 532);
            this.slot1Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.slot1Button.Name = "slot1Button";
            this.slot1Button.Size = new System.Drawing.Size(264, 75);
            this.slot1Button.TabIndex = 20;
            this.slot1Button.Text = "Slot 1";
            this.slot1Button.UseVisualStyleBackColor = false;
            this.slot1Button.Click += new System.EventHandler(this.slot1Button_Click);
            // 
            // slot3Button
            // 
            this.slot3Button.BackColor = System.Drawing.Color.DarkCyan;
            this.slot3Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slot3Button.Location = new System.Drawing.Point(890, 683);
            this.slot3Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.slot3Button.Name = "slot3Button";
            this.slot3Button.Size = new System.Drawing.Size(264, 75);
            this.slot3Button.TabIndex = 21;
            this.slot3Button.Text = "Slot 3";
            this.slot3Button.UseVisualStyleBackColor = false;
            this.slot3Button.Click += new System.EventHandler(this.slot3Button_Click);
            // 
            // slot2Button
            // 
            this.slot2Button.BackColor = System.Drawing.Color.DarkCyan;
            this.slot2Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slot2Button.Location = new System.Drawing.Point(890, 608);
            this.slot2Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.slot2Button.Name = "slot2Button";
            this.slot2Button.Size = new System.Drawing.Size(264, 75);
            this.slot2Button.TabIndex = 22;
            this.slot2Button.Text = "Slot 2";
            this.slot2Button.UseVisualStyleBackColor = false;
            this.slot2Button.Click += new System.EventHandler(this.slot2Button_Click);
            // 
            // loadMapButton
            // 
            this.loadMapButton.BackColor = System.Drawing.Color.DarkCyan;
            this.loadMapButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadMapButton.Location = new System.Drawing.Point(696, 574);
            this.loadMapButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(315, 103);
            this.loadMapButton.TabIndex = 23;
            this.loadMapButton.Text = "Load Map";
            this.loadMapButton.UseVisualStyleBackColor = false;
            this.loadMapButton.Click += new System.EventHandler(this.loadMapButton_Click);
            // 
            // mapInfoLabel
            // 
            this.mapInfoLabel.AutoSize = true;
            this.mapInfoLabel.BackColor = System.Drawing.Color.DarkCyan;
            this.mapInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapInfoLabel.Location = new System.Drawing.Point(886, 794);
            this.mapInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.mapInfoLabel.Name = "mapInfoLabel";
            this.mapInfoLabel.Size = new System.Drawing.Size(409, 52);
            this.mapInfoLabel.TabIndex = 24;
            this.mapInfoLabel.Text = "Please select a slot.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1924, 966);
            this.Controls.Add(this.mapInfoLabel);
            this.Controls.Add(this.loadMapButton);
            this.Controls.Add(this.slot2Button);
            this.Controls.Add(this.slot3Button);
            this.Controls.Add(this.slot1Button);
            this.Controls.Add(this.selectSlotLabel);
            this.Controls.Add(this.saveMapButton);
            this.Controls.Add(this.playerDistanceFromChaserLabel);
            this.Controls.Add(this.quitButton);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.highScoreLabel);
            this.Controls.Add(this.endlessRunnerLabel);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.viewStatisticsButton);
            this.Controls.Add(this.playAgainButton);
            this.Controls.Add(this.mainMenuButton);
            this.Controls.Add(this.gameOverLabel);
            this.Controls.Add(this.powerUpsPickedUpLabel);
            this.Controls.Add(this.obstaclesPassedLabel);
            this.Controls.Add(this.ActivePowerUpsLabel);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.coinCounterLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        public System.Windows.Forms.Label coinCounterLabel;
        public System.Windows.Forms.Label scoreLabel;
        public System.Windows.Forms.Label ActivePowerUpsLabel;
        public System.Windows.Forms.Label obstaclesPassedLabel;
        public System.Windows.Forms.Label powerUpsPickedUpLabel;
        private System.Windows.Forms.Label gameOverLabel;
        private System.Windows.Forms.Button mainMenuButton;
        private System.Windows.Forms.Button playAgainButton;
        private System.Windows.Forms.Button viewStatisticsButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Label endlessRunnerLabel;
        private System.Windows.Forms.Label highScoreLabel;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button quitButton;
        public System.Windows.Forms.Label playerDistanceFromChaserLabel;
        private System.Windows.Forms.Button saveMapButton;
        private System.Windows.Forms.Label selectSlotLabel;
        private System.Windows.Forms.Button slot1Button;
        private System.Windows.Forms.Button slot3Button;
        private System.Windows.Forms.Button slot2Button;
        private System.Windows.Forms.Button loadMapButton;
        private System.Windows.Forms.Label mapInfoLabel;
    }
}

