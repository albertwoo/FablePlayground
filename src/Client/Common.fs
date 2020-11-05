[<AutoOpen>]
module Common

open Zanaptak.TypedCssClasses

let [<Literal>] TailwindCssPath = __SOURCE_DIRECTORY__ + "/../../css/app.css"
type Tw = CssClasses<TailwindCssPath, Naming.Verbatim>