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
            ModuleObjectSetup("sys", "XSpect.MetaTweet.Modules.SystemFlow", "order=5"),
        ],
        "TwilogFlow": [
            ModuleObjectSetup("twilog", "XSpect.MetaTweet.Modules.TwilogFlow", "order=10"),
        ],
        "TwitterApiFlow": [
            ModuleObjectSetup("twitter", "XSpect.MetaTweet.Modules.TwitterApiFlow", "order=10"),
        ],
        "TwitterUserStreamsServant": [
            ModuleObjectSetup("twitter_s", "XSpect.MetaTweet.Modules.TwitterUserStreamsServant", "order=15"),
        ],
#       "RemotingServant": [
#           ModuleObjectSetup("remoting", "XSpect.MetaTweet.Modules.RemotingTcpServant"),
#           ModuleObjectSetup("remoting", "XSpect.MetaTweet.Modules.RemotingIpcServant"),
#       ],
#        "WcfServant": [
#            ModuleObjectSetup("wcf", "XSpect.MetaTweet.Modules.WcfNetTcpServant"),
#        ],
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
