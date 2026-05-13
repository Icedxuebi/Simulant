using Simulant.Core.Entity;
using Simulant.Game;
using System;
using System.Collections.Generic;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimEntityManager // 后续注意线程安全！
    {
        private readonly EntitySpawner _spawner;
        private readonly List<Character> _fakePlayers = new List<Character>();
        private readonly List<Character> _dummies = new List<Character>();
        private readonly List<EntityBase> _entities = new List<EntityBase>();

        public IReadOnlyList<Character> FakePlayers => _fakePlayers;

        public Character SpawnPlayer(byte job)
        {
            var player = _spawner.SpawnPlayer(job);
            _fakePlayers.Add(player);
            _entities.Add(player);
            return player;
        }

        public Character SpawnBNpc(uint bNpcBaseId, uint bNpcNameId = 0)
        {
            var bnpc = _spawner.SpawnBNpc(bNpcBaseId, bNpcNameId);
            _entities.Add(bnpc);
            return bnpc;
        }

        public void Delete(EntityBase entity)
        {
            _spawner.Delete(entity);
            _entities.Remove(entity);
            // ...
        }

        internal void CreateDummies(int dummyCount)
        {
            for (int i = 0; i < dummyCount; i++)
            {
                var dummy = _spawner.SpawnBNpc(9020);
                _dummies.Add(dummy);
            }
        }

        /// <summary> 自动获取首个空闲的假实体。</summary>
        public Character IdleDummy()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            foreach (var entity in _entities.ToArray())
            {
                try
                {
                    Delete(entity);
                }
                catch
                {
                    // ...
                }
            }

            _fakePlayers.Clear();
            _entities.Clear();
            _dummies.Clear();
        }
    }
}
