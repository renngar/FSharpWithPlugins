module UVPlugin

open PluginBase
open System
open System.Runtime.InteropServices

[<DllImport("libuv", CallingConvention = CallingConvention.Cdecl)>]
extern unativeint uv_version()

let private getVersion() =
    let version = int (uv_version())
    Version((version &&& 0xFF0000) >>> 16, (version &&& 0xFF00) >>> 8, version &&& 0xFF)

type JsonPlugin() =
    interface ICommand with
        member __.Name = "uv"
        member __.Description = "Uses the native library libuv to show its version."

        member __.Execute() =
            printfn "Using libuv version %A" (getVersion())
            0
