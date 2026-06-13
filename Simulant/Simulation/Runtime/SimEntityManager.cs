using Simulant.Core;
using Simulant.Core.Entity;
using Simulant.Game;
using System;
using System.Collections.Generic;
using Job = Simulant.Game.FFCS.Client.Game.Character.Job;

namespace Simulant.Simulation.Runtime
{
    public sealed class SimEntityManager
    {
        private readonly EntitySpawner _spawner;
        private readonly List<EntityBase> _localEntities = new List<EntityBase>();
        internal List<Character> Party = new List<Character>();

        public SimEntityManager(EntitySpawner spawner)
        { 
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
        }

        public Character SpawnPlayer(Job job)
        {
            var player = _spawner.SpawnPlayer(job);
            lock (_localEntities)
            {
                _localEntities.Add(player);
            }
            return player;
        }

        public Character SpawnBNpc(uint bNpcBaseId, uint bNpcNameId = 0, byte level = 0)
        {
            var bnpc = _spawner.SpawnBNpc(bNpcBaseId, bNpcNameId, level);
            lock (_localEntities)
            {
                _localEntities.Add(bnpc);
            }
            return bnpc;
        }

        public void Delete(EntityBase entity)
        {
            _spawner.Delete(entity);
            lock (_localEntities)
            {
                _localEntities.Remove(entity);
            }
            // ...
        }

        public void Clear()
        {
            lock (_localEntities)
            {
                foreach (var entity in _localEntities.ToArray())
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
                _localEntities.Clear();
            }
            Party.Clear();
        }

        internal Character GetPartyMember(int partyIndex)
        { 
            if (partyIndex < 1 || partyIndex > Party.Count)
                throw new ArgumentOutOfRangeException(nameof(partyIndex), $"小队成员索引必须在范围内（1-{Party.Count}）。");

            return Party[partyIndex - 1];
        }
    }
}
