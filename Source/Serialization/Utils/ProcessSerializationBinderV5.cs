// Copyright (c) 2013-2019 Innoactive GmbH
// Licensed under the Apache License, Version 2.0
// Modifications copyright (c) 2021-2026 MindPort GmbH

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Serialization;

namespace VRBuilder.Core.Serialization.V5
{
    /// <summary>
    /// Serialization binder that writes assembly-free type names so process files can be exchanged
    /// between engines. The Godot build compiles everything into the "TinkerFlow" assembly while
    /// Unity uses "VRBuilder.Core" plus per-plugin assemblies; assembly-qualified names therefore
    /// are not portable and must not be used as identifiers.
    ///
    /// On write the full type name is emitted without any assembly qualification (generic arguments
    /// included). On read the type is resolved first against the ProcessEngine assembly (where the
    /// VRBuilder core types live in both engines), then against every loaded assembly (plugin types).
    /// </summary>
    internal class ProcessSerializationBinderV5 : DefaultSerializationBinder
    {
        private static readonly Dictionary<string, Type> ResolvedTypeCache = new Dictionary<string, Type>();

        public override void BindToName(Type serializedType, out string? assemblyName, out string? typeName)
        {
            assemblyName = null;
            typeName = GetAssemblyFreeName(serializedType);
        }

        public override Type BindToType(string? assemblyName, string typeName)
        {
            if (ResolvedTypeCache.TryGetValue(typeName, out Type cached))
            {
                return cached;
            }

            Type type = ResolveType(typeName);
            if (type == null)
            {
                // Last resort: legacy files that still carry assembly-qualified names.
                type = base.BindToType(assemblyName, typeName);
            }

            ResolvedTypeCache[typeName] = type;
            return type;
        }

        private static Type ResolveType(string typeName)
        {
            // Fast path: Type.GetType resolves VRBuilder core types against the calling assembly
            // (the ProcessEngine assembly in both engines) and handles bare generic arguments,
            // e.g. "System.Collections.Generic.List`1[[VRBuilder.Core.IStep]]".
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            // Fallback: plugin types compiled into other assemblies (VRBuilder addons, custom behaviors).
            string bareName = typeName.Split(',')[0];
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(bareName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the full type name without assembly qualification. Generic arguments are
        /// emitted in Newtonsoft's native <c>[[...]]</c> syntax with their assemblies stripped too,
        /// e.g. <c>System.Collections.Generic.List`1[[VRBuilder.Core.IStep]]</c>.
        /// Nested types keep the '+' separator that Type.GetType expects.
        /// </summary>
        private static string GetAssemblyFreeName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            Type genericDefinition = type.GetGenericTypeDefinition();
            Type[] genericArguments = type.GetGenericArguments();
            StringBuilder name = new StringBuilder();
            name.Append(genericDefinition.FullName);
            name.Append("[[");
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                {
                    name.Append("],[");
                }

                name.Append(GetAssemblyFreeName(genericArguments[i]));
            }

            name.Append("]]");
            return name.ToString();
        }
    }
}