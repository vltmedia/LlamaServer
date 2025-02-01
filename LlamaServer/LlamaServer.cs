using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LLama.Common;
using LLama;
using CommandLine;
using static LlamaRestServer;
using LLama.Sampling;

class LlamaRestServer
{
    private static ChatSession? session;
    private static InferenceParams inferenceParams;
    public static Options options;
    public static string modelsFolderPath {get{
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
            }}
    public static string modelsConfigPath {get{
            return Path.Combine(modelsFolderPath, "config.json");
            }}

    // ✅ Define command-line arguments
    public class Options
    {
        [Option("model", Required = false, Default = "", HelpText = "Path to the GGUF model file.")]
        public string ModelPath { get; set; } = "";
        
        [Option("systemPrompt", Required = false, Default = "", HelpText = "System Prompt that will handle the behavior")]
        public string SystemPrompt { get; set; } = "";

        [Option("antiPrompts", Required = false, Default = "", HelpText = "Anti-prompts to stop the model from generating. Comma sepperated")]
        public string AntiPrompts { get; set; } = "";

        [Option("port", Required = false, Default = 5598, HelpText = "Port number for the REST API.")]
        public int Port { get; set; }

        [Option("contextSize", Required = false, Default = 1024, HelpText = "Context size for Llama model.")]
        public int ContextSize { get; set; }

        [Option("gpuLayers", Required = false, Default = 34, HelpText = "Number of GPU layers to offload.")]
        public int GpuLayerCount { get; set; }
        [Option("seed", Required = false, Default = 42, HelpText = "Seed used to change generated media.")]
        public int Seed { get; set; }

        [Option("maxTokens", Required = false, Default = 256, HelpText = "Maximum tokens for response.")]
        public int MaxTokens { get; set; }
        [Option("temperature", Required = false, Default = 0.5f, HelpText = "Temperature for the model")]
        public float Temperature { get; set; }

        [Option("returnJson", Required = false, Default = false, HelpText = "Return JSON response.")]
        public bool ReturnJson { get; set; }
        

        [Option("stream", Required = false, Default = false, HelpText = "Wheter to return the data in a streaming manner or a single chunk.")]
        public bool Stream { get; set; }

        public string Args { get {

                var str = string.Format("--model \"{0}\" --port {1} --contextSize {2} --gpuLayers {3} --seed {4} --maxTokens {5} --temperature {6}  --systemPrompt \"{8}\" --antiPrompts \"{9}\"",
                ModelPath, Port, ContextSize, GpuLayerCount, Seed, MaxTokens, Temperature, ReturnJson, SystemPrompt, AntiPrompts
                );
                    if(ReturnJson)
                    {
                        str += " --returnJson";
                    }
                    return str;
                    
                    
                    ; } }
    }

    public static async Task Main(string[] args)
    {
        // ✅ Parse command-line arguments
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(async options_ => await RunServer(options_));
            
    }
    public static string VerifyModel(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            if (Directory.Exists(modelsFolderPath))
            {
                // Check for a default model
                if (File.Exists(modelsConfigPath))
                {
                    var configData = File.ReadAllText(modelsConfigPath);
                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(configData);

                    if (data.TryGetValue("DefaultModel", out string defaultValue))
                    {
                    //var config = JsonSerializer.Deserialize<Config>(configData);
                    //if (config != null)
                    //{
                        if (defaultValue != "")
                        {
                            var defaultPath = Path.Combine(modelsFolderPath, defaultValue);
                            if (File.Exists(defaultPath))
                            {
                                return defaultPath;
                            }
                        }
                    }
                }

                // Check for any GGUF files in the models folder if no default model is set
                var ggufFiles = Directory.GetFiles(modelsFolderPath, "*.gguf");
                if (ggufFiles.Length > 0)
                {
                    return ggufFiles[0];
                }
                else
                {
                    return "";
                }
            }
            else
            {
                Directory.CreateDirectory(modelsFolderPath);
                return "";
            }
        }
        else
        {
            return path;
        }
    }

    public static async Task RunServer(Options options_)
    {
        options = options_;
        options.ModelPath = VerifyModel(options.ModelPath);
        Console.WriteLine(options.Args);
        if (options.ModelPath == "")
        {
            Console.WriteLine("No model found. Please provide a valid model path.");
            Console.WriteLine("Alternatively, place a GGUF model file in the 'models' folder or set a default model in 'config.json'.");
            Console.WriteLine("Shutting down.");
            return;
        }
        // ✅ Load LlamaSharp Model
        var parameters = new ModelParams(options.ModelPath)
        {
            ContextSize = (uint)options.ContextSize,
            GpuLayerCount = options.GpuLayerCount
        };

        using var model = LLamaWeights.LoadFromFile(parameters);
        using var context = model.CreateContext(parameters);
        var executor = new InteractiveExecutor(context);

        // ✅ Strict JSON Prompt Setup
        var chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System,
            options.SystemPrompt != "" ? options.SystemPrompt.Replace("\\n", "\n") :
    "You are a pulitzer prize headline writer. Write a Headline and Body text for an Magazine front-page headline about the person in the data provided. Make the generated JSON string about them and the answers provided. \n"
    + "Use <|START|> to start the response, and end your response with <|FINISHED|><|END|>\n"
    + "You must generate a response in this format: \n"
    + "<|START|>Your response here<|FINISHED|><|END|>\n"
    + "When you are finished, respond with <|FINISHED|><|END|>. \n"
    
);

        session = new ChatSession(executor, chatHistory);
        List<string> AntiPrompts = new List<string>() { "}\n", "} ", "<|END|>", "<|END|>\n", "<|FINISHED|><|END|>\n" };
        if (options.AntiPrompts != "")
        {
            if (options.AntiPrompts.Contains(","))
            {
                AntiPrompts.AddRange(options.AntiPrompts.Replace("\\n", "\n").Split(',').ToList());
            }
            else
            {
                AntiPrompts.Add(options.AntiPrompts.Replace("\\n", "\n"));
            }

        }
        inferenceParams = new InferenceParams()
        {
            MaxTokens = options.MaxTokens,
            AntiPrompts = AntiPrompts,
            SamplingPipeline = new DefaultSamplingPipeline
            {
                Temperature = options.Temperature,
                Seed = (uint)options.Seed
            }
        };

        // ✅ Start REST API Server
        Console.WriteLine(string.Format("Starting Llama REST API on http://localhost:{0}/ ...", options.Port.ToString()));
        await StartHttpServer(options.Port);
    }

    private static async Task StartHttpServer(int port)
    {
        var httpListener = new HttpListener();  // ✅ No 'using' to keep it alive
        httpListener.Prefixes.Add($"http://localhost:{port}/");
        httpListener.Start();

        Console.WriteLine($"Server running on port {port}. Send POST requests to http://localhost:{port}/");

        while (true)  // ✅ Keeps server alive
        {
            HttpListenerContext context = await httpListener.GetContextAsync();
            _ = HandleRequest(context);
        }
    }

    private static async Task HandleRequest(HttpListenerContext context)
    {
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        if (request.HttpMethod == "POST" && request.InputStream != null)
        {
            using var reader = new StreamReader(request.InputStream, Encoding.UTF8);
            string requestBody = await reader.ReadToEndAsync();

            try
            {
                var requestData = JsonSerializer.Deserialize<UserRequest>(requestBody);
                string formattedUserInput = $"<|User|>\n{requestData.UserInput}\n<|Assistant|>\nOutput:\n";

                response.StatusCode = 200;
                response.ContentType = options.ReturnJson ? "application/json" : "text/plain";

                if (options.Stream)
                {
                    response.SendChunked = true;  // ✅ Enables streaming response
                    await response.OutputStream.FlushAsync();  // Ensure headers are sent

                    await foreach (var text in session!.ChatAsync(
                        new ChatHistory.Message(AuthorRole.User, formattedUserInput),
                        inferenceParams))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(text);
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        await response.OutputStream.FlushAsync(); // ✅ Ensures client gets each chunk
                    }
                }
                else
                {
                    // ✅ Collect Full Response (No Streaming)
                    StringBuilder aiResponse = new StringBuilder();
                    await foreach (var text in session!.ChatAsync(
                        new ChatHistory.Message(AuthorRole.User, formattedUserInput),
                        inferenceParams))
                    {
                        aiResponse.Append(text);
                    }

                    byte[] buffer = Encoding.UTF8.GetBytes(aiResponse.ToString());
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                }

                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                byte[] buffer = Encoding.UTF8.GetBytes($"{{ \"error\": \"{ex.Message}\" }}");
                response.StatusCode = 500;
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                response.OutputStream.Close();
            }
        }
        else
        {
            response.StatusCode = 400;
            response.Close();
        }
    }


    // ✅ Extracts JSON from Llama output
    private static string ExtractJson(string responseText)
    {
        int start = responseText.IndexOf("<|START|>") + "<|START|>".Length;
        int end = responseText.IndexOf("<|FINISHED|><|END|>");

        if (start >= 0 && end >= 0 && end > start)
        {
            return responseText.Substring(start, end - start).Trim();
        }

        return "{ \"error\": \"Invalid response format\" }";
    }

    public class UserRequest
    {
        public string UserInput { get; set; } = "";
    }

    // Define a class to match the JSON structure
    [System.Serializable]
    class Config
    {
        public string DefaultModel ;
    }
}
