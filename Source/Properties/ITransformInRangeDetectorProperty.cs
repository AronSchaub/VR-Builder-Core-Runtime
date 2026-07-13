// copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
#if UNITY_6000_0_OR_NEWER
using UnityEngine;
#elif GODOT
using Godot;
#endif

using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Interface that allows a Property to detect if a <see cref="ISceneObject"/> is in range.
    /// </summary>
    public interface ITransformInRangeDetectorProperty : ISceneObjectProperty
    {
        /// <summary>
        /// The detection radius in world units.
        /// </summary>
        float DetectionRange { get; set; }

        /// <summary>
        /// Emitted when the tracked transform enters range.
        /// </summary>
        event Action<RangeEventArgs> EnteredRangeAction;

        /// <summary>
        /// Emitted when the tracked transform exits range.
        /// </summary>
        event Action<RangeEventArgs> ExitedRangeAction;

        /// <summary>
        /// Returns true if the tracked <see cref="ISceneObject"/> is within <see cref="DetectionRange"/>.
        /// </summary>
        bool IsTargetInsideRange();

        /// <summary>
        /// Sets which <see cref="ISceneObject"/> to track for range detection.
        /// </summary>
        /// <param name="transformToBeTracked">The <see cref="ISceneObject"/> to track.</param>
        void SetTrackedTransform(ISceneObject transformToBeTracked);

        /// <summary>
        /// Snaps the detector's position to the tracked <see cref="ISceneObject"/>'s current position.
        /// </summary>
        void ForceMoveToTracked();
    }

    /// <summary>
    /// Event arguments for range enter and exit events.
    /// </summary>
#if UNITY_6000_0_OR_NEWER
    public class RangeEventArgs : EventArgs
    {
        /// <summary>
        /// The tracked object's GameObject.
        /// </summary>
        public readonly GameObject TrackedTransform;

        public RangeEventArgs(GameObject trackedTransform)
        {
            TrackedTransform = trackedTransform;
        }
    }
#elif GODOT
    public partial class RangeEventArgs : GodotObject
    {
        /// <summary>
        /// The tracked object's Node.
        /// </summary>
        public readonly Node TrackedTransform;

        public RangeEventArgs(Node trackedTransform)
        {
            TrackedTransform = trackedTransform;
        }
    }
#endif
}