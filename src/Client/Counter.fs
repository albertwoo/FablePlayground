module Counter

open Feliz


let counter =
    React.functionComponent
        ("Counter"
        ,fun () ->
            let count, setCount = React.useState(0)
            Html.div [
                prop.children [
                    Html.button [
                        prop.text "+"
                        prop.onClick (fun _ -> setCount (count + 1))
                    ]
                    Html.text count
                    Html.button [
                        prop.text "-"
                        prop.onClick (fun _ -> setCount (count - 1))
                    ]
                ]
            ])