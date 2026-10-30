// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Runtime.Serialization;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.RestrictiveEnvironment;
using VRBuilder.Core.Runtime.Registry;
using VRBuilder.Core.Utils;
using VRBuilder.Utils;

namespace VRBuilder.Core.Conditions
{
    /// <summary>
    /// An implementation of <see cref="ICondition"/>. Use it as the base class for your custom conditions.
    /// </summary>
    [DataContract(IsReference = true)]
    public abstract class Condition<TData> : CompletableEntity<TData>, ICondition, ILockablePropertiesProvider where TData : class, IConditionData, new()
    {
        /// <summary>
        /// Creates a new condition and subscribes to lifecycle logging when enabled in the runtime configuration.
        /// </summary>
        protected Condition()
        {
            if (ServiceRegistry.Get<IRuntimeService>().LifeCycleLogging.LogConditions)
            {
                LifeCycle.StageChanged += (sender, args) => { ForwardingLogger.LogFormat($"{ConsoleUtils.GetTabs(2)}<b>Condition</b> <i>'{Data.Name} ({GetType().Name})'</i> is <b>{LifeCycle.Stage}</b>.\n"); };
            }
        }

        /// <inheritdoc />
        IConditionData IDataOwner<IConditionData>.Data
        {
            get { return Data; }
        }

        /// <inheritdoc />
        public virtual IEnumerable<LockablePropertyData> GetLockableProperties()
        {
            return PropertyReflectionHelper.ExtractLockablePropertiesFromCondition(Data);
        }
    }
}