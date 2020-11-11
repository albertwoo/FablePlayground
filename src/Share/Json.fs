module Json


let inline toString data = Thoth.Json.Encode.Auto.toString(4, data)
