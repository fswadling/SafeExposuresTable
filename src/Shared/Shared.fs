module Shared

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

let endpoint = "/socket"

open System

type Exposure = { ValueDate: DateTime; Volume: decimal}

type ApplicationState = Exposure list

type Msg =
    | InitialStateLoaded of Result<ApplicationState, string>
    | ConnectSocket
    | NewState of ApplicationState

type RemoteServerMessage =
    | Connect
    | Action of ApplicationState

type ServerMsg =
    | RS of RemoteServerMessage
    | Closed
