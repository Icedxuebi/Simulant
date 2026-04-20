using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Simulant.Core
{
    public enum LogType { Error, Warning, Sim, Call, Verbose }

    internal sealed class PluginLog
    {
        private const int MaxCountPerLevel = 30000;

        private readonly Dictionary<LogType, Queue<LogItem>> _log =
            Enum.GetValues(typeof(LogType))
                .Cast<LogType>()
                .ToDictionary(level => level, level => new Queue<LogItem>());

        internal void Clear()
        {
            lock (_log)
            {
                foreach (var pair in _log)
                {
                    pair.Value.Clear();

                }
            }
        }

        internal void Add(LogType type, string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            var item = new LogItem
            {
                Timestamp = DateTime.Now,
                Type = type,
                Message = message
            };

            var queue = _log[type];
            lock (queue)
            {
                queue.Enqueue(item);
                if (queue.Count > MaxCountPerLevel)
                {
                    queue.Dequeue();
                }
            }
        }

        internal Dictionary<LogType, List<LogItem>> Snapshot()
        {
            var result = new Dictionary<LogType, List<LogItem>>();

            foreach (var pair in _log)
            {
                var queue = pair.Value;
                lock (queue)
                {
                    result[pair.Key] = queue.ToList();
                }
            }

            return result;
        }
    }

    internal sealed class LogItem
    {
        public DateTime Timestamp { get; set; }
        public LogType Type { get; set; }
        public string Message { get; set; }
        public string TypeDescription => _typeToDesc.TryGetValue(Type, out var desc) ? desc : Type.ToString();

        private Dictionary<LogType, string> _typeToDesc = new Dictionary<LogType, string>
        {
            { LogType.Error,    "错误" },
            { LogType.Warning,  "警告" },
            { LogType.Sim,      "模拟" },
            { LogType.Call,     "调用" },
            { LogType.Verbose,  "详细" },
        };
    }
}