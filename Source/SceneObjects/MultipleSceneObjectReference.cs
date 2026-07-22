// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using VRBuilder.Core.Configuration;
using VRBuilder.Core.Runtime.Registry;

namespace VRBuilder.Core.SceneObjects
{
    /// <summary>
    /// Step inspector reference to multiple <see cref="ISceneObject"/>s.
    /// </summary>
    [DataContract(IsReference = true)]
    public class MultipleSceneObjectReference : MultipleSceneReference<ISceneObject>
    {
        /// <inheritdoc />
        protected override IEnumerable<ISceneObject> DetermineValue(IEnumerable<ISceneObject> cachedValue)
        {
            if (!ServiceRegistry.Has<IRuntimeService>() || IsEmpty())
            {
                return new List<ISceneObject>();
            }

            IEnumerable<ISceneObject> value = cachedValue;

            // If value exists, return it.
            if (value != null)
            {
                return value;
            }

            value = new List<ISceneObject>();

            foreach (Guid guid in Guids)
            {
                value = value.Concat(ServiceRegistry.Get<ISceneObjectRegistry>().GetObjects(guid)).Distinct();
            }

            return value;
        }

        public MultipleSceneObjectReference() : base() { }
        public MultipleSceneObjectReference(Guid guid) : base(guid) { }
        public MultipleSceneObjectReference(IEnumerable<Guid> guids) : base(guids) { }
    }
}
