using Job = Simulant.Game.FFCS.Client.Game.Character.Job;
using Simulant.Simulation.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simulant.Core.Entity;
using Simulant.Core;

namespace Simulant.Simulation
{
    public abstract class SimLogicBase
    {
        private readonly PluginHost _host;
        private readonly SimPresetBase _preset;

        internal SimEntityManager EntityManager { get; set; }

        // 如果修改构造函数传参，需要同步修改 SimSession 构造函数中的 CreateInstance 调用参数
        public SimLogicBase(PluginHost host, SimPresetBase preset)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _preset = preset ?? throw new ArgumentNullException(nameof(preset));
        }

        public void Start()
        { 
            LoadOptionsFrom(_preset.Options);
            OnStart();
        }

        public void Stop()
        {
            OnStop();
            EntityManager.Clear();
        }

        protected abstract void OnStart();
        protected abstract void OnStop();

        internal void LoadOptionsFrom(IEnumerable<SimOptionBase> options)
        {
            // 传入的所有 UI 选项构建为字典，过滤掉非 SimOption<T> 子类（没有 PropertyName）
            var dict = (options ?? Enumerable.Empty<SimOptionBase>())
                .Where(o => !string.IsNullOrEmpty(o.PropertyName))
                .ToDictionary(o => o.PropertyName);

            // 当前 SimLogicBase 子类中所有标记 SimOptionAttribute 的属性
            var props = GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(p => p.GetCustomAttribute<SimOptionAttribute>() != null);

            foreach (var prop in props)
            {
                if (!dict.TryGetValue(prop.Name, out var option))
                    throw new InvalidOperationException($"配置选项 {GetType().Name} 的标记属性 {prop.Name} 在界面中未找到对应选项。");

                option.ApplyTo(this);
            }
        }

        #region Party / Roles

        protected List<Character> Party => EntityManager.Party;
        protected virtual Character MT => EntityManager.GetPartyMember(1);
        protected virtual Character ST => EntityManager.GetPartyMember(2);
        protected virtual Character H1 => EntityManager.GetPartyMember(3);
        protected virtual Character H2 => EntityManager.GetPartyMember(4);
        protected virtual Character D1 => EntityManager.GetPartyMember(5);
        protected virtual Character D2 => EntityManager.GetPartyMember(6);
        protected virtual Character D3 => EntityManager.GetPartyMember(7);
        protected virtual Character D4 => EntityManager.GetPartyMember(8);

        protected enum JobRole
        {
            None, Tank, Healer, Melee, PhysicalRanged, MagicalRanged
        }

        protected Dictionary<JobRole, List<Job>> JobsByRole = new Dictionary<JobRole, List<Job>>()
        {
            [JobRole.None] = new List<Job>(),
            [JobRole.Tank] = new List<Job> { Job.PLD, Job.WAR, Job.DRK, Job.GNB },
            [JobRole.Healer] = new List<Job> { Job.WHM, Job.AST, Job.SGE, Job.SCH },
            [JobRole.Melee] = new List<Job> { Job.MNK, Job.DRG, Job.NIN, Job.RPR, Job.VPR },
            [JobRole.PhysicalRanged] = new List<Job> { Job.BRD, Job.MCH, Job.DNC },
            [JobRole.MagicalRanged] = new List<Job> { Job.BLM, Job.SMN, Job.RDM, Job.PCT },
        };

        protected struct AutoRole
        {
            internal JobRole Role;
            internal Job Job;
            /// <summary>
            /// 自动分配职能时，从对应职能的职业列表中选择职业的顺序。
            /// </summary>
            internal bool IsAscending;
            internal bool IsFixedJob;

            public static AutoRole Asc(JobRole role)
                => new AutoRole { Role = role, IsAscending = true, IsFixedJob = false };

            public static AutoRole Desc(JobRole role)
                => new AutoRole { Role = role, IsAscending = false, IsFixedJob = false };

            public static AutoRole Fixed(Job job)
                => new AutoRole { Role = JobRole.None, Job = job, IsFixedJob = true };
        }

        /// <summary>
        /// 默认为 MT ST H1 H2 D1 D2 D3 D4 的常规小队配置，子类可重写以实现不同的队伍构成和职业分配顺序。
        /// </summary>
        protected virtual List<AutoRole> PartyRoles() => new List<AutoRole>()
        {
            AutoRole.Asc(JobRole.Tank),
            AutoRole.Desc(JobRole.Tank),
            AutoRole.Asc(JobRole.Healer),
            AutoRole.Desc(JobRole.Healer),
            AutoRole.Asc(JobRole.Melee),
            AutoRole.Desc(JobRole.Melee),
            AutoRole.Asc(JobRole.PhysicalRanged),
            AutoRole.Asc(JobRole.MagicalRanged),
        };

        protected void GeneratePartyMembers(int myPartyIdx, Job myJob)
        {
            var partyRoles = PartyRoles();

            var isOutOfRange = myPartyIdx < 1 || myPartyIdx > partyRoles.Count;
            if (isOutOfRange)
                throw new ArgumentOutOfRangeException(nameof(myPartyIdx), $"小队职能序号必须在小队范围内（1-{partyRoles.Count}）。");

            var usedJobs = new HashSet<Job> { myJob };

            List<Job?> jobs = new List<Job?>();

            for (int partyIdx = 1; partyIdx <= partyRoles.Count; partyIdx++)
            {
                if (partyIdx == myPartyIdx)
                {
                    jobs.Add(null);
                    continue;
                }

                var autoRole = partyRoles[partyIdx - 1];

                if (autoRole.IsFixedJob)
                {
                    jobs.Add(autoRole.Job);
                    usedJobs.Add(autoRole.Job);
                    continue;
                }

                var role = autoRole.Role;
                var jobList = JobsByRole[role].Cast<Job?>().ToList();

                Job? job = null;

                if (autoRole.IsAscending)
                    job = jobList.FirstOrDefault(j => j.HasValue && !usedJobs.Contains(j.Value))
                        ?? jobList.FirstOrDefault();
                else
                    job = jobList.LastOrDefault(j => j.HasValue && !usedJobs.Contains(j.Value))
                        ?? jobList.LastOrDefault();

                _ = job ?? throw new InvalidOperationException($"无法为队员 # {partyIdx} 的职能 {role} 分配合适的职业。");

                jobs.Add(job);
                usedJobs.Add(job.Value);
            }

            foreach (var job in jobs)
            {
                Character player;
                if (job.HasValue)
                    player = EntityManager.SpawnPlayer(job.Value);
                else
                    player = _host.EntityProvider.GetMyself();

                Party.Add(player);
            }
        }

        #endregion Party / Roles
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SimOptionAttribute : Attribute
    {
    }
}
