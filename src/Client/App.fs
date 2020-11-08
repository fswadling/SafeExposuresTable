module App

open Elmish
open Elmish.React
open Elmish.Bridge

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram Index.init Index.update Index.view
|> Program.withBridgeConfig (Bridge.endpoint "./socket" |> Bridge.withUrlMode Append)
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
