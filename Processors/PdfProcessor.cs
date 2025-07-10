using jsreport.Local;
using jsreport.Types;
using Crossbill.Common.Processors;
using Crossbill.Common.Resources;
using Crossbill.Common.Utils;
using System;
using System.IO;
using Crossbill.Common;

namespace Crossbill.Plugins.PDF.JsReport.Processors
{
    public class PdfProcessor : IPdfProcessor
    {
        public ICrossContainer _container;
        public PdfProcessor(ICrossContainer container)
        {
            this._container = container;
        }

        public virtual MemoryStream HtmlToPdf(string html)
        {
            MemoryStream stream = new MemoryStream();
            HtmlToPdfStream(html, stream);
            return stream;
        }

        public virtual void HtmlToPdfFile(string html, string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                HtmlToPdfStream(html, stream);
            }
        }

        protected virtual void HtmlToPdfStream(string html, Stream stream)
        {
            IPathProcessor pathSvc = _container.Resolve<IPathProcessor>();
            string p = pathSvc.GetDataDir() + "JsReport/";
            string dataDir = pathSvc.Map(ref p);
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            var rs = new LocalReporting()
                             .UseBinary(OS.IsLinux() ?
                                        jsreport.Binary.Linux.JsReportBinary.GetBinary()
                                        : jsreport.Binary.JsReportBinary.GetBinary())
                            .RunInDirectory(dataDir)
                            .AsUtility().Create();

            var report = rs.RenderAsync(new RenderRequest()
            {
                Template = new Template()
                {
                    Recipe = Recipe.ChromePdf,
                    Engine = Engine.None,
                    Content = html
                }
            }).GetAwaiter().GetResult();

            report.Content.CopyTo(stream);
        }
    }
}
