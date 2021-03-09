[<AutoOpen>]
module Common

open Fable.Core.JsInterop
open Zanaptak.TypedCssClasses


importSideEffects "./www/css/app.css"
importSideEffects "./www/css/icomoon/style.css"

let [<Literal>] TailwindCssPath = __SOURCE_DIRECTORY__ + "/www/css/tailwind-generated.css"
let [<Literal>] IconmoonCssPath = __SOURCE_DIRECTORY__ + "/www/css/icomoon/style.css"
type Tw = CssClasses<TailwindCssPath, Naming.Verbatim>
type Ic = CssClasses<IconmoonCssPath, Naming.Verbatim>


// Use this to ensure this file is referenced so the css or other assets can be imported
let importRequiredAssets() = ()
