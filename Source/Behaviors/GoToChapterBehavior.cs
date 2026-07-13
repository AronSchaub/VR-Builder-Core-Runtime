using Newtonsoft.Json;
using System;
using System.Collections;
#if UNITY_6000_0_OR_NEWER
using System.Linq;
#endif
using System.Runtime.Serialization;
using VRBuilder.Core.Attributes;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// This behavior sets the next chapter to an arbitrary chapter and immediately aborts the current chapter.
    /// </summary>
    [DataContract(IsReference = true)]
    public class GoToChapterBehavior : Behavior<GoToChapterBehavior.EntityData>
    {
        /// <summary>
        /// Behavior data.
        /// </summary>
        [DisplayName("Go to Chapter")]
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            [DataMember]
            public Guid ChapterGuid { get; set; }

            public Metadata Metadata { get; set; }

            [IgnoreDataMember]
            public string Name => "Go to Chapter";
        }

        [JsonConstructor]
        public GoToChapterBehavior() : this(Guid.Empty)
        {
        }

        public GoToChapterBehavior(Guid chapterGuid)
        {
            Data.ChapterGuid = chapterGuid;
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
                IChapter chapter = ProcessRunner.Current.Data.Chapters.FirstOrDefault(chapter => chapter.ChapterMetadata.Guid == Data.ChapterGuid);

                if (chapter != null)
                {
                    ProcessRunner.SetNextChapter(chapter);
                }

                ProcessRunner.Current.Data.Current?.LifeCycle.Abort();
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

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }
}
