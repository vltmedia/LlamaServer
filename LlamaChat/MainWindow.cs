using LlamaServer.Connector;
using System.Windows.Forms;
namespace LlamaServer_Connector_TestUI
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            statusText.Text = "";

            startServerToolStripMenuItem.Click += startServerToolStripMenuItem_Click;
            killServerToolStripMenuItem.Click += killServerToolStripMenuItem_Click;

        }

        private void killServerToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            button2_Click(sender, e);
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
        public void SetModel()
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                LlamaChat.Properties.Settings.Default.ModelPath = openFileDialog1.FileName;
                LlamaChat.Properties.Settings.Default.Save();
            }
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
                if(LlamaChat.Properties.Settings.Default.ModelPath == "")
                {
                    SetModel();
                }


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
        public void AppendText(string text)
        {
            chatHistory.Text += text;
        }
        private async void sendButton_Click(object sender, EventArgs e)
        {
            statusText.Text = "Sending... " + userInput.Text;
            sendButton.Enabled = false;
            string resp = "";
            AddLine("User: " + userInput.Text);

            if (LlamaChat.Properties.Settings.Default.Stream)
            {
                AppendText("Bot: " );

                resp = await LLamaServerConnector.SendUserInputAsyncStream(userInput.Text, onChunk: OnChunkReceived);
                AppendText("\n");

            }
            else
            {
                resp = await LLamaServerConnector.SendUserInputAsync(userInput.Text);
                AddLine("Bot: " + resp);

            }
            statusText.Text = "Received: " + resp;
            sendButton.Enabled = true;

        }

        private void OnChunkReceived(string obj)
        {
            AppendText(obj);

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
