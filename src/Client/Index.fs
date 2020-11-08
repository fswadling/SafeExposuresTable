module Index

open Elmish
open Fable.Remoting.Client
open Shared
open Thoth.Fetch
open Fable.Core
open Elmish.Bridge

type Model = {
    State: ApplicationState option
    SocketConnected: bool
}

let initialState () = Fetch.tryFetchAs<FetchError, ApplicationState>("/api/init")

let init(): Model * Cmd<Msg> =
    let initialModel = { State = None; SocketConnected = false }
    let loadStateCmd =
        Cmd.OfPromise.either
            initialState ()
            (fun res ->
                match res with
                | Ok r ->  InitialStateLoaded (Ok r)
                | _ -> InitialStateLoaded (Error "Unknown error")
            )
            (fun _ -> InitialStateLoaded (Error "Fetch error"))
    initialModel, loadStateCmd

let update (msg : Msg) (currentModel : Model) : Model * Cmd<Msg> =
    match currentModel.State, msg with
    | _, InitialStateLoaded (Ok initialState)->
        let nextModel = { State = Some initialState; SocketConnected = false }
        nextModel, Cmd.ofMsg ConnectSocket

    | _, ConnectSocket ->
        try
            Bridge.Send Connect
            { currentModel with SocketConnected = true }, Cmd.none
        with _ ->
            let delay () = promise {
                do! Async.Sleep 1000 |> Async.StartAsPromise
            }
            let checkSocket = (fun _ -> ConnectSocket)
            { currentModel with SocketConnected = false }, Cmd.OfPromise.either delay () checkSocket checkSocket

    | _, NewState list ->
        let nextModel = { currentModel with State = Some list }
        nextModel, Cmd.none

    | _ -> currentModel, Cmd.none

open Fable.React
open Fable.React.Props
open Fulma

let view (model : Model) (dispatch : Msg -> unit) =
    Hero.hero [
        Hero.Color IsPrimary
        Hero.IsFullHeight
        Hero.Props [
            Style [
                Background """linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url("https://unsplash.it/1200/900?random") no-repeat center center fixed"""
                BackgroundSize "cover"
            ]
        ]
    ] [
        Hero.head [ ] [
            Navbar.navbar [ ] [
                Container.container [ ] [ ]
            ]
        ]

        Hero.body [ ] [
            Container.container [ ] [
                Column.column [
                    Column.Width (Screen.All, Column.Is6)
                    Column.Offset (Screen.All, Column.Is3)
                ] [
                    Heading.p [ Heading.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ] [ str "SafeExposuresTable" ]
                ]
            ]
        ]
    ]
