Create({
    "StorageName": "main",
    "WorkerCount": 4,
    "Targets": [
        MutableTuple.Create('category: ProfileImage expr: it.Account.Realm == "com.twitter"', 'it.Value'),
    ],
})
