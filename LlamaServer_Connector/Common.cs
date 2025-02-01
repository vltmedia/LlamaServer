using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LlamaServer.Connector
{

    public class Settings
    {
        [Category("AI")]
        [DisplayName("Model Path")]
        [Description("Path to the GGUF model file.")]
        public string ModelPath { get; set; } = "";

        [Category("Network")]
        [DisplayName("Port")]
        [Description("Port number for the REST API.")]
        [DefaultValue(5598)]
        public int Port { get; set; } = 5598;

        [Category("Generation")]
        [DisplayName("Context Size")]
        [Description("Context size for the Llama model.")]
        [DefaultValue(1024)]
        public uint ContextSize { get; set; } = 1024;

        [Category("GPU")]
        [DisplayName("GPU Layers")]
        [Description("Number of GPU layers to offload.")]
        [DefaultValue(34)]
        public int GpuLayerCount { get; set; } = 34;

        [Category("Generation")]
        [DisplayName("Seed")]
        [Description("Seed used to change generated media.")]
        [DefaultValue(42)]
        public uint Seed { get; set; } = 42;

        [Category("Generation")]
        [DisplayName("Max Tokens")]
        [Description("Maximum tokens for response.")]
        [DefaultValue(256)]
        public int MaxTokens { get; set; } = 256;

        [Category("Generation")]
        [DisplayName("Temperature")]
        [Description("Temperature for the model.")]
        [DefaultValue(0.5f)]
        public float Temperature { get; set; } = 0.5f;

        [Category("Network")]
        [DisplayName("Return JSON Response")]
        [Description("Return JSON response.")]
        [DefaultValue(false)]
        public bool ReturnJson { get; set; } = false;

        [Category("Network")]
        [DisplayName("Auto Close On Quit")]
        [Description("Auto close the server when the application quits.")]
        [DefaultValue(true)]
        public bool AutoCloseOnQuit { get; set; } = true;

      
    }

}
