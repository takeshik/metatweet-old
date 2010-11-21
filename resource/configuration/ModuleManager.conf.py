Create({
    "StartupObjects": {
        "SQLiteStorage": [
            ModuleObjectSetup("main", "XSpect.MetaTweet.Modules.SQLiteStorage", "order=-100"),
        ],
#       "SqlServerStorage": [
#           ModuleObjectSetup("main", "XSpect.MetaTweet.Modules.SqlServerStorage", "order=-100"),
#       ],
        "SystemFlow": [
            ModuleObjectSetup("sys", "XSpect.MetaTweet.Modules.SystemInput", "order=5"),
            ModuleObjectSetup("sys", "XSpect.MetaTweet.Modules.SystemFilter", "order=5"),
            ModuleObjectSetup("sys", "XSpect.MetaTweet.Modules.SystemOutput", "order=5"),
        ],
        "TwitterApiFlow": [
            ModuleObjectSetup("twitter", "XSpect.MetaTweet.Modules.TwitterApiInput", "order=10"),
            ModuleObjectSetup("twitter", "XSpect.MetaTweet.Modules.TwitterApiOutput", "order=10"),
        ],
        "TwitterUserStreamsServant": [
            ModuleObjectSetup("twitter_s", "XSpect.MetaTweet.Modules.TwitterUserStreamsServant", "order=15"),
        ],
        "DataFetcherServant": [
            ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.DataFetcherServant"),
        ],
        "RemotingServant": [
            ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.RemotingTcpServant"),
#           ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.RemotingIpcServant"),
        ],
        "HttpServant": [
            ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.HttpServant"),
        ],
        "SstpServant": [
            ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.SstpServant"),
        ],
        "LocalServant": [
            ModuleObjectSetup("datafetch", "XSpect.MetaTweet.Modules.LocalServant", "order=100"),
        ],
    },
})
