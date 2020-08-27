namespace PluginBase

type ICommand =
    abstract Name: string
    abstract Description: string

    abstract Execute: unit -> int
