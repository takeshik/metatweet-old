using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TidyNet;
using System.IO;
using System.Xml;

namespace XSpect.Xml
{
    public sealed class XmlTidier
    {
        private Tidy _tidy = new Tidy();

        public XmlTidier()
        {
            this._tidy.Options.BreakBeforeBR = true;
            this._tidy.Options.CharEncoding = CharEncoding.UTF8;
            this._tidy.Options.DocType = DocType.Omit;
            this._tidy.Options.DropEmptyParas = true;
            this._tidy.Options.DropFontTags = true;
            this._tidy.Options.EncloseBlockText = true;
            this._tidy.Options.EncloseText = true;
            this._tidy.Options.FixBackslash = true;
            this._tidy.Options.FixComments = true;
            this._tidy.Options.LiteralAttribs = false;
            this._tidy.Options.LogicalEmphasis = true;
            this._tidy.Options.MakeClean = true;
            this._tidy.Options.NumEntities = true;
            this._tidy.Options.QuoteAmpersand = true;
            this._tidy.Options.QuoteMarks = true;
            this._tidy.Options.QuoteNbsp = true;
            this._tidy.Options.WrapAsp = true;
            this._tidy.Options.WrapAttVals = true;
            this._tidy.Options.WrapJste = true;
            this._tidy.Options.WrapPhp = true;
            this._tidy.Options.WrapScriptlets = true;
            this._tidy.Options.WrapSection = true;
            this._tidy.Options.Xhtml = true;
            this._tidy.Options.XmlOut = true;
        }

        public Stream Tidy(Stream input)
        {
            MemoryStream output = new MemoryStream();
            this._tidy.Parse(input, output, new TidyMessageCollection());
            return output;
        }

        public XmlDocument Parse(String input)
        {
            using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(input), false))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    this._tidy.Parse(inputStream, outputStream, new TidyMessageCollection());
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.Load(outputStream);
                    return xdoc;
                }
            }
        }
    }
}
