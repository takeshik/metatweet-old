using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Diagnostics;
using XSpect.Configuration;

namespace XSpect.MetaTweet
{
    internal static class Program
    {
        internal static void Main(String[] args)
        {
			if (args.Where(s => s == "-d" || s == "--debug").Any())
			{
				Debugger.Launch();
			}
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            ServiceBase.Run(new ServerHost());
        }
    }
}
  