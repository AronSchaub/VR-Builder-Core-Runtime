#if UNITY_6000_0_OR_NEWER
// Copyright (c) 2013-2019 Innoactive GmbH
// Modifications copyright (c) 2021-2026 MindPort GmbH
// SPDX-License-Identifier: Apache-2.0

using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace VRBuilder.Core.Settings
{
    /// <summary>
    /// ScriptableObject with additional load and save mechanic to make it a singleton.
    /// </summary>
    /// <typeparam name="T">The class itself</typeparam>
    public class SettingsObject<T> : ScriptableObject where T : ScriptableObject, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (EditorUtility.IsDirty(instance))
                {
                    instance = null;
                }
#endif
                if (instance == null)
                {
                    instance = Load();
                }

                return instance;
            }
        }

        private static T Load()
        {
            try
            {
                T settings = Resources.Load<T>(typeof(T).Name);

                if (settings == null)
                {
                    // Create an instance
                    settings = CreateInstance<T>();
#if UNITY_EDITOR
                    if (!Directory.Exists("Assets/MindPort/VR Builder/Resources"))
                    {
                        Directory.CreateDirectory("Assets/MindPort/VR Builder/Resources");
                    }

                    AssetDatabase.CreateAsset(settings, $"Assets/MindPort/VR Builder/Resources/{typeof(T).Name}.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
#endif
                }

                return settings;
            }
            catch (UnityException e)
            {
                // This is commented to avoid confusing errors since this exception can trigger but does not block functionality. Uncomment for debugging purposes.
                // ForwardingLogger.LogException(e);
                throw;
            }
        }

        /// <summary>
        /// Saves the VR Builder settings, only works in editor.
        /// </summary>
        public void Save()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        ~SettingsObject()
        {
#if UNITY_EDITOR
            if (EditorUtility.IsDirty(this))
            {
                Save();
            }
#endif
        }
    }
}
#elif GODOT
// Modifications copyright (c) 2026 Aron Schaub
// SPDX-License-Identifier:LGPL-3.0-or-later

using Godot;

namespace VRBuilder.Core.Settings //TinkerFlow.Core.Settings
{
    /// <summary>
    /// Godot 4 C# equivalent of the Unity ScriptableObject-based SettingsObject.
    /// Stores settings in ProjectSettings (project.godot) instead of Resources/AssetDatabase.
    /// </summary>
    /// <typeparam name="T">The concrete settings class (must have a parameterless constructor).</typeparam>
    public abstract class SettingsObject<T> where T : SettingsObject<T>, new()
    {
        private static T _instance;

        /// <summary>
        /// Singleton instance. Initializes defaults on first access.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.RegisterDefaults();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The ProjectSettings path prefix for this settings group.
        /// Example: "addons/process_engine"
        /// </summary>
        protected abstract string SettingsPrefix { get; }

        /// <summary>
        /// Override to call Define() for each setting with its default value.
        /// Called once on first singleton access.
        /// </summary>
        protected virtual void RegisterDefaults() { }

        /// <summary>
        /// Define a setting with a default and register its initial value.
        /// If the setting doesn't exist in project.godot yet, it is created.
        /// SetInitialValue ensures the "Reset" button in Project Settings UI works.
        /// </summary>
        protected void Define<[MustBeVariant] TValue>(string key, TValue defaultValue)
        {
            string path = $"{SettingsPrefix}/{key}";

            if (!ProjectSettings.HasSetting(path))
            {
                ProjectSettings.SetSetting(path, Variant.From(defaultValue));
            }

            ProjectSettings.SetInitialValue(path, Variant.From(defaultValue));
        }

        /// <summary>
        /// Get a setting value. Falls back to <paramref name="defaultValue"/> if not set.
        /// </summary>
        protected TValue Get<[MustBeVariant] TValue>(string key, TValue defaultValue = default) 
        {
            string path = $"{SettingsPrefix}/{key}";
            Variant result = ProjectSettings.GetSetting(path, Variant.From(defaultValue));
            return result.As<TValue>();
        }

        /// <summary>
        /// Set a setting value and persist it to project.godot.
        /// </summary>
        protected void Set<[MustBeVariant] TValue>(string key, TValue value)
        {
            string path = $"{SettingsPrefix}/{key}";
            ProjectSettings.SetSetting(path, Variant.From(value));
        }

        /// <summary>
        /// Persist all ProjectSettings to project.godot.
        /// Only works in the editor (ProjectSettings are baked into exported binaries).
        /// For runtime persistence in exported builds, use a ConfigFile at user:// instead.
        /// </summary>
        public void Save()
        {
            ProjectSettings.Save();
        }
    }
}
#endif
