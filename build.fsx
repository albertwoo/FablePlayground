#r "nuget: Fake.Core.Process,5.20.0"
#r "nuget: Fake.IO.FileSystem,5.20.0"
#r "nuget: BlackFox.Fake.BuildTask,0.1.3"

open System.IO
open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.IO.FileSystemOperators
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


let yarn   = run (platformTool "yarn" "yarn.cmd")
let dotnet = run (platformTool "dotnet" "dotnet.exe")  


let watchFile fn file =
    let watcher = new FileSystemWatcher(Path.getDirectory file, Path.GetFileName file)
    watcher.NotifyFilter <-  NotifyFilters.CreationTime ||| NotifyFilters.Size ||| NotifyFilters.LastWrite
    watcher.Changed.Add (fun _ ->
        printfn "File changed %s" file
        fn())
    watcher.EnableRaisingEvents <- true
    watcher


let buildClientJs env watch =
    let mode = match watch with false -> "" | true -> " watch"
    let define = match env with PROD -> "" | DEV -> " --define DEBUG"
    dotnet (sprintf "fable%s . --outDir ./www/.clientjs%s" mode define) "./src/Client"

let buildClientCss() =
    printfn "Build client css"
    yarn "tailwindcss build css/app-dev.css -o css/app.css" "./src/Client/www"


let checkEnv =
    BuildTask.create "Check environment" [] {
        yarn "--version" ""
        yarn "install" "./src/Client/www"
        dotnet "tool restore" ""
    }


let runDev =
    BuildTask.create "StartDev" [ checkEnv ] {
        Shell.cleanDir "./src/Client/www/.clientjs"
        buildClientCss()
        buildClientJs DEV false
        [
            async {
                buildClientJs DEV true
            }
            async {
                let watchers =
                    !!"./src/Client/www/css/app-dev.css"
                    ++"./src/Client/www/tailwind.config.js"
                    |> Seq.toList
                    |> List.map (watchFile buildClientCss)
                
                Shell.cleanDir "./src/Client/www/.dist"
                yarn "parcel serve index.html --out-dir .dist" "./src/Client/www"

                printfn "Clean up..."
                watchers |> List.iter (fun x -> x.Dispose())
            }
        ]
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore
    }


let bundleProd =
    BuildTask.create "BundleProd" [ checkEnv ] {
        Shell.cleanDir "./src/Client/www/.clientjs"
        Shell.cleanDir "./src/Client/www/.dist_prod"
        buildClientCss()
        buildClientJs PROD false
        yarn "parcel build index.html --out-dir .dist_prod --public-url ./ --no-source-maps" "./src/Client/www"
    }


BuildTask.runOrDefault runDev
