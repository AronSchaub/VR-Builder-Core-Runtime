// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

#if UNITY_6000_0_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
using VRBuilder.Core.Godot.Attributes;
using System.Linq;
#endif

using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    [RequireComponent(typeof(ProcessSceneObject))]
#if UNITY_6000_0_OR_NEWER
    public abstract class ProcessSceneObjectProperty : MonoBehaviour, ISceneObjectProperty
#elif GODOT
    public abstract partial class ProcessSceneObjectProperty : Node3D, ISceneObjectProperty
#endif
    {
        private ISceneObject sceneObject;

        public ISceneObject SceneObject
        {
            get
            {
#if UNITY_6000_0_OR_NEWER
                if (sceneObject == null) sceneObject = GetComponent<ISceneObject>();
#elif GODOT
                sceneObject ??= (ISceneObject)GetParent().GetChildren().First(c => c.GetType() == typeof(ProcessSceneObject));
#endif

                return sceneObject;
            }
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void Reset()
        {
            this.AddProcessPropertyExtensions();
        }

        public override string ToString()
        {
#if UNITY_6000_0_OR_NEWER
            return SceneObject.GameObject.name;
#elif GODOT
            return SceneObject.GameObject.Name;
#endif
        }
    }
}
