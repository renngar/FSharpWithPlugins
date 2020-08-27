namespace FrenchPlugin

open Humanizer.Localisation.Formatters
open PluginBase
open System

type JsonPlugin() =
    interface ICommand with
        member __.Name = "french"
        member __.Description = "Uses satellite assembly to display french."

        member __.Execute() =
            let formatter = DefaultFormatter("fr")
            printfn "%s" (formatter.DateHumanize_Now())
            for assembly in AppDomain.CurrentDomain.GetAssemblies()
                            |> Array.filter (fun a -> (a.GetName().Name.StartsWith("Humanizer"))) do
                printfn "%s from %s" assembly.FullName assembly.Location

            0
