// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using VRBuilder.Core.Attributes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Behaviors
{
    /// <summary>
    /// Behavior that loads the specified scene, either additively or not.
    /// Loading a scene not additively interrupts the current process.
    /// </summary>
    [DataContract(IsReference = true)]
    public class LoadSceneBehavior : Behavior<LoadSceneBehavior.EntityData>
    {
        /// <summary>
        /// The data class for a load scene behavior.
        /// </summary>        
        [DataContract(IsReference = true)]
        public class EntityData : IBehaviorData
        {
            /// <summary>
            /// Asset path of the scene to load.
            /// </summary>
            [DataMember]
            [UsesSpecificProcessDrawer("SceneDropdownDrawer")]
            [DisplayName("Scene to load")]
            public string ScenePath { get; set; }

            [DataMember]
            [DisplayName("Scene Controller")]
            public SingleScenePropertyReference<ISceneProperty> SceneProperty { get; set; }
            
            /// <summary>
            /// If true, the scene will be loaded additively.
            /// </summary>
            [DataMember]
            [DisplayName("Load additively")]
            public bool LoadAdditively { get; set; }

            /// <summary>
            /// If true, the scene will be loaded asynchronously during the update cycle of the stage process.
            /// </summary>
            [DataMember]
            [DisplayName("Load asynchronously")]
            public bool LoadAsynchronously { get; set; }

            public Metadata Metadata { get; set; }

            [IgnoreDataMember]
            public string Name
            {
                get
                {
                    string sceneName = string.IsNullOrEmpty(ScenePath) ? "[NULL]" : Path.GetFileNameWithoutExtension(ScenePath);
                    string additively = LoadAdditively ? " additively" : "";

                    return $"Load scene '{sceneName}'{additively}";
                }
            }
        }

        [JsonConstructor]
        public LoadSceneBehavior()
        {
        }

        private class ActivatingProcess : StageProcess<EntityData>
        {
            bool isLoading = false;
            IAsyncCallback asyncHandler;

            public ActivatingProcess(EntityData data) : base(data)
            {
            }

            /// <inheritdoc />
            public override void Start()
            {
                if (Data.LoadAsynchronously)
                {
                    asyncHandler = Data.SceneProperty.Value.StartLoadAsync(Data.ScenePath, Data.LoadAdditively);
                }
                else
                {
                    Data.SceneProperty.Value.LoadSynchronously(Data.ScenePath, Data.LoadAdditively);
                }

            }

            /// <inheritdoc />
            public override IEnumerator Update()
            {
                if (Data.LoadAsynchronously)
                {
                    isLoading = true;

                    while (asyncHandler.isDone == false)
                    {
                        yield return null;
                    }

                    isLoading = false;
                }
                else
                {
                    yield return null;
                }
            }

            /// <inheritdoc />
            public override void End()
            {
            }


            /// <inheritdoc />
            public override void FastForward()
            {
                if (Data.LoadAsynchronously && isLoading == false)
                {
                    Data.SceneProperty.Value.LoadSynchronously(Data.ScenePath, Data.LoadAdditively);
                }
            }
        }

        /// <inheritdoc />
        public override IStageProcess GetActivatingProcess()
        {
            return new ActivatingProcess(Data);
        }
    }

    /// <summary>
    /// Exception related to load scene behavior.
    /// </summary>
    public class LoadSceneBehaviorException : Exception
    {
        public LoadSceneBehaviorException(string message) : base(message)
        {
        }
    }
}
