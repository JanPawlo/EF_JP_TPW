using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TP.ConcurrentProgramming.BusinessLogic
{
    public interface IDiagnosticLogger : IDisposable
    {
        void LogCollision(string message);
    }

    public sealed class DiagnosticLogger : IDiagnosticLogger
    {
        private static readonly Lazy<DiagnosticLogger> _instance =
            new Lazy<DiagnosticLogger>(() => new DiagnosticLogger());

        public static IDiagnosticLogger Instance => _instance.Value;

        private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private readonly AutoResetEvent _logEvent = new AutoResetEvent(false);
        private readonly Thread _loggingThread;
        private volatile bool _disposing = false;
        private readonly object _fileLock = new object();
        private readonly string _logFilePath;

        private DiagnosticLogger(string logFilePath = "collisions_log.txt")
        {
            _logFilePath = logFilePath;
            _loggingThread = new Thread(ProcessLogQueue)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            _loggingThread.Start();

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
        }

        public void LogCollision(string message)
        {
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {message}";
            _logQueue.Enqueue(logEntry);
            _logEvent.Set();
        }

        private void ProcessLogQueue()
        {
            while (!_disposing)
            {
                _logEvent.WaitOne(1000); // Wait for signal or timeout

                while (_logQueue.TryDequeue(out var logEntry))
                {
                    WriteLogEntry(logEntry);
                }
            }
            FlushRemainingLogs();
        }

        private void WriteLogEntry(string logEntry)
        {
            try
            {
                lock (_fileLock)
                {
                    File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (IOException ex)
            {
                // Requeue if IO fails (temporary issue)
                _logQueue.Enqueue(logEntry);
                Debug.WriteLine($"Failed to write log entry: {ex.Message}");
                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Critical logging error: {ex.Message}");
            }
        }

        private void FlushRemainingLogs()
        {
            while (_logQueue.TryDequeue(out var logEntry))
            {
                try
                {
                    lock (_fileLock)
                    {
                        File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to flush log entry: {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            if (_disposing) return;

            _disposing = true;
            _logEvent.Set();

            if (_loggingThread.IsAlive)
            {
                _loggingThread.Join(2000); // Wait max 2 seconds
            }

            _logEvent.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}