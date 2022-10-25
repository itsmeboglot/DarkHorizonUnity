using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Serialization;

namespace Whimsy.Shared.Serialization.Json
{
    internal class NetCoreSerializationBinder : DefaultSerializationBinder
    {
        #region Consts

        private static readonly Regex Regex = new Regex(
            @"System\.Private\.CoreLib(, Version=[\d\.]+)?(, Culture=[\w-]+)(, PublicKeyToken=[\w\d]+)?");

        private static readonly Dictionary<Type, (string assembly, string type)> Cache = new Dictionary<Type, (string, string)>();

        #endregion

        #region Implementations

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            base.BindToName(serializedType, out assemblyName, out typeName);

            if (Cache.TryGetValue(serializedType, out var name))
            {
                assemblyName = name.assembly;
                typeName = name.type;
            }
            else
            {
                if (assemblyName.Contains("System.Private.CoreLib"))
                    assemblyName = Regex.Replace(assemblyName, "mscorlib");

                if (typeName.Contains("System.Private.CoreLib"))
                    typeName = Regex.Replace(typeName, "mscorlib");

                Cache.Add(serializedType, (assemblyName, typeName));
            }
        }

        #endregion
    }
}