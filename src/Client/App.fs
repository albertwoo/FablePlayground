module App

open System
open Elmish
open Elmish.React
open Elmish.Debug
open Elmish.HMR
open Feliz


type State =
    { Count: int }

type Msg =
    | Increase


let init() = 
    { Count = 0 }


let update (msg: Msg) (state: State) =
    match msg with
    | Increase ->
        { state with Count = state.Count + 1 }
        

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [
        prop.style [ style.padding 20 ]
        prop.children [
          Counter.counter()
        ]
    ]


Program.mkSimple init update render
#if DEBUG
|> Program.withDebugger
#endif
|> Program.withReactSynchronous "elmish-app"
|> Program.run
