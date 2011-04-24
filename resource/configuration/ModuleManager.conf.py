Create({
    "StartupObjects": {
        "SQLiteStorage": [
            ModuleObjectSetup("main", "XSpect.MetaTweet.Objects.SQLiteStorage", "order=-100"),
        ],
#       "SqlServerStorage": [
#           ModuleObjectSetup("main", "XSpect.MetaTweet.Objects.SqlServerStorage", "order=-100"),
#       ],
#       "SqlServerCompactStorage": [
#           ModuleObjectSetup("main", "XSpect.MetaTweet.Objects.SqlServerCompactStorage", "order=-100"),
#       ],
#       "RavenLightweightStorage": [
#           ModuleObjectSetup("main", "XSpect.MetaTweet.Objects.RavenLightweightStorage", "order=-100"),
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
            ModuleObjectSetup("remoting", "XSpect.MetaTweet.Modules.RemotingTcpServant"),
#           ModuleObjectSetup("remoting", "XSpect.MetaTweet.Modules.RemotingIpcServant"),
        ],
        "WcfServant": [
            ModuleObjectSetup("wcf", "XSpect.MetaTweet.Modules.WcfNetTcpServant"),
        ],
        "HttpServant": [
            ModuleObjectSetup("http", "XSpect.MetaTweet.Modules.HttpServant"),
        ],
        "SstpServant": [
            ModuleObjectSetup("sstp", "XSpect.MetaTweet.Modules.SstpServant"),
        ],
        "LocalServant": [
            ModuleObjectSetup("local", "XSpect.MetaTweet.Modules.LocalServant", "order=100"),
        ],
    },
})
