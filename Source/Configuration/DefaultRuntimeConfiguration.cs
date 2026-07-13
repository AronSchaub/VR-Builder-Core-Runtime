// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.Properties;
using VRBuilder.Core.SceneObjects;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Process runtime configuration which is used if no other was implemented.
    /// </summary>
    public class DefaultRuntimeConfiguration : BaseRuntimeConfiguration
    {
        private IAudioPlayer audioPlayer;
        private ISceneObjectManager sceneObjectManager;

        /// <summary>
        /// Default mode which white lists everything.
        /// </summary>
        public static readonly IMode DefaultMode = new Mode("Default", new WhitelistTypeRule<IOptional>());

        public DefaultRuntimeConfiguration()
        {
            Modes = new BaseModeHandler(new List<IMode> { DefaultMode });
        }

        /// <inheritdoc />
        [Obsolete("Use User property instead.")]
        public override UserSceneObject LocalUser
        {
            get
            {
                UserSceneObject user = User as UserSceneObject;

                if (user == null)
                {
                    throw new Exception("Could not find a UserSceneObject in the scene.");
                }

                return user;
            }
        }

        /// <inheritdoc />
        public override IXRRigTransform User
        {
            get
            {
                UserSceneObject user = GameObject.FindObjectsByType<UserSceneObject>(FindObjectsSortMode.None).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("Could not find a user rig in the scene.");
                }

                return user;
            }
        }

        /// <inheritdoc />
        public override AudioSource InstructionPlayer
        {
            get
            {
                //TODO: after moving to Core/Runtime fix:
                // return AudioPlayer.FallbackAudioSource.ToUnity();
                return null;
            }
        }

        /// <inheritdoc />
        public override IAudioPlayer AudioPlayer
        {
            get
            {
                //TODO: after moving to Core/Runtime fix:
                // if (audioPlayer == null)
                // {
                //     audioPlayer = new DefaultAudioPlayer();
                // }

                return audioPlayer;
            }
        }

        /// <inheritdoc />
        public override ISceneObjectManager SceneObjectManager
        {
            get
            {
                if (sceneObjectManager == null)
                {
                    sceneObjectManager = new DefaultSceneObjectManager();
                }

                return sceneObjectManager;
            }
        }

        /// <inheritdoc />
        public override IEnumerable<IXRRigTransform> UserTransforms
        {
            get
            {
                if (User != null)
                {
                    return new List<IXRRigTransform>() { User };
                }
                else
                {
                    return new List<IXRRigTransform>();
                }
            }
        }
    }
}
