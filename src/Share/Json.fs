module Json


let toString data = Thoth.Json.Encode.Auto.toString(4, data)
