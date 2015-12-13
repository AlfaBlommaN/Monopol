namespace Monopol
{
    partial class Menu
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
            this.buttonAddPlayer = new System.Windows.Forms.Button();
            this.textBoxPlayerName = new System.Windows.Forms.TextBox();
            this.labelPlayerName = new System.Windows.Forms.Label();
            this.buttonStartGame = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.AIcheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonAddPlayer
            // 
            this.buttonAddPlayer.Location = new System.Drawing.Point(31, 51);
            this.buttonAddPlayer.Name = "buttonAddPlayer";
            this.buttonAddPlayer.Size = new System.Drawing.Size(119, 23);
            this.buttonAddPlayer.TabIndex = 0;
            this.buttonAddPlayer.Text = "Lägg till spelare!";
            this.buttonAddPlayer.UseVisualStyleBackColor = true;
            this.buttonAddPlayer.Click += new System.EventHandler(this.buttonAddPlayer_Click);
            // 
            // textBoxPlayerName
            // 
            this.textBoxPlayerName.Location = new System.Drawing.Point(31, 25);
            this.textBoxPlayerName.Name = "textBoxPlayerName";
            this.textBoxPlayerName.Size = new System.Drawing.Size(119, 20);
            this.textBoxPlayerName.TabIndex = 1;
            this.textBoxPlayerName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPlayerName_KeyPress);
            // 
            // labelPlayerName
            // 
            this.labelPlayerName.AutoSize = true;
            this.labelPlayerName.Location = new System.Drawing.Point(28, 9);
            this.labelPlayerName.Name = "labelPlayerName";
            this.labelPlayerName.Size = new System.Drawing.Size(69, 13);
            this.labelPlayerName.TabIndex = 2;
            this.labelPlayerName.Text = "Spelarnamn: ";
            // 
            // buttonStartGame
            // 
            this.buttonStartGame.Location = new System.Drawing.Point(55, 80);
            this.buttonStartGame.Name = "buttonStartGame";
            this.buttonStartGame.Size = new System.Drawing.Size(75, 23);
            this.buttonStartGame.TabIndex = 3;
            this.buttonStartGame.Text = "Spela!";
            this.buttonStartGame.UseVisualStyleBackColor = true;
            this.buttonStartGame.Click += new System.EventHandler(this.buttonStartGame_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 109);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Ladda tidigare spel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AIcheckBox
            // 
            this.AIcheckBox.AutoSize = true;
            this.AIcheckBox.Location = new System.Drawing.Point(156, 28);
            this.AIcheckBox.Name = "AIcheckBox";
            this.AIcheckBox.Size = new System.Drawing.Size(36, 17);
            this.AIcheckBox.TabIndex = 5;
            this.AIcheckBox.Text = "AI";
            this.AIcheckBox.UseVisualStyleBackColor = true;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 144);
            this.Controls.Add(this.AIcheckBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxPlayerName);
            this.Controls.Add(this.buttonAddPlayer);
            this.Controls.Add(this.buttonStartGame);
            this.Controls.Add(this.labelPlayerName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Menu";
            this.Text = "Monopol";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAddPlayer;
        private System.Windows.Forms.TextBox textBoxPlayerName;
        private System.Windows.Forms.Label labelPlayerName;
        private System.Windows.Forms.Button buttonStartGame;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox AIcheckBox;
    }
}