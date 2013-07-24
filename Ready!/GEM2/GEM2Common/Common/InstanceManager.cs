//===========================================================================================================
// GEM2 - Generic entity model 2
//===========================================================================================================
// Copyright © Bruno Klarin (brunedito@yahoo.com).  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED.
//===========================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace GEM2Common
{
    /// <summary>
    /// Factory used to construct types.
    /// </summary>
    internal static class InstanceManager
    {
        private static object _lock = new object();

        /// <summary>
        /// Creates instance of type specified in input path, that implements interface T.
        /// </summary>
        /// <typeparam name="T">Interface type which implementation is requested</typeparam>
        /// <param name="path">Type name in form of *[Namespace].[ConcreteType], [AssemblyName], [Version], [PublicKeyToken], [Culture]*</param>
        /// <returns></returns>
        public static T GetInstance<T>(string path) where T : class
        {
            T instance = null;

            try
            {
                if (!String.IsNullOrEmpty(path))
                {
                    string[] pathInfo = path.Split(new string[] { ", " }, StringSplitOptions.None);
                    string typeName = pathInfo[0];
                    AssemblyName assemblyName = new AssemblyName(pathInfo[1] + ", " + pathInfo[2] + ", " + pathInfo[3] + ", " + pathInfo[4]);

                    Type requestedType = null;
                    Assembly requestedAssembly = null;
                    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                    // Checking if assembly is already loaded
                    foreach (Assembly loadedAssembly in loadedAssemblies)
                    {
                        if (loadedAssembly.GetName().FullName == assemblyName.FullName)
                        {
                            requestedAssembly = loadedAssembly;
                            break;
                        }
                    }

                    // If assembly is not loaded
                    if (requestedAssembly == null)
                    {
                        lock (_lock)
                        {
                            requestedAssembly = Assembly.Load(assemblyName);
                        }
                    }

                    // Creating type
                    requestedType = requestedAssembly.GetType(typeName);
                    instance = (T)Activator.CreateInstance(requestedType);
                }
            }
            catch (Exception ex)
            {
                throw new GEMObjectBuilderException("Exception in InstanceManager. See inner exception for more details.", ex);
            }

            return instance;
        }

    }
}
