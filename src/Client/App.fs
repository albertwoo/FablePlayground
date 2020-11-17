module App

open Elmish
open Elmish.React
open Elmish.Debug
open Elmish.HMR
open Feliz


type State = { Count: int }

type Msg = Increase


let init() = { Count = 0 }


let update (msg: Msg) (state: State) =
    match msg with
    | Increase ->
        { state with Count = state.Count + 1 }
        

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [
        prop.classes [ Tw.``h-screen``; Tw.``w-screen``; Tw.flex; Tw.``items-center``; Tw.``justify-center`` ]
        prop.children [
            Html.div [ 
                prop.classes [
                    Tw.``w-32``; Tw.rounded; Tw.``shadow-lg``; Tw.``hover:shadow-2xl``;
                    Tw.``bg-pink-200``; Tw.``overflow-hidden``
                ]
                prop.children [
                    Html.button [
                        prop.text "/\\"
                        prop.onClick (fun _ -> Increase |> dispatch)
                        prop.classes [
                            Tw.``text-center``; Tw.``bg-green-500``; Tw.``w-full``; Tw.``cursor-pointer``; Tw.``text-sm``
                            Tw.``text-white``; Tw.``py-1``; Tw.``outline-none``; Tw.``focus:outline-none``; Tw.``hover:bg-green-600``; Tw.``ease-linear``
                        ]
                    ]
                    Html.div [
                        prop.classes [ Tw.``text-center``; Tw.``text-4xl``; Tw.``font-semibold``; Tw.``h-20``; Tw.flex; Tw.``items-center``; Tw.``justify-center`` ]
                        prop.children [ Html.div state.Count ]
                    ]
                ]
            ]
        ]
    ]


Program.mkSimple init update render
#if DEBUG
|> Program.withDebugger
#endif
|> Program.withReactSynchronous "elmish-app"
|> Program.run
