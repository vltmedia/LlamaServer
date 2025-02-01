using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LlamaServer.Connector;
using LlamaServer.UI;
using LlamaChat;

namespace LlamaServer_Connector_TestUI
{
    public partial class Preferences : Form
    {
        public Preferences()
        {
            InitializeComponent();

        }

        private void Preferences_Load(object sender, EventArgs e)
        {
            var newSettings = new PreferencesSettings();
            newSettings.Load();
            propertyGrid2.SelectedObject = newSettings;

        }

        private void killServerButton_Click(object sender, EventArgs e)
        {
            var sel = propertyGrid2.SelectedObject as PreferencesSettings;
            sel.Save();
            MessageBox.Show("Settings saved.");

        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }

    public class PreferencesSettings : LlamaServer.Connector.Settings
    {
        [Category("AI")]
        [DisplayName("System Prompt")]
        [Description("Determines the AI System behavior.")]

        [Editor(typeof(MultilineTextEditor), typeof(UITypeEditor))] // 👈 Custom Editor
        public string SystemPrompt { get; set; } = "";
        [Category("AI")]
        [DisplayName("Anti Prompts")]
        [Description("Handles the end of the response")]
        [Editor(typeof(MultilineTextEditor), typeof(UITypeEditor))] // 👈 Custom Editor
        public string AntiPrompts { get; set; } = "<|END|>";

        public void Save()
        {
            LlamaChat.Properties.Settings.Default.ModelPath = ModelPath;
            LlamaChat.Properties.Settings.Default.Port = Port;
            LlamaChat.Properties.Settings.Default.ContextSize = ContextSize;
            LlamaChat.Properties.Settings.Default.GpuLayerCount = GpuLayerCount;
            LlamaChat.Properties.Settings.Default.Seed = Seed;
            LlamaChat.Properties.Settings.Default.MaxTokens = MaxTokens;
            LlamaChat.Properties.Settings.Default.Temperature = Temperature;
            LlamaChat.Properties.Settings.Default.ReturnJson = ReturnJson;
            LlamaChat.Properties.Settings.Default.SystemPrompt = SystemPrompt;
            LlamaChat.Properties.Settings.Default.AntiPrompts = AntiPrompts;

            LlamaChat.Properties.Settings.Default.Save(); // Saves settings to disk
        }
        public void Load()
        {
            ModelPath = LlamaChat.Properties.Settings.Default.ModelPath;
            Port = LlamaChat.Properties.Settings.Default.Port;
            ContextSize = LlamaChat.Properties.Settings.Default.ContextSize;
            GpuLayerCount = LlamaChat.Properties.Settings.Default.GpuLayerCount;
            Seed = LlamaChat.Properties.Settings.Default.Seed;
            MaxTokens = LlamaChat.Properties.Settings.Default.MaxTokens;
            Temperature = LlamaChat.Properties.Settings.Default.Temperature;
            ReturnJson = LlamaChat.Properties.Settings.Default.ReturnJson;
            SystemPrompt = LlamaChat.Properties.Settings.Default.SystemPrompt;
            AntiPrompts = LlamaChat.Properties.Settings.Default.AntiPrompts;

        }
        public static string Arguments { get
            {
                var settings = new PreferencesSettings();

                settings.Load();
                var resp= string.Format(
                    "--model \"{0}\" --port {1} --contextSize {2} --gpuLayers {3} --seed {4} --maxTokens {5} --temperature {6} --returnJson {7} --systemPrompt \"{8}\" --antiPrompts \"{9}\"",
                    settings.ModelPath, settings.Port, settings.ContextSize, settings.GpuLayerCount, settings.Seed, settings.MaxTokens, settings.Temperature, settings.ReturnJson, settings.SystemPrompt, settings.AntiPrompts
                    );
                return resp;

            } }
    }
}
