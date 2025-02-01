using System;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LlamaServer.Connector
{
    [System.Serializable]
    public class LLamaServerConnector
    {
        public static string processName = "LlamaServer";
        public static string processNameOS { get { 
            return processName + GetAppExtension();
            } }
        public static int port = 5598;

        public static string GetAppExtension()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return ".exe";
            return "";
        }
        public static void CheckProcessRunning( Action onRunning, Action onNotRunning, string ProcessPath = "C:/Program Files/Llama Server/Llama Server/LlamaServer.exe")
        {
            ProcessMonitor processMonitor = new ProcessMonitor(processNameOS, () =>
            {
                port = ProcessMonitor.GetProcessPort(processName) ?? port;
                onRunning();
            }, () =>
            {
               if(System.IO.File.Exists(ProcessPath ))
               {
                   System.Diagnostics.Process.Start(ProcessPath);
                    onRunning();
               }
               else
               {
                    onNotRunning();
               }
            }, 0);
        }

        public static async Task<string> SendPostRequestAsync(string url, string json)
        {
            using (HttpClient client = new HttpClient())
            {
                // Set request content type to JSON
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the request
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Throw if status code is not success

                // Read response content
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static string SendUserInput(string userInput, string host = "http://localhost")
        {
            return SendUserInputAsync(userInput, host).Result;
        }
        public static async Task<string> SendUserInputAsync(string userInput, string host="http://localhost")
        {
            UserRequest userRequest = new UserRequest();
            userRequest.UserInput = userInput;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(userRequest);
            string res = await SendPostRequestAsync(string.Format("{0}:{1}", host, port), json);
            if (res != null) {
                return res;
            }
            return "";
        }

        public static void KillProcess()
        {
            ProcessMonitor.KillProcessByName(processNameOS);
        }
        public class UserRequest
        {
            public string UserInput { get; set; } = "";
        }


    }
}
