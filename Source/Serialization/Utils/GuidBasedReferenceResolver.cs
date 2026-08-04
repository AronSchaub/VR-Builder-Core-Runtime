// Copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VRBuilder.Core.Serialization.V5
{
    /// <summary>
    /// Replaces Newtonsoft.Json's incremental integer <c>$id</c> values with stable, semantic
    /// identifiers derived from the GUIDs already present on process, chapter and step metadata.
    ///
    /// Used together with <see cref="PreserveReferencesHandling.Objects"/>. Newtonsoft emits
    /// <c>$id</c> for every object in that mode; the resolver returns a GUID-based identifier for
    /// the entities that can actually be shared (a step referenced from a chapter's FirstStep/Steps,
    /// a chapter referenced via FirstChapter, a step ref appearing twice) and <c>null</c> for
    /// everything else. Those objects occur exactly once, are never referenced, and their null ids
    /// are stripped by <see cref="NewtonsoftJsonProcessSerializerV5"/> after serialization, so the
    /// output stays stable across saves instead of renumbering. Content-derived identifiers are
    /// deliberately not used, because two distinct objects with the same identifier would be merged
    /// into one on load.
    /// </summary>
    internal class GuidBasedReferenceResolver : IReferenceResolver
    {
        private readonly Dictionary<object, string> objectToReference = new Dictionary<object, string>(ReferenceEqualityComparer.Instance);
        private readonly Dictionary<string, object> referenceToObject = new Dictionary<string, object>();

        public string? GetReference(object context, object? value)
        {
            if (value == null)
            {
                return null;
            }

            if (objectToReference.TryGetValue(value, out string existing))
            {
                return existing;
            }

            // Only entities that can appear more than once in the serialized graph need an
            // identifier. StepRef must be matched before IStep because it implements IStep.
            string? reference = value switch
            {
                StepRef stepRef => $"stepref/{stepRef.StepMetadata.Guid:N}",
                IStep step => $"step/{step.StepMetadata.Guid:N}",
                IChapter chapter => $"chapter/{chapter.ChapterMetadata.Guid:N}",
                IProcess process => $"process/{process.ProcessMetadata.Guid:N}",
                _ => null
            };

            if (reference != null)
            {
                objectToReference[value] = reference;
                referenceToObject[reference] = value;
            }

            return reference;
        }

        public bool IsReferenced(object context, object value)
        {
            return value != null && objectToReference.ContainsKey(value);
        }

        public void AddReference(object context, string reference, object value)
        {
            if (reference != null && value != null)
            {
                referenceToObject[reference] = value;
                objectToReference[value] = reference;
            }
        }

        public object ResolveReference(object context, string reference)
        {
            if (reference != null && referenceToObject.TryGetValue(reference, out object value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Reference equality comparer for dictionary lookups.
        /// Ensures we use reference identity rather than object.Equals().
        /// </summary>
        private sealed class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

            public new bool Equals(object? x, object? y)
            {
                return ReferenceEquals(x, y);
            }

            public int GetHashCode(object obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }
    }
}