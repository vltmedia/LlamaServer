# LLAMA Server Connector
This is a a connector to the LlamaServer.exe

# Features
- Check if the LlamaServer is running
- Auto start the LlamaServer if it isn't running
- Kill the LlamaServer

# Usage
Import the `LLamaServer_Connector.dll` into your project.

## Example
```csharp


using LlamaServer.Connector;
using System.Windows.Forms;
namespace LlamaServer_Connector_TestUI
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startServerToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            StartServer();
        }
        public void StartServer()
        {
            LLamaServerConnector.CheckProcessRunning(OnRunning, OnNotRunning,Arguments:PreferencesSettings.Arguments, TryStart: true, CreateNoWindow: false, WindowStyle: System.Diagnostics.ProcessWindowStyle.Hidden);
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void OnNotRunning()
        {
            // Show a message box
            DialogResult result = MessageBox.Show(
            "The Llama Serve isn't running, would you like to start it?",   // Message text
            "Llama Server Not Running",                         // Title
            MessageBoxButtons.YesNo,                // Yes/No buttons
            MessageBoxIcon.Question                 // Question icon
);
            if (result == DialogResult.Yes)
            {
                StartServer();
            }
            else
            {
                statusText.Text = LLamaServerConnector.processNameOS + " Not Running";
                sendButton.Enabled = false;
            }

        }

        private void OnRunning()
        {
            statusText.Text = LLamaServerConnector.processNameOS + " Running at port: " + LLamaServerConnector.port;
            sendButton.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            LLamaServerConnector.KillProcess();
            statusText.Text = LLamaServerConnector.processNameOS + " Killed";
        }
        public void AddLine(string line)
        {
            chatHistory.Text += line + Environment.NewLine + Environment.NewLine;
        }
        private async void sendButton_Click(object sender, EventArgs e)
        {
            statusText.Text = "Sending... " + userInput.Text;
            sendButton.Enabled = false;
            var resp = await LLamaServerConnector.SendUserInputAsync(userInput.Text);
            AddLine("User: " + userInput.Text);
            AddLine("Bot: " + resp);
            statusText.Text = "Received: " + resp;
            sendButton.Enabled = true;

        }

        private void preferencesToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            Preferences preferences = new Preferences();
            preferences.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LLamaServerConnector.CheckProcessRunning(OnRunning, OnNotRunning, TryStart: false, CreateNoWindow: false, WindowStyle: System.Diagnostics.ProcessWindowStyle.Hidden);

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

```