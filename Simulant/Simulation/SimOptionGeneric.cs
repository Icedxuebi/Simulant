using Simulant.Simulation.Options;
using System;
using System.Reflection;

namespace Simulant.Simulation
{
    public abstract class SimOption<T> : SimOptionBase
    {
        protected T _default;

        internal abstract T GetValue();

        internal override void ApplyTo(object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (string.IsNullOrEmpty(PropertyName))
                return;

            var property = target.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Property '{PropertyName}' was not found on {target.GetType().FullName}.");

            if (!property.CanWrite)
                throw new InvalidOperationException($"Property '{PropertyName}' on {target.GetType().FullName} is not writable.");

            if (property.PropertyType != typeof(T))
                throw new InvalidOperationException($"Option type mismatch: Option type = SimOption<{typeof(T).Name}>, {PropertyName} type = {property.PropertyType.Name}.");

            var value = GetValue();

            if (typeof(T).IsEnum)
                value = value.TryResolveRandom();

            property.SetValue(target, value, null);
        }
    }

}
