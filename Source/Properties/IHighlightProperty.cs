// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
#if UNITY_6000_0_OR_NEWER
using UnityEngine;
using UnityEngine.Events;
#elif GODOT
using Godot;
#endif
using VRBuilder.Core.Primitives;

namespace VRBuilder.Core.Properties
{
    /// <summary>
    /// Interface for scene objects that can be highlighted.
    /// </summary>
    public interface IHighlightProperty : ISceneObjectProperty
    {
        /// <summary>
        /// Emitted when the object gets highlighted.
        /// </summary>
        event Action<HighlightPropertyEventArgs> HighlightStartedAction;

        /// <summary>
        /// Emitted when the object gets unhighlighted.
        /// </summary>
        event Action<HighlightPropertyEventArgs> HighlightEndedAction;

        /// <summary>
        /// Is object currently highlighted.
        /// </summary>
        bool IsHighlighted { get; }

        /// <summary>
        /// Highlight this object and use <paramref name="highlightColor"/>.
        /// </summary>
        /// <param name="highlightColor">Color to use for highlighting.</param>
        void Highlight(IColor highlightColor);

        /// <summary>
        /// Disable highlight.
        /// </summary>
        void Unhighlight();
    }

#if UNITY_6000_0_OR_NEWER
    public class HighlightPropertyEventArgs : EventArgs
#elif GODOT
    public partial class HighlightPropertyEventArgs : GodotObject
#endif
    {
        public readonly IColor? HighlightColor;

        public HighlightPropertyEventArgs(IColor? highlightColor)
        {
            HighlightColor = highlightColor;
        }
    }
}