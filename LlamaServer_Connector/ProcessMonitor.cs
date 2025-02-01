using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

public class ProcessMonitor
{
    private readonly string _processName;
    private readonly Action _onRunning;
    private readonly Action _onNotRunning;
    private readonly Timer _timer;

    public ProcessMonitor(string processName, Action onRunning, Action onNotRunning, int checkIntervalMs = 0)
    {
        _processName = processName;
        _onRunning = onRunning;
        _onNotRunning = onNotRunning;
        if (checkIntervalMs > 0)
        {
            _timer = new Timer(CheckProcess, null, 0, checkIntervalMs);
        }
        else
        {
            CheckProcess(null);
        }
    }
    public static int? GetProcessPort(string processName)
    {
        var process = Process.GetProcessesByName(processName).FirstOrDefault();
        if (process == null)
            return null;

        int processId = process.Id;

        var tcpConnections = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpConnections();
        foreach (var connection in tcpConnections)
        {
            if (connection.State == TcpState.Established && connection.LocalEndPoint.Port > 0)
            {
                var p = Process.GetProcesses().FirstOrDefault(x => x.Id == processId);
                if (p != null)
                    return connection.LocalEndPoint.Port;
            }
        }

        return null;
    }
    public static void KillProcessByName(string processName)
    {
        var processes = Process.GetProcessesByName(processName);

        if (processes.Length == 0)
        {
            Console.WriteLine($"No process named {processName} found.");
            return;
        }

        foreach (var process in processes)
        {
            try
            {
                process.Kill();
                process.WaitForExit(); // Ensures process is fully terminated
                Console.WriteLine($"Killed process {process.ProcessName} (PID: {process.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not kill {process.ProcessName} (PID: {process.Id}): {ex.Message}");
            }
        }
        }
        private void CheckProcess(object state)
    {
        bool isRunning = Process.GetProcesses()
                                .Any(p => p.ProcessName.Equals(_processName, StringComparison.OrdinalIgnoreCase));

        if (isRunning) { 
        _onRunning?.Invoke();
    }
        else{
            _onNotRunning?.Invoke();
}
    }

    public void Stop()
    {
        _timer.Dispose();
    }
}
