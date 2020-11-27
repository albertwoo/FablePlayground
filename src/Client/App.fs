module App

open Elmish
open Elmish.React
open Elmish.HMR
open Feliz
open type Html
open type prop


type State = { Count: int }

type Msg = Increase | Decrease


let init() = { Count = 0 }


let update (msg: Msg) (state: State) =
    match msg with
    | Increase -> { state with Count = state.Count + 1 }
    | Decrease -> { state with Count = state.Count - 1 }
        

[<ReactComponent>]
let counterButton (content: string) (onClicked: unit -> unit) =
    button [
        text content
        onClick (fun _ -> onClicked())
        classes [
            Tw.``text-center``; Tw.``bg-green-200``; Tw.``w-full``; Tw.``cursor-pointer``; Tw.``text-sm``
            Tw.``py-1``; Tw.``outline-none``; Tw.``focus:outline-none``; Tw.``hover:bg-green-600``; Tw.``hover:text-white``; Tw.``ease-linear``
        ]
    ]

let render (state: State) (dispatch: Msg -> unit) =
    div [
        classes [ Tw.``h-screen``; Tw.``w-screen``; Tw.flex; Tw.``items-center``; Tw.``justify-center`` ]
        children [
            div [ 
                classes [ Tw.rounded; Tw.``shadow-lg``; Tw.``hover:shadow-2xl``; Tw.``bg-green-100``; Tw.``overflow-hidden`` ]
                children [
                    counterButton "/\\" (fun _ -> Increase |> dispatch)
                    div [
                        classes [ Tw.``text-center``; Tw.``font-semibold``; Tw.flex; Tw.``items-center``; Tw.``justify-center``; Tw.``p-6`` ]
                        children [
                            Html.div $"Elmish counter {state.Count}"
                        ]
                    ]
                    counterButton "\/" (fun _ -> Decrease |> dispatch)
                ]
            ]
            Counter.Counter()
        ]
    ]


importRequiredAssets()

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
