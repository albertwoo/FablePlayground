#r "nuget: Fake.Core.Process"
#r "nuget: Fake.IO.FileSystem"
#r "nuget: BlackFox.Fake.BuildTask"

open Fake.Core
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open BlackFox.Fake


type Env = PROD | DEV


fsi.CommandLineArgs
|> Array.skip 1
|> BuildTask.setupContextFromArgv 

let platformTool tool winTool =
    let tool = if Environment.isUnix then tool else winTool
    match ProcessUtils.tryFindFileOnPath tool with
    | Some t -> t
    | _ -> failwith (tool + " was not found in path. ")

let run cmd args workingDir =
    let arguments = args |> String.split ' ' |> Arguments.OfArgs
    Command.RawCommand (cmd, arguments)
    |> CreateProcess.fromCommand
    |> CreateProcess.withWorkingDirectory workingDir
    |> CreateProcess.ensureExitCode
    |> Proc.run
    |> ignore


let node   = run (platformTool "node" "node.exe")
let yarn   = run (platformTool "yarn" "yarn.cmd")
let dotnet = run (platformTool "dotnet" "dotnet.exe")  


let buildClientJs env =
    let env = match env with PROD -> "" | DEV -> " watch"
    dotnet (sprintf "fable%s . --outDir ./www/.clientjs" env) "./src/Client"


let checkEnv =
    BuildTask.create "Check environment" [] {
        yarn "--version" ""
        yarn "install" "./src/Client/www"
        dotnet "tool restore" ""
    }


let watchTailwindForClient =
    BuildTask.create "Watch tailwind css for client dev" [ checkEnv ] {
        let buildTailwind() = yarn "tailwindcss build css/app-dev.css -o css/app.css" "./src/Client/www"
        buildTailwind()
        ChangeWatcher.run 
            (fun _ -> printfn "Rebuild tailwind..."; buildTailwind()) 
            (!!"./src/Client/www/css/app.css"
             ++"./src/Client/www/tailwind.config.js")
        |> ignore
    }


let runDev =
    BuildTask.create "StartDev" [ watchTailwindForClient ] {
        [
            async {
                buildClientJs DEV
            }
            async {
                do! Async.Sleep 15_000
                Shell.cleanDir "./src/Client/www/.dist"
                yarn "parcel serve index.html --out-dir .dist" "./src/Client/www"
            }
        ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore
    }


let bundleProd =
    BuildTask.create "BundleProd" [ checkEnv ] {
        Shell.cleanDir "./src/Client/www/.dist_prod"
        buildClientJs PROD
        yarn "parcel build index.html --out-dir .dist_prod --public-url ./ --no-source-maps" "./src/Client/www"
    }


BuildTask.runOrDefault runDev
