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

    public class PreferencesSettings : Settings
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
            Properties.Settings.Default.ModelPath = ModelPath;
            Properties.Settings.Default.Port = Port;
            Properties.Settings.Default.ContextSize = ContextSize;
            Properties.Settings.Default.GpuLayerCount = GpuLayerCount;
            Properties.Settings.Default.Seed = Seed;
            Properties.Settings.Default.MaxTokens = MaxTokens;
            Properties.Settings.Default.Temperature = Temperature;
            Properties.Settings.Default.ReturnJson = ReturnJson;
            Properties.Settings.Default.SystemPrompt = SystemPrompt;
            Properties.Settings.Default.AntiPrompts = AntiPrompts;

            Properties.Settings.Default.Save(); // Saves settings to disk
        }
        public void Load()
        {
            ModelPath = Properties.Settings.Default.ModelPath;
            Port = Properties.Settings.Default.Port;
            ContextSize = Properties.Settings.Default.ContextSize;
            GpuLayerCount = Properties.Settings.Default.GpuLayerCount;
            Seed = Properties.Settings.Default.Seed;
            MaxTokens = Properties.Settings.Default.MaxTokens;
            Temperature = Properties.Settings.Default.Temperature;
            ReturnJson = Properties.Settings.Default.ReturnJson;
            SystemPrompt = Properties.Settings.Default.SystemPrompt;
            AntiPrompts = Properties.Settings.Default.AntiPrompts;

        }
        public static string Arguments { get
            {
                var settings = new PreferencesSettings();
                settings.Load();
                return string.Format(
                    "--model_path \"{0\"} --port {1} --context_size {2} --gpuLayers {3} --seed {4} --maxTokens {5} --temperature {6} --returnJson {7} --systemPrompt \"{8}\" --antiPrompts \"{9}\"",
                    settings.ModelPath, settings.Port, settings.ContextSize, settings.GpuLayerCount, settings.Seed, settings.MaxTokens, settings.Temperature, settings.ReturnJson, settings.SystemPrompt, settings.AntiPrompts
                    );

            } }
    }
}
