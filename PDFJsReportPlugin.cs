using Crossbill.Common;
using Crossbill.Common.Plugins;
using Crossbill.Common.Processors;
using Crossbill.Plugins.PDF.JsReport.Processors;
using System.ComponentModel.Composition;

namespace Crossbill.Plugins.PDF.JsReport
{
    [Export(typeof(IPlugin))]
    public class PDFJsReportPlugin : APlugin
    {
        public override void RegisterApplicationContextTypes(ICrossContainer container)
        {
            base.RegisterApplicationContextTypes(container);

            container.RegisterType<IPdfProcessor, PdfProcessor>(CrossTypeLifetime.Hierarchical);
        }
    }
}
