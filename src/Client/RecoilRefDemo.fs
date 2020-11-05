module RecoilRefDemo

open Feliz
open Feliz.Recoil

type ConfigRenderProp = { Config: string; Ref: IRefValue<(unit -> string) option> }

type ExtensionConfig =
    { GetConfigRender: ConfigRenderProp -> ReactElement }

    static member DefaultValue =
        { GetConfigRender = fun _ -> Html.none }


let extensionConfig =
    atomFamily {
        key "extension-config"
        def (fun (id: int) -> ExtensionConfig.DefaultValue)
    }


let extensionConfigView =
    React.functionComponent
        ("ExtensionConfigView"
        ,fun () ->
            let getConfigRef = React.useRef None
            let config = Recoil.useValue(extensionConfig 0)
            let gotConfig, setGotConfig = React.useState None

            Html.div [
                prop.classes [ Tw.``p-10``; Tw.rounded; Tw.``shadow-lg`` ]
                prop.children [
                    Html.div "extension config"

                    Html.div [
                        prop.classes [ Tw.``shadow-md``; Tw.``p-4``; Tw.``my-2``; Tw.rounded; Tw.``bg-red-200`` ]
                        prop.children [
                            config.GetConfigRender { Config = "123"; Ref = getConfigRef }
                        ]
                    ]

                    Html.button [
                        prop.text "Click to get config"
                        prop.onClick (fun _ -> getConfigRef.current |> Option.map (fun ref -> ref()) |> setGotConfig)
                    ]

                    Html.div "Got config"
                    Html.div (string gotConfig)
                ]
            ])    


let demoExtensionConfig =
    React.functionComponent
        ("DemoExtensionConfig"
        ,fun (props: ConfigRenderProp) ->
            let state, setState = React.useState props.Config

            let callbackRef = React.useCallbackRef(fun () -> state)

            React.useEffectOnce(fun () ->
                props.Ref.current <- Some callbackRef
            )

            Html.div [
                prop.children [
                    Html.div "demo extension config"
                    Html.input [
                        prop.type'.text
                        prop.value state
                        prop.onChange setState
                    ]
                ]
            ])    


let demoExtension =
    React.functionComponent
        ("DemoExtension"
        ,fun () ->
            let setValue = Recoil.useSetState(extensionConfig 0)
            
            React.useEffectOnce(fun () ->
                setValue { GetConfigRender = demoExtensionConfig }
            )

            Html.div [
                prop.text "demo extension"
            ])


let demoView = React.functionComponent(fun () ->
    Html.div [
        prop.classes [ Tw .``p-10``; Tw.``mt-10``; Tw.rounded; Tw.``bg-purple-100`` ]
        prop.children [
            Recoil.root [
                extensionConfigView()

                Html.br []

                demoExtension()
            ]
        ]
    ])
