using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;

namespace XSpect.MetaTweet.Install
{
    [RunInstaller(true)]
    public partial class ServiceInstaller
        : Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
