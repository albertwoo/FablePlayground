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
                onClick (fun _ -> setCount(count + 1))
                classes [ Tw.``bg-blue-500``; Tw.``rounded-full``; Tw.``w-10``; Tw.``h-10``; Tw.``mt-4``; Tw.``hover:bg-blue-400``; Tw.``text-white``; Tw.``focus:ring-4``; Tw.``focus:ring-blue-600`` ]
                children [
                    span [
                        classes [ Ic.``icon-plus``;  ]
                    ]
                ]
            ]
        ]
    ]
