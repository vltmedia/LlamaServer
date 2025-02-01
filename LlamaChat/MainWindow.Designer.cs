

namespace LlamaServer_Connector_TestUI
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            sendButton = new Button();
            userInput = new TextBox();
            statusText = new Label();
            chatHistory = new TextBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            serverToolStripMenuItem = new ToolStripMenuItem();
            startServerToolStripMenuItem = new ToolStripMenuItem();
            killServerToolStripMenuItem = new ToolStripMenuItem();
            openFileDialog1 = new OpenFileDialog();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // sendButton
            // 
            sendButton.Enabled = false;
            sendButton.FlatAppearance.BorderColor = Color.White;
            sendButton.FlatStyle = FlatStyle.Flat;
            sendButton.ForeColor = SystemColors.ControlLightLight;
            sendButton.Location = new Point(713, 429);
            sendButton.Name = "sendButton";
            sendButton.Size = new Size(75, 23);
            sendButton.TabIndex = 2;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += sendButton_Click;
            // 
            // userInput
            // 
            userInput.BackColor = Color.FromArgb(1, 3, 5);
            userInput.BorderStyle = BorderStyle.FixedSingle;
            userInput.ForeColor = SystemColors.Window;
            userInput.Location = new Point(18, 350);
            userInput.Multiline = true;
            userInput.Name = "userInput";
            userInput.Size = new Size(770, 69);
            userInput.TabIndex = 4;
            // 
            // statusText
            // 
            statusText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            statusText.BackColor = Color.Black;
            statusText.ForeColor = Color.FromArgb(64, 64, 64);
            statusText.Location = new Point(0, 464);
            statusText.Margin = new Padding(0, 0, 3, 0);
            statusText.Name = "statusText";
            statusText.Padding = new Padding(15, 3, 10, 0);
            statusText.Size = new Size(800, 25);
            statusText.TabIndex = 5;
            statusText.Text = "Status";
            // 
            // chatHistory
            // 
            chatHistory.BackColor = Color.FromArgb(1, 3, 5);
            chatHistory.BorderStyle = BorderStyle.FixedSingle;
            chatHistory.ForeColor = SystemColors.Window;
            chatHistory.Location = new Point(18, 38);
            chatHistory.Multiline = true;
            chatHistory.Name = "chatHistory";
            chatHistory.ReadOnly = true;
            chatHistory.Size = new Size(770, 289);
            chatHistory.TabIndex = 6;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, serverToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 7;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(97, 22);
            exitToolStripMenuItem.Text = "Quit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { preferencesToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "Edit";
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(135, 22);
            preferencesToolStripMenuItem.Text = "Preferences";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click_2;
            // 
            // serverToolStripMenuItem
            // 
            serverToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startServerToolStripMenuItem, killServerToolStripMenuItem });
            serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            serverToolStripMenuItem.Size = new Size(51, 20);
            serverToolStripMenuItem.Text = "Server";
            // 
            // startServerToolStripMenuItem
            // 
            startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            startServerToolStripMenuItem.Size = new Size(133, 22);
            startServerToolStripMenuItem.Text = "Start Server";
            // 
            // killServerToolStripMenuItem
            // 
            killServerToolStripMenuItem.Name = "killServerToolStripMenuItem";
            killServerToolStripMenuItem.Size = new Size(133, 22);
            killServerToolStripMenuItem.Text = "Kill Server";
            // 
            // openFileDialog1
            // 
            openFileDialog1.DefaultExt = "gguf";
            openFileDialog1.FileName = "openFileDialog1";
            openFileDialog1.Filter = "GGUF Files|*.gguf|All files|*.*";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(9, 11, 16);
            ClientSize = new Size(800, 488);
            Controls.Add(chatHistory);
            Controls.Add(statusText);
            Controls.Add(userInput);
            Controls.Add(sendButton);
            Controls.Add(menuStrip1);
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            Text = "Chat Bot";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button sendButton;
        private TextBox userInput;
        private Label statusText;
        private TextBox chatHistory;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripMenuItem serverToolStripMenuItem;
        private ToolStripMenuItem startServerToolStripMenuItem;
        private ToolStripMenuItem killServerToolStripMenuItem;
        private OpenFileDialog openFileDialog1;
    }
}
