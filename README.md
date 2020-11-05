# Elmish TODO

Elmish TODO app from the great [Elmish Book](https://zaid-ajaj.github.io/the-elmish-book) by Zaid Ajaj, compiled with Nagareyama (Fable 3).

Intentionally, this example contains the minimal set of files you need to get up and running. It assumes you have [dotnet sdk](https://dotnet.microsoft.com), [nodejs](https://nodejs.org/) and [Parcel](https://parceljs.org/getting_started.html) installed globally.

After cloning, this is what you need to start up a server http://localhost:1234 that will automatically refresh on code changes.

```
dotnet tool restore
dotnet fable watch src/Client --outDir devClient --run parcel index.html
```

Have fun!
