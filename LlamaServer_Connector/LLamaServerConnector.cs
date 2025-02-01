using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public static void CheckProcessRunning( Action onRunning, Action onNotRunning, string Arguments = "", bool TryStart=false, string ProcessPath = "", bool CreateNoWindow = false, System.Diagnostics.ProcessWindowStyle WindowStyle = System.Diagnostics.ProcessWindowStyle.Minimized)
        {
            ProcessMonitor processMonitor = new ProcessMonitor(processName, () =>
            {
                //port = ProcessMonitor.GetProcessPort(processName) ?? port;
                onRunning();
            }, () =>
            {
                var installedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LlamaServer.exe");
                var installedPathProgramFiles = "C:/Program Files/Llama Server/Llama Server/LlamaServer.exe";
                var installedPathDev = "NOTTOBEFOUND/NO";
                try
                {
                    installedPathDev = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.FullName)?.FullName)?.FullName)?.FullName)?.FullName, "LlamaServer\\bin\\Debug\\net8.0", "LlamaServer.exe");
                }catch {
                
                
                }
                    if (ProcessPath == "")
                {
                    if (System.IO.File.Exists(installedPath) && TryStart)
                    {
                        ProcessPath = installedPath;

                    }
                    else if (System.IO.File.Exists(installedPathDev) && installedPathDev != "NOTTOBEFOUND/NO"&& TryStart)
                    {
                        ProcessPath = installedPathDev;

                    }
                    else if (System.IO.File.Exists(installedPathProgramFiles) && TryStart)
                    {
                        ProcessPath = installedPathProgramFiles;

                    }
                   
                }
                if (System.IO.File.Exists(ProcessPath ) && TryStart)
               {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = $"\"{ProcessPath}\"";
                    process.StartInfo.Arguments = Arguments;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(ProcessPath);
                    process.Start();
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
            ProcessMonitor.KillProcessByName(processName);
        }
        public class UserRequest
        {
            public string UserInput { get; set; } = "";
        }


    }
}
