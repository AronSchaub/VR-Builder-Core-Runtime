// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VRBuilder.Core.Behaviors;
using VRBuilder.Core.EntityOwners;
using VRBuilder.Core.Serialization.NewtonsoftJson;
using VRBuilder.Core.Serialization.V5;

namespace VRBuilder.Core.Serialization
{
    /// <summary>
    /// Version 5 of the Newtonsoft JSON process serializer.
    /// Uses stable GUID-based reference IDs and clean type names without assembly qualification.
    /// </summary>
    public class NewtonsoftJsonProcessSerializerV5 : NewtonsoftJsonProcessSerializer
    {
        /// <summary>
        /// JSON serializer settings for V5 format using the GUID-based reference resolver and V5 type binder.
        /// Cached because settings objects are not cheap to build and the resolver/provider are stateless.
        /// </summary>
        private static readonly JsonSerializerSettings V5Settings = CreateV5Settings();

        /// <inheritdoc/>
        public override string Name { get; } = "Newtonsoft Json Importer v5";

        /// <inheritdoc/>
        protected override int Version { get; } = 5;

        private static JsonSerializerSettings CreateV5Settings()
        {
            return new JsonSerializerSettings
            {
                Converters = ProcessSerializerSettings.Converters,
                // Only emit $id/$ref for objects that actually appear more than once in the
                // serialized graph (steps, chapters, step refs). Everything else stays inline,
                // so the JSON is smaller and stable across saves.
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceResolverProvider = () => new GuidBasedReferenceResolver(),
                Formatting = Formatting.Indented,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                SerializationBinder = new ProcessSerializationBinderV5(),
                // Auto writes $type only where the runtime type differs from the declared type
                // (polymorphic steps/behaviors/conditions/boxed values). Collection members declared
                // as e.g. List<IStep> serialize as plain arrays without $type or $values wrappers.
                TypeNameHandling = TypeNameHandling.Auto
            };
        }

        /// <inheritdoc/>
        public override IProcess ProcessFromByteArray(byte[] data)
        {
            string stringData = new UTF8Encoding().GetString(data);
            JObject dataObject = JsonConvert.DeserializeObject<JObject>(stringData, ProcessSerializerSettings);

            // Check if process was serialized with a previous version.
            int version = dataObject.GetValue("$serializerVersion").ToObject<int>();
            if (version == 1)
            {
                return base.ProcessFromByteArray(data);
            }

            if (version == 2)
            {
                return new ImprovedNewtonsoftJsonProcessSerializer().ProcessFromByteArray(data);
            }

            if (version == 3)
            {
                return new NewtonsoftJsonProcessSerializerV3().ProcessFromByteArray(data);
            }

            if (version == 4)
            {
                return new NewtonsoftJsonProcessSerializerV4().ProcessFromByteArray(data);
            }

            ProcessWrapper wrapper = Deserialize<ProcessWrapper>(data, V5Settings);
            return wrapper.GetProcess();
        }

        /// <inheritdoc/>
        public override byte[] ProcessToByteArray(IProcess process)
        {
            ProcessWrapper wrapper = new ProcessWrapper(process);
            byte[] bytes = null;

            try
            {
                JObject jObject = JObject.FromObject(wrapper, JsonSerializer.Create(V5Settings));
                RemoveNullReferenceIds(jObject);
                jObject.Add("$serializerVersion", Version);
                bytes = new UTF8Encoding().GetBytes(jObject.ToString());
            }
            catch (Exception ex)
            {
                ForwardingLogger.LogError(ex.Message);
            }

            // This line is required to undo the changes applied to the process.
            wrapper.GetProcess();

            return bytes;
        }

        /// <inheritdoc/>
        public override IChapter ChapterFromByteArray(byte[] data)
        {
            string stringData = new UTF8Encoding().GetString(data);
            JObject dataObject = JsonConvert.DeserializeObject<JObject>(stringData, ProcessSerializerSettings);

            // Check if chapter was serialized with a previous version.
            int version = dataObject.GetValue("$serializerVersion").ToObject<int>();
            if (version == 1)
            {
                return base.ChapterFromByteArray(data);
            }

            if (version == 2)
            {
                return new ImprovedNewtonsoftJsonProcessSerializer().ChapterFromByteArray(data);
            }

            if (version == 3)
            {
                return new NewtonsoftJsonProcessSerializerV3().ChapterFromByteArray(data);
            }

            if (version == 4)
            {
                return new NewtonsoftJsonProcessSerializerV4().ChapterFromByteArray(data);
            }

            ChapterWrapper wrapper = Deserialize<ChapterWrapper>(data, V5Settings);
            return wrapper.GetChapter();
        }

        /// <inheritdoc/>
        public override byte[] ChapterToByteArray(IChapter chapter)
        {
            ChapterWrapper wrapper = new ChapterWrapper(chapter);
            byte[] bytes = null;

            try
            {
                JObject jObject = JObject.FromObject(wrapper, JsonSerializer.Create(V5Settings));
                RemoveNullReferenceIds(jObject);
                jObject.Add("$serializerVersion", Version);
                bytes = new UTF8Encoding().GetBytes(jObject.ToString());
            }
            catch (Exception ex)
            {
                ForwardingLogger.LogError(ex.Message);
            }

            // This line is required to undo the changes applied to the chapter.
            wrapper.GetChapter();

            return bytes;
        }

        /// <summary>
        /// Removes <c>"$id": null</c> properties from the serialized tree. In
        /// <see cref="PreserveReferencesHandling.Objects"/> mode Newtonsoft writes <c>$id</c> on every
        /// object; the <see cref="GuidBasedReferenceResolver"/> only assigns identifiers to entities
        /// that can be shared, so everything else gets <c>null</c>. Those objects occur exactly once,
        /// are never referenced by <c>$ref</c> and are never registered in the resolver (null is
        /// guarded), so removing the property is semantically lossless and keeps the output clean.
        /// </summary>
        private static void RemoveNullReferenceIds(JToken token)
        {
            switch (token)
            {
                case JObject obj:
                {
                    foreach (JProperty property in obj.Properties().Where(p => p.Name == "$id" && p.Value.Type == JTokenType.Null).ToList())
                    {
                        property.Remove();
                    }

                    foreach (JProperty property in obj.Properties().ToList())
                    {
                        RemoveNullReferenceIds(property.Value);
                    }

                    break;
                }
                case JArray array:
                    foreach (JToken child in array.ToList())
                    {
                        RemoveNullReferenceIds(child);
                    }

                    break;
            }
        }

        #region Base wrapper with helper methods

        private class Wrapper
        {
            protected IEnumerable<IStep> GetSteps(IChapter chapter)
            {
                List<IStep> steps = new List<IStep>();

                steps.AddRange(chapter.Data.Steps);

                IEnumerable<IChapter> subChapters = chapter.Data.Steps.SelectMany(step => step.Data.Behaviors.Data.Behaviors.Where(behavior => behavior.Data is IEntityCollectionData<IChapter>))
                    .Select(behavior => behavior.Data)
                    .Cast<IEntityCollectionData<IChapter>>()
                    .SelectMany(behavior => behavior.GetChildren());

                foreach (IChapter subChapter in subChapters)
                {
                    steps.AddRange(GetSteps(subChapter));
                }

                return steps;
            }

            protected IEnumerable<IChapter> GetSubChapters(IChapter chapter)
            {
                List<IChapter> subChapters = new List<IChapter>();

                foreach (IStep step in chapter.Data.Steps)
                {
                    foreach (IBehavior behavior in step.Data.Behaviors.Data.Behaviors)
                    {
                        if (behavior.Data is IEntityCollectionData<IChapter> data)
                        {
                            subChapters.InsertRange(0, data.GetChildren());

                            foreach (IChapter subChapter in data.GetChildren())
                            {
                                subChapters.InsertRange(0, GetSubChapters(subChapter));
                            }
                        }
                    }
                }

                return subChapters;
            }
        }

        #endregion

        #region Wrapper classes

        [Serializable]
        private class ChapterWrapper : Wrapper
        {
            [DataMember]
            public IChapter Chapter;

            [DataMember]
            public List<IStep> Steps = new List<IStep>();

            [DataMember]
            public List<IChapter> SubChapters = new List<IChapter>();

            public ChapterWrapper()
            {
            }

            public ChapterWrapper(IChapter chapter)
            {
                // Set LastSelectedStep to null, to prevent it needlessly serializing a full step tree.
                chapter.ChapterMetadata.LastSelectedStep = null;

                Steps.AddRange(GetSteps(chapter));
                SubChapters.AddRange(GetSubChapters(chapter));

                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep != null)
                        {
                            transition.Data.TargetStep = new StepRef() { StepMetadata = new StepMetadata() { Guid = transition.Data.TargetStep.StepMetadata.Guid } };
                        }
                    }
                }

                foreach (IChapter subChapter in SubChapters)
                {
                    // Set LastSelectedStep to null, to prevent it needlessly serializing a full step tree.
                    subChapter.ChapterMetadata.LastSelectedStep = null;

                    List<IStep> stepRefs = new List<IStep>();
                    foreach (IStep step in subChapter.Data.Steps)
                    {
                        IStep stepRef = new StepRef() { StepMetadata = new StepMetadata() { Guid = step.StepMetadata.Guid } };
                        stepRefs.Add(stepRef);

                        if (subChapter.Data.FirstStep != null && subChapter.Data.FirstStep.StepMetadata.Guid == stepRef.StepMetadata.Guid)
                        {
                            subChapter.Data.FirstStep = stepRef;
                        }
                    }

                    subChapter.Data.Steps = stepRefs;
                }

                Chapter = chapter;
            }

            public IChapter GetChapter()
            {
                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep == null)
                        {
                            continue;
                        }

                        StepRef stepRef = (StepRef)transition.Data.TargetStep;
                        transition.Data.TargetStep = Steps.FirstOrDefault(s => s.StepMetadata.Guid == stepRef.StepMetadata.Guid);
                    }
                }

                foreach (IChapter subChapter in SubChapters)
                {
                    List<IStep> steps = new List<IStep>();

                    foreach (IStep stepRef in subChapter.Data.Steps)
                    {
                        IStep step = Steps.FirstOrDefault(s => s.StepMetadata.Guid == stepRef.StepMetadata.Guid);
                        steps.Add(step);

                        if (subChapter.Data.FirstStep != null && subChapter.Data.FirstStep.StepMetadata.Guid == stepRef.StepMetadata.Guid)
                        {
                            subChapter.Data.FirstStep = step;
                        }
                    }

                    subChapter.Data.Steps = steps;
                }

                return Chapter;
            }
        }

        [Serializable]
        private class ProcessWrapper : Wrapper
        {
            [DataMember]
            public IProcess Process;

            [DataMember]
            public List<IStep> Steps = new List<IStep>();

            [DataMember]
            public List<IChapter> SubChapters = new List<IChapter>();

            public ProcessWrapper()
            {
            }

            public ProcessWrapper(IProcess process)
            {
                foreach (IChapter chapter in process.Data.Chapters)
                {
                    // Set LastSelectedStep to null, to prevent it needlessly serializing a full step tree.
                    chapter.ChapterMetadata.LastSelectedStep = null;

                    Steps.AddRange(GetSteps(chapter));
                    SubChapters.AddRange(GetSubChapters(chapter));
                }

                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep != null)
                        {
                            transition.Data.TargetStep = new StepRef() { StepMetadata = new StepMetadata() { Guid = transition.Data.TargetStep.StepMetadata.Guid } };
                        }
                    }
                }

                foreach (IChapter subChapter in SubChapters)
                {
                    // Set LastSelectedStep to null, to prevent it needlessly serializing a full step tree.
                    subChapter.ChapterMetadata.LastSelectedStep = null;

                    List<IStep> stepRefs = new List<IStep>();
                    foreach (IStep step in subChapter.Data.Steps)
                    {
                        IStep stepRef = new StepRef() { StepMetadata = new StepMetadata() { Guid = step.StepMetadata.Guid } };
                        stepRefs.Add(stepRef);

                        if (subChapter.Data.FirstStep != null && subChapter.Data.FirstStep.StepMetadata.Guid == stepRef.StepMetadata.Guid)
                        {
                            subChapter.Data.FirstStep = stepRef;
                        }
                    }

                    subChapter.Data.Steps = stepRefs;
                }

                Process = process;
            }

            public IProcess GetProcess()
            {
                foreach (IStep step in Steps)
                {
                    foreach (ITransition transition in step.Data.Transitions.Data.Transitions)
                    {
                        if (transition.Data.TargetStep == null)
                        {
                            continue;
                        }

                        StepRef stepRef = (StepRef)transition.Data.TargetStep;
                        transition.Data.TargetStep = Steps.FirstOrDefault(s => s.StepMetadata.Guid == stepRef.StepMetadata.Guid);
                    }
                }

                foreach (IChapter subChapter in SubChapters)
                {
                    List<IStep> steps = new List<IStep>();

                    foreach (IStep stepRef in subChapter.Data.Steps)
                    {
                        IStep step = Steps.FirstOrDefault(s => s.StepMetadata.Guid == stepRef.StepMetadata.Guid);
                        steps.Add(step);

                        if (subChapter.Data.FirstStep != null && subChapter.Data.FirstStep.StepMetadata.Guid == stepRef.StepMetadata.Guid)
                        {
                            subChapter.Data.FirstStep = step;
                        }
                    }

                    subChapter.Data.Steps = steps;
                }

                return Process;
            }
        }

        #endregion
    }
}