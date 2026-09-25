// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.ProcessRunning;
using VRBuilder.Core.Runtime.Registry;
#if UNITY_6000_0_OR_NEWER
using System.Linq;
#endif

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// This behavior sets the next chapter to an arbitrary chapter and immediately aborts the current chapter.
    /// </summary>
    [DataContract(IsReference = true)]
    public class GoToChapterBehavior : Behavior<GoToChapterBehavior.EntityData>
    {
        /// <summary>
        /// Creates a new <see cref="GoToChapterBehavior"/> without a target chapter.
        /// </summary>
        [JsonConstructor]
        public GoToChapterBehavior() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Creates a behavior that jumps to the chapter identified by <paramref name="chapterGuid"/>.
        /// </summary>
        /// <param name="chapterGuid">Unique id of the chapter to jump to.</param>
        public GoToChapterBehavior(Guid chapterGuid)
        {
            Data.ChapterGuid = chapterGuid;
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }

        /// <summary>
        /// Behavior data.
        /// </summary>
        [DisplayName("Go to Chapter")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Unique id of the chapter to jump to. The current chapter is aborted immediately.
            /// </summary>
            [DataMember]
            [DisplayName("Chapter")]
            [DisplayTooltip("Chapter to jump to. The current chapter is aborted immediately.")]
            public Guid ChapterGuid { get; set; }

            /// <inheritdoc />
            public Metadata Metadata { get; set; }

            /// <inheritdoc />
            [IgnoreDataMember]
            public string Name => "Go to Chapter";
        }

        private class ActivatingProcess : StageProcess<EntityData>
        {
            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                if (Data.ChapterGuid == null || Data.ChapterGuid == Guid.Empty)
                {
                    return;
                }

#if UNITY_6000_0_OR_NEWER
                var processRunner = ServiceRegistry.Get<IProcessRunner>();
                IChapter chapter = processRunner.CurrentProcess.Data.Chapters.FirstOrDefault(chapter => chapter.ChapterMetadata.Guid == Data.ChapterGuid);

                if (chapter != null)
                {
                    processRunner.SetNextChapter(chapter);
                }

                processRunner.CurrentProcess.Data.Current?.LifeCycle.Abort();
#endif
            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                yield return null;
            }

            /// <inheritdoc />
            public override void End()
            {
            }

            /// <inheritdoc />
            public override void FastForward()
            {
            }
        }
    }
}