namespace AppWithPlugin

open System
open System.IO
open System.Reflection

open ApiWithPlugin
open PluginBase

type Program() =

    static let LoadPlugin(relativePath: string) =
        let root =
            Path.GetFullPath
                (Path.Combine
                    (Path.GetDirectoryName
                        (Path.GetDirectoryName
                            (Path.GetDirectoryName
                                (Path.GetDirectoryName(Path.GetDirectoryName(typeof<Program>.Assembly.Location)))))))
        let pluginLocation =
            Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)))
        printfn "Loading commands from %s" pluginLocation
        let loadContext = PluginLoadContext(pluginLocation)
        loadContext.LoadFromAssemblyName(AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)))

    static let CreateCommands(assembly: Assembly) =
        let mutable count = 0
        seq {
            for t in assembly.GetTypes() do
                if typeof<ICommand>.IsAssignableFrom(t) then
                    let result = Activator.CreateInstance(t) :?> ICommand
                    count <- count + 1
                    yield result
        }

    [<EntryPoint>]
    static let main argv =
        try
            if argv.Length = 1 && argv.[0] = "/d" then
                printfn "Waiting for any key..."
                Console.ReadLine() |> ignore

            let pluginPaths =
                [ @"HelloPlugin\bin\Debug\netcoreapp3.1\HelloPlugin.dll"
                  @"JsonPlugin\bin\Debug\netcoreapp3.1\JsonPlugin.dll"
                  @"XcopyablePlugin\bin\Debug\netcoreapp3.1\XcopyablePlugin.dll"
                  @"OldJsonPlugin\bin\Debug\netcoreapp2.1\OldJsonPlugin.dll"
                  @"FrenchPlugin\bin\Debug\netcoreapp3.1\FrenchPlugin.dll"
                  @"UVPlugin\bin\Debug\netcoreapp3.1\UVPlugin.dll" ]

            let commands = Seq.collect (LoadPlugin >> CreateCommands) pluginPaths

            if argv.Length = 0 then
                printfn "Commands: "
                commands |> Seq.iter (fun c -> printfn "%s\t - %s" c.Name c.Description)
            else
                for commandName in argv do
                    printf "-- %s --" commandName
                    match commands |> Seq.tryFind (fun c -> c.Name = commandName) with
                    | None -> printfn "No such command is known"
                    | Some command ->
                        command.Execute() |> ignore
                        printfn ""
        with ex -> printfn "%A" ex

        0 // return an integer exit code
