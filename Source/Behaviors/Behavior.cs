// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System.Runtime.Serialization;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Runtime.Registry;
using VRBuilder.Utils;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Inherit from this abstract class when creating your own behaviors.
    /// </summary>
    /// <typeparam name="TData">The type of the behavior's data.</typeparam>
    [DataContract(IsReference = true)]
    public abstract class Behavior<TData> : Entity<TData>, IBehavior where TData : class, IBehaviorData, new()
    {
        /// <inheritdoc />
        IBehaviorData IDataOwner<IBehaviorData>.Data
        {
            get { return Data; }
        }

        /// <summary>
        /// Creates a new behavior and subscribes to lifecycle logging when enabled in the runtime configuration.
        /// </summary>
        protected Behavior()
        {
            if (ServiceRegistry.Get<IRuntimeService>().LifeCycleLogging.LogBehaviors)
            {
                LifeCycle.StageChanged += (sender, args) => { ForwardingLogger.LogFormat($"{ConsoleUtils.GetTabs(2)}<b>Behavior</b> <i>'{Data.Name} ({GetType().Name})'</i> is <b>{LifeCycle.Stage}</b>.\n"); };
            }
        }

    }
}
