using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.IO;

namespace DatabaseTest
{
    [TestClass]
    public class DatabaseTestBase
    {
        protected string _testDatabaseName = Path.GetTempFileName();
        protected DatabaseHandler _handler;
        protected MembersContainer _container;

        [TestInitialize]
        public virtual void Initialize()
        {
            _handler = new DatabaseHandler(_testDatabaseName, "password");
            _handler.Open();
            _container = _handler.Database;
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            try
            {
                _handler.CloseDatabase();
            }
            catch (Exception) { }
            try
            {
                File.Delete(_testDatabaseName);
            }
            catch (Exception) { }
        }
    }
}
