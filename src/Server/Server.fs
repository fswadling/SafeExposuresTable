module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Saturn

open Shared

[<Literal>]
let ConnectionString =
    @"Data Source=LPA-H-NB-182;Initial Catalog=SafeExposuresTable;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

let blankApi = {
        blank = fun () -> async { return 1 }
      }

let webApp =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue blankApi
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app
