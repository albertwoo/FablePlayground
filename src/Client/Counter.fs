module Counter

open Feliz
open type Html
open type prop
open Common


[<ReactComponent>]
let Counter () =
    let (count, setCount) = React.useState(0)
    div [
        classes [ 
            Tw.``ml-10``; Tw.rounded; Tw.``shadow-lg``; Tw.``hover:shadow-2xl``;
            Tw.``bg-blue-100``; Tw.``overflow-hidden``; Tw.``p-6``
        ]
        children [
            h2 [
                classes [ Tw.``font-semibold`` ]
                text $"Fast refresh counter {count}"
            ]
            button [
                text "Increment"
                onClick (fun _ -> setCount(count + 1))
                classes [ Tw.``bg-blue-500``; Tw.rounded; Tw.``px-4``; Tw.``py-1``; Tw.``mt-4``; Tw.``hover:bg-blue-400``; Tw.``text-white``; Tw.``focus:ring-4``; Tw.``focus:ring-blue-600`` ]
            ]
        ]
    ]
