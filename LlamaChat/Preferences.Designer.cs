using LlamaServer.Connector;

namespace LlamaServer_Connector_TestUI
{
    partial class Preferences
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
            PreferencesSettings preferencesSettings1 = new PreferencesSettings();
            preferencesSettings1.Load();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preferences));
            propertyGrid2 = new PropertyGrid();
            killServerButton = new Button();
            startServerButton = new Button();
            SuspendLayout();
            // 
            // propertyGrid2
            // 
            propertyGrid2.CommandsForeColor = SystemColors.ActiveCaption;
            propertyGrid2.Location = new Point(12, 12);
            propertyGrid2.Name = "propertyGrid2";
            preferencesSettings1.ContextSize = 1024U;
            preferencesSettings1.ModelPath = "";
            preferencesSettings1.Seed = 42U;
            preferencesSettings1.SystemPrompt = "This is a large text box.\nYou can enter multiple lines.";
            propertyGrid2.SelectedObject = preferencesSettings1;
            propertyGrid2.Size = new Size(776, 388);
            propertyGrid2.TabIndex = 0;
            // 
            // killServerButton
            // 
            killServerButton.FlatAppearance.BorderColor = Color.White;
            killServerButton.FlatStyle = FlatStyle.Flat;
            killServerButton.ForeColor = SystemColors.ControlLightLight;
            killServerButton.Location = new Point(713, 415);
            killServerButton.Name = "killServerButton";
            killServerButton.Size = new Size(75, 23);
            killServerButton.TabIndex = 3;
            killServerButton.Text = "Save";
            killServerButton.UseVisualStyleBackColor = true;
            killServerButton.Click += killServerButton_Click;
            // 
            // startServerButton
            // 
            startServerButton.FlatAppearance.BorderColor = Color.White;
            startServerButton.FlatStyle = FlatStyle.Flat;
            startServerButton.ForeColor = SystemColors.ControlLightLight;
            startServerButton.Location = new Point(632, 415);
            startServerButton.Name = "startServerButton";
            startServerButton.Size = new Size(75, 23);
            startServerButton.TabIndex = 2;
            startServerButton.Text = "Close";
            startServerButton.UseVisualStyleBackColor = true;
            startServerButton.Click += startServerButton_Click;
            // 
            // Preferences
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(9, 11, 16);
            ClientSize = new Size(800, 450);
            Controls.Add(killServerButton);
            Controls.Add(startServerButton);
            Controls.Add(propertyGrid2);
            ForeColor = SystemColors.ControlLightLight;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Preferences";
            Text = "Preferences";
            Load += Preferences_Load;
            ResumeLayout(false);
        }

        #endregion

        private PropertyGrid propertyGrid2;
        private Button killServerButton;
        private Button startServerButton;
    }
}