namespace HelloPlugin

open PluginBase

type HelloCommand() =
    interface ICommand with
        member __.Name = "hello"
        member __.Description = "Displays hello message."

        member __.Execute() =
            printfn "Hello !!!"
            0
