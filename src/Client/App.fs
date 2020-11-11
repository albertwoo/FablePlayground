module App

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
        prop.classes []
        prop.children [
            Html.div [ 
                prop.classes [ Tw.``bg-blue-200``; Tw.``p-10`` ]
                prop.children [
                    Html.div state.Count
                    Html.button [
                        prop.text "+"
                        prop.onClick (fun _ -> Increase |> dispatch)
                    ]
                ]
            ]
            Html.div [
                prop.classes []
                prop.text (Json.toString {| DemoJson = "name" |})
            ]
        ]
    ]


Program.mkSimple init update render
#if DEBUG
|> Program.withDebugger
#endif
|> Program.withReactSynchronous "elmish-app"
|> Program.run
