using System;
using System.Text;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Export;
using Export.Exporters;
using Export.Interfaces;
using System.Threading;
using UnitTests;

namespace ExportTest
{
    class FakeExport : PdfExport
    {
        public override int PagesPer100Members
        {
            get { return 1; }
        }

        protected override StringBuilder BuildDocument(IEnumerable<Database.Member> members)
        {
            Thread.Sleep(2);
            return new StringBuilder();
        }
    }

    [TestClass]
    public class ExportServiceTest : ExportTestBase
    {
        [TestInitialize]
        public void Initialize()
        {
            base._Initialize();
        }

        [TestCleanup]
        public void CleanUp()
        {
            base._CleanUp();
        }

        [TestMethod]
        public void TestAllExportsCompletedEventIsReported()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            bool _allExportsDone = false;
            IExportService es = new ExportService(Dispatcher.CurrentDispatcher);

            MemberListExport mle = new MemberListExport();
            mle.Members = _handler.Database.MemberSet.AsEnumerable();
            mle.OutputPath = _testOutputName;

            Assert.IsFalse(es.ExportInProgress);
            es.AllExportsCompleted += delegate { _allExportsDone = true; evt.Set(); };
            es.QueueExportJob(mle);

            DispatcherUtil.DoEventsUntil(evt);

            Assert.IsTrue(_allExportsDone);
        }

        [TestMethod]
        public void TestQueueingSeveralExportsReportsOnlyOneExportCompleted()
        {
            AutoResetEvent evt = new AutoResetEvent(false);
            bool _allExportsDone = false;
            IExportService es = new ExportService(Dispatcher.CurrentDispatcher);

            IPdfExport export = new FakeExport();
            export.OutputPath = _testOutputName;

            Assert.IsFalse(es.ExportInProgress);
            es.AllExportsCompleted += delegate {
                Assert.IsFalse(_allExportsDone);
                _allExportsDone = true; // this should be called only once
                evt.Set();
            };

            for (int i = 0; i < 15; ++i)
                es.QueueExportJob(export);

            DispatcherUtil.DoEventsUntil(evt, 8000);

            Assert.IsTrue(_allExportsDone);
        }
    }
}
