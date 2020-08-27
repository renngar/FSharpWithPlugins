namespace ApiWithPlugin

open System
open System.Runtime.Loader

type PluginLoadContext(pluginPath: string) =
    inherit AssemblyLoadContext()

    let _resolver = AssemblyDependencyResolver pluginPath

    override __.Load(assemblyName) =
        let assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName)
        if isNull assemblyPath then null else base.LoadFromAssemblyPath(assemblyPath)

    override __.LoadUnmanagedDll(unmanagedDllName) =
        let libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName)
        if isNull libraryPath
        then IntPtr.Zero
        else base.LoadUnmanagedDllFromPath(libraryPath)
