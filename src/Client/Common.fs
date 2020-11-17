[<AutoOpen>]
module Common

open Fable.Core.JsInterop
open Zanaptak.TypedCssClasses


importSideEffects "./www/css/app.css"

let [<Literal>] TailwindCssPath = __SOURCE_DIRECTORY__ + "/www/css/tailwind-generated.css"
type Tw = CssClasses<TailwindCssPath, Naming.Verbatim>


// Use this to ensure this file is referenced so the css or other assets can be imported
let importRequiredResources() = ()
