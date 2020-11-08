module Server

open Elmish

open Saturn
open Giraffe

open Shared

open System.Threading.Tasks
open FSharp.Control.Tasks.V2
open Elmish.Bridge


[<Literal>]
let ConnectionString =
    @"Data Source=LPA-H-NB-182;Initial Catalog=SafeExposuresTable;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

type ConnectionState =
    | Connected
    | Disconnected

let connections = ServerHub<ConnectionState, ServerMsg, Msg>().RegisterServer(RS)

let update clientDispatch msg state =
    match msg with
    | RS msg ->
        match msg with
        | Connect ->
            Connected, Cmd.none
        | Action list ->
            NewState list |> clientDispatch
            state, Cmd.none
    | _ -> Disconnected, Cmd.none

let init _ () = Disconnected, Cmd.none

let server =
    Bridge.mkServer Shared.endpoint init update
    |> Bridge.register RS
    |> Bridge.whenDown Closed
    |> Bridge.withServerHub connections
    |> Bridge.run Giraffe.server

let initState() : Task<ApplicationState> = Task.FromResult []

let webApp = router {
    get "/api/init" (fun next ctx ->
        task {
            let! res = initState()
            return! json res next ctx
        })
    forward "/socket" server
}

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app
