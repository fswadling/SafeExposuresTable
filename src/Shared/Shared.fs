module Shared

module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

let endpoint = "/socket"

type Model =
    { Input: string }
    
type ApplicationState = Ping | Pong
    
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
