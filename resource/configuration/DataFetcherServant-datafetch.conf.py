Create({
    "StorageName": "main",
    "WorkerCount": 4,
    "Targets": [
        MutableTuple.Create('name: ProfileImage expr: it.Account.Realm == "com.twitter"', 'it.Value'),
    ],
})
