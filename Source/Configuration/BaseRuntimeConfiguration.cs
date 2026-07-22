// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier: Apache-2.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using VRBuilder.Core.Configuration.Modes;
using VRBuilder.Core.IO;
using VRBuilder.Core.Runtime.Registry;
using VRBuilder.Core.Serialization;
using VRBuilder.Core.Utils;
using VRBuilder.UI.Console;

namespace VRBuilder.Core.Configuration
{
    /// <summary>
    /// Base class for your runtime process configuration. Extend it to create your own.
    /// </summary>
#pragma warning disable 0618
    public abstract class BaseRuntimeConfiguration
    {
#pragma warning restore 0618
        /// <summary>
        /// Name of the manifest file that could be used to save process asset information.
        /// </summary>
        public static string ManifestFileName => "ProcessManifest";

        private ISceneService sceneService;

        /// <inheritdoc />
        public IProcessSerializer Serializer { get; set; } = new NewtonsoftJsonProcessSerializerV4();

        /// <inheritdoc />
        public IModeHandler Modes { get; protected set; }

        public virtual string VRBConsolePrefab => "Prefabs/DefaultVRBConsole";

        protected ILogConsole logConsole;

        /// <summary>
        /// Logger prefab loader for runtime based console log.
        /// </summary>
        /// <exception cref="NullReferenceException">Throw execution if prefab is not found.</exception>
        public ILogConsole VRBConsole
        {
            get
            {
                if (logConsole == null)
                {
                    GameObject logConsoleObj = GameObject.Instantiate(Resources.Load<GameObject>(VRBConsolePrefab));
                    logConsole = logConsoleObj.GetComponent<ILogConsole>();
                }

                if (logConsole == null)
                {
                    throw new NullReferenceException("Failed to load world console prefab.");
                }

                return logConsole;
            }
        }
        
        /// <summary>
        /// Loads the process according to the platform and the serializer.
        /// </summary>
        /// <param name="path">Path to the process json.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Throw if process is not found.</exception>
        public virtual async Task<IProcess> LoadProcess(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("Given path is null or empty!");
                }

                int index = path.LastIndexOf("/");
                string processFolder = path.Substring(0, index);
                string processName = GetProcessNameFromPath(path);
                string manifestPath = $"{processFolder}/{ManifestFileName}.{Serializer.FileFormat}";

                IProcessAssetManifest manifest;
                
                manifest = await ServiceRegistry.Get<IPlatformFileSystem>().FetchManifest(processName, manifestPath, Serializer);
                IProcessAssetStrategy assetStrategy = ReflectionUtils.CreateInstanceOfType(ReflectionUtils.GetConcreteImplementationsOf<IProcessAssetStrategy>().FirstOrDefault(type => type.FullName == manifest.AssetStrategyTypeName)) as IProcessAssetStrategy;

                string processAssetPath = $"{processFolder}/{manifest.ProcessFileName}.{Serializer.FileFormat}";
                byte[] processData = await ServiceRegistry.Get<IPlatformFileSystem>().Read(processAssetPath);
                List<byte[]> additionalData = await GetAdditionalProcessData(processFolder, manifest);

                return assetStrategy.GetProcessFromSerializedData(processData, additionalData, Serializer);
            }
            catch (Exception exception)
            {
                ForwardingLogger.LogError($"Error when loading process. {exception.GetType().Name}, {exception.Message}\n{exception.StackTrace},{RuntimeConfigurator.Instance.gameObject}");
            }

            return null;
        }

        private async Task<List<byte[]>> GetAdditionalProcessData(string processFolder, IProcessAssetManifest manifest)
        {
            List<byte[]> additionalData = new List<byte[]>();
            foreach (string fileName in manifest.AdditionalFileNames)
            {
                string filePath = $"{processFolder}/{fileName}.{Serializer.FileFormat}";

                if (await ServiceRegistry.Get<IPlatformFileSystem>().Exists(filePath))
                {
                    additionalData.Add(await ServiceRegistry.Get<IPlatformFileSystem>().Read(filePath));
                }
                else
                {
                    ForwardingLogger.Log($"Error loading process. File not found: {filePath}");
                }
            }

            return additionalData;
        }

        private static string GetProcessNameFromPath(string path)
        {
            int slashIndex = path.LastIndexOf('/');
            string fileName = path.Substring(slashIndex + 1);
            int pointIndex = fileName.LastIndexOf('.');
            fileName = fileName.Substring(0, pointIndex);

            return fileName;
        }
    }
}
