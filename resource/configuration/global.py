import clr
clr.AddReference("System")
clr.AddReference("System.Core")
clr.AddReference("Linx")
clr.AddReference("MetaTweetObjectModel")
clr.AddReference("MetaTweetFoundation")
from System import *
from System.Collections.Generic import *
from XSpect import *
from XSpect.MetaTweet import *
from XSpect.MetaTweet.Modules import *
from XSpect.MetaTweet.Requesting import *

def Create(body):
    child = Dynamic.ExpandoObject()
    for e in body:
        IDictionary[String, Object].Add(child, e, body[e])
    return child

