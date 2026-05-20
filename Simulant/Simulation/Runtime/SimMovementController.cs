using Simulant.Core.Entity;
using Simulant.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Simulant.Simulation.Runtime
{
    internal sealed class SimMovementController : IDisposable
    {
        private readonly object _lock = new object();
        private readonly List<SimCharacter> _characters = new List<SimCharacter>();

        private readonly Thread _thread;
        private volatile bool _running;

        private const int TickIntervalMs = 16;
        private const float StopDistance = 0.03f;

        public SimMovementController()
        {
            _running = true;

            _thread = new Thread(Loop)
            {
                IsBackground = true,
                Name = "Simulant Movement"
            };

            _thread.Start();
        }

        public void Register(SimCharacter character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            lock (_lock)
            {
                if (!_characters.Contains(character))
                    _characters.Add(character);
            }
        }

        public void RegisterRange(IEnumerable<SimCharacter> characters)
        {
            if (characters == null)
                throw new ArgumentNullException(nameof(characters));

            foreach (var character in characters)
                Register(character);
        }

        public void Unregister(SimCharacter character)
        {
            if (character == null)
                return;

            lock (_lock)
            {
                _characters.Remove(character);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _characters.Clear();
            }
        }

        public void SetTarget(SimCharacter character, Vector2 targetPos)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            character.TargetPos = targetPos;
        }

        public void SetTarget(SimCharacter character, Vector2 targetPos, float speed)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            if (speed < 0)
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed cannot be negative.");

            character.TargetPos = targetPos;
            character.Speed = speed;
        }

        private void Loop()
        {
            var last = DateTime.UtcNow;

            while (_running)
            {
                try
                {
                    var now = DateTime.UtcNow;
                    var delta = (float)(now - last).TotalSeconds;
                    last = now;

                    Tick(delta);
                }
                catch
                {
                    // 避免后台线程因为单个实体异常直接退出。
                    // 如果这里要记录日志，可以把 PluginHost 注入进来。
                }

                Thread.Sleep(TickIntervalMs);
            }
        }

        private void Tick(float delta)
        {
            SimCharacter[] characters;

            lock (_lock)
            {
                characters = _characters.ToArray();
            }

            foreach (var character in characters)
                MoveCharacter(character, delta);
        }

        private static void MoveCharacter(SimCharacter character, float delta)
        {
            if (character == null)
                return;

            if (character.Native.IsNull())
                return;
            /*
            var current = character.Pos;
            var target = character.TargetPos;
            var diff = target - current;
            var distance = diff?.Length();

            if (distance <= StopDistance)
            {
                character.Pos = target.Value;
                return;
            }

            var speed = character.Speed;
            if (speed <= 0)
                return;

            var step = speed * delta;
            if (step >= distance)
            {
                character.Pos = target;
                return;
            }

            character.Pos = current + diff / distance * step;
            */
        }

        public void Dispose()
        {
            _running = false;

            if (_thread != null && _thread.IsAlive)
                _thread.Join(500);
        }
    }
}