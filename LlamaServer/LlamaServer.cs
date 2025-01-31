using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LLama.Common;
using LLama;
using CommandLine;
using static LlamaRestServer;

class LlamaRestServer
{
    private static ChatSession? session;
    private static InferenceParams inferenceParams;
    public static Options options;
    // ✅ Define command-line arguments
    public class Options
    {
        [Option("model", Required = true, Default = "DeepSeek-R1-Distill-Llama-8B-Q8_0.gguf", HelpText = "Path to the GGUF model file.")]
        public string ModelPath { get; set; } = "";

        [Option("port", Required = false, Default = 5598, HelpText = "Port number for the REST API.")]
        public int Port { get; set; }

        [Option("contextSize", Required = false, Default = 1024, HelpText = "Context size for Llama model.")]
        public uint ContextSize { get; set; }

        [Option("gpuLayers", Required = false, Default = 34, HelpText = "Number of GPU layers to offload.")]
        public int GpuLayerCount { get; set; }

        [Option("maxTokens", Required = false, Default = 256, HelpText = "Maximum tokens for response.")]
        public int MaxTokens { get; set; }

        [Option("returnJson", Required = false, Default = true, HelpText = "Return JSON response.")]
        public bool ReturnJson { get; set; } = true;
  
    }

    public static async Task Main(string[] args)
    {
        // ✅ Parse command-line arguments
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(async options_ => await RunServer(options_));
            
    }


    public static async Task RunServer(Options options_)
    {
        options = options_;
        // ✅ Load LlamaSharp Model
        var parameters = new ModelParams(options.ModelPath)
        {
            ContextSize = options.ContextSize,
            GpuLayerCount = options.GpuLayerCount
        };

        using var model = LLamaWeights.LoadFromFile(parameters);
        using var context = model.CreateContext(parameters);
        var executor = new InteractiveExecutor(context);

        // ✅ Strict JSON Prompt Setup
        var chatHistory = new ChatHistory();
        chatHistory.AddMessage(AuthorRole.System,
    "You are a pulitzer prize headline writer. Write a Headline and Body text for an Magazine front-page headline about the person in the data provided. Make the generated JSON string about them and the answers provided. \n"
    + "Incorporate the person's name, and company, artistically allude to the person's title, and answers.  \n"
    + "Headline should be a maximum of 8 words, and Body should be a maximum of 20 words. \n"
    + "You must generate a JSON string in this format: \n"
    + "<|START|>{ \"headline\": \"Your headline here\", \"body\": \"Your body text here\" }<|FINISHED|><|END|>\n"
    + "You MUST ONLY return a valid JSON string. DO NOT provide explanations. DO NOT add extra text. \n"
    + "When you are finished, respond with <|END|>. \n"
    + "Example:\n"
    + "Output:\n"
    + "<|START|>{ \"headline\": \"AI Breakthrough Redefines Business\", \"body\": \"Industry experts say this changes everything.\" }<|FINISHED|><|END|> \n"
    + "Now generate a response in the same format. Begin output:\n"
);

        session = new ChatSession(executor, chatHistory);
        inferenceParams = new InferenceParams()
        {
            MaxTokens = options.MaxTokens,
            AntiPrompts = new List<string> { "}\n", "} ", "<|END|>", "<|END|>\n", "<|FINISHED|><|END|>\n" }
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
                // ✅ Parse User Input
                var requestData = JsonSerializer.Deserialize<UserRequest>(requestBody);
                string formattedUserInput = $"<|User|>\n{requestData.UserInput}\n<|Assistant|>\nOutput:\n";

                // ✅ Generate AI Response
                StringBuilder aiResponse = new StringBuilder();
                await foreach (var text in session!.ChatAsync(
                    new ChatHistory.Message(AuthorRole.User, formattedUserInput),
                    inferenceParams))
                {
                    aiResponse.Append(text);
                }


                // ✅ Extract JSON Response
                if(options.ReturnJson == true)
                {
                    string jsonResponse = ExtractJson(aiResponse.ToString());
                    aiResponse.Clear();
                    aiResponse.Append(jsonResponse);
                }

                // ✅ Send JSON Response
                byte[] buffer = Encoding.UTF8.GetBytes(aiResponse.ToString());
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                // Return error response
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
}
