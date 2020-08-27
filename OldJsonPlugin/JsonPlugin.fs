namespace JsonPlugin

open Newtonsoft.Json
open PluginBase
open System

type Info =
    { JsonVersion: string
      JsonLocation: string
      Machine: string
      Date: DateTime }

type JsonPlugin() =
    interface ICommand with
        member __.Name = "oldjson"
        member __.Description = "Outputs JSON value."

        member __.Execute() =
            let jsonAssembly = typeof<JsonConvert>.Assembly

            let info =
                { JsonVersion = jsonAssembly.FullName
                  JsonLocation = jsonAssembly.Location
                  Machine = Environment.MachineName
                  Date = DateTime.Now }

            printfn "%s" (JsonConvert.SerializeObject(info, Formatting.Indented))
            0
