using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Export;
using Database;
using System.ComponentModel;

namespace ExportTest
{
    [TestClass]
    public class ExportTest : ExportTestBase
    {
        private static bool _exportCompleted = false;

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
        public void TestMemberListExport()
        {
            MemberListExport mle = new MemberListExport();
            mle.ProgressChanged += delegate(object o, ProgressChangedEventArgs pcea) { if (pcea.ProgressPercentage == 100) _exportCompleted = true; };
            mle.Members = _handler.Database.MemberSet.AsEnumerable();
            mle.OutputPath = _testOutputName;
            mle.Export();
            Assert.IsTrue(_exportCompleted);
        }

        [TestMethod]
        public void TestMemberAddressExport()
        {
            MemberAddressExport mle = new MemberAddressExport();
            mle.ProgressChanged += delegate(object o, ProgressChangedEventArgs pcea) { if (pcea.ProgressPercentage == 100) _exportCompleted = true; };
            mle.Members = _handler.Database.MemberSet.AsEnumerable();
            mle.OutputPath = _testOutputName;
            mle.Export();
            Assert.IsTrue(_exportCompleted);
        }

        [TestMethod]
        public void TestMemberGroupExport()
        {
            GroupMembersExport mle = new GroupMembersExport();
            mle.ProgressChanged += delegate(object o, ProgressChangedEventArgs pcea) { if (pcea.ProgressPercentage == 100) _exportCompleted = true; };
            mle.Members = _handler.Database.MemberSet.AsEnumerable();
            mle.OutputPath = _testOutputName;
            mle.Export();
            Assert.IsTrue(_exportCompleted);
        }
    }
}
