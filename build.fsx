#r "nuget: Fake.Core.Process"
#r "nuget: Fake.IO.FileSystem"
#r "nuget: BlackFox.Fake.BuildTask"

open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open BlackFox.Fake


fsi.CommandLineArgs
|> Array.skip 1
|> BuildTask.setupContextFromArgv 


let run cmd args workingDir =
    let arguments = args |> String.split ' ' |> Arguments.OfArgs
    Command.RawCommand (cmd, arguments)
    |> CreateProcess.fromCommand
    |> CreateProcess.withWorkingDirectory workingDir
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore


let buildClientJs buildOrWatch =
    let mode = if buildOrWatch then "build" else "watch"
    run "dotnet" (sprintf "fable %s . --outDir ./www/.clientjs" mode) "./src/Client"


let checkEnv =
    BuildTask.create "Check environment" [] {
        run "yarn" "--version" ""
        run "yarn" "install" "./src/Client/www"
        run "dotnet" "tool restore" ""
    }


let watchTailwindForClient =
    BuildTask.create "Watch tailwind css for client dev" [ checkEnv ] {
        let buildTailwind() = run "yarn" "tailwindcss build css/app.css -o css/app-dev.css" "./src/Client/www"
        buildTailwind()
        ChangeWatcher.run 
            (fun _ -> printfn "Rebuild tailwind..."; buildTailwind()) 
            (!!"./src/Client/www/css/app.css"
             ++"./src/Client/www/tailwind.config.js")
        |> ignore
    }


let startDev =
    BuildTask.create "Start development" [ watchTailwindForClient ] {
        buildClientJs true
        
        [
            async {
                run "yarn" "parcel index.html --out-dir .dist" "./src/Client/www"
            }
            async {
                buildClientJs false
            }
        ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore
    }


let buildForProd =
    BuildTask.create "BundleProd" [ checkEnv ] {
        Shell.cleanDir "./src/Client/www/.dist_prod"
        buildClientJs true
        run "yarn" "parcel build index.html --out-dir .dist_prod" "./src/Client/www"
    }


BuildTask.runOrDefault startDev
