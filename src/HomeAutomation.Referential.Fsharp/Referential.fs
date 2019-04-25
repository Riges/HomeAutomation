namespace HomeAutomation.Referential.Fsharp

open System

type DeviceType = Undefined = 0 | Thermometer = 1 | Lighting = 2 | Valve = 3
type RoomType =
    | Undefined = 0
    | Livingroom = 1
    | Kitchen = 2
    | Office = 3
    | Bedroom = 4
    | Bathroom = 5
    | Toilets = 6
    | Corridor = 7
    | DiningRoom = 8
    | Stairs = 9
    | Lobby = 10
    | Outdoor = 11
    | Garage = 12
    | Custom = 13

type ThingId = ThingId of Guid
type Room = {id: ThingId; label: string;  roomType: RoomType}
type Device = {id: ThingId; label: string; deviceType: DeviceType; roomId: Guid}
type Thing = Room | Device
type ThingProviderMapping = {thingId: ThingId; providerId: string}
type ReferentialRecord = { devices: Device list; rooms: Room list; thingProviderMapping: Map<string, ThingId> }

module ConfigurationReferential =
    let getDeviceByProviderId referential providerId = 
        let thingId = referential.thingProviderMapping.TryFind providerId
        match thingId with
        | Some id -> referential.devices
                     |> List.tryFind(fun d -> d.id = id)
        | None -> None
        
    let createReferential devices rooms thingProviderMapping = { devices = devices; rooms = rooms; thingProviderMapping = thingProviderMapping}