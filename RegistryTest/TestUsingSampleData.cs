using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using Import;
using System.IO;

namespace RegistryTest
{
    /// <summary>
    /// Base class for tests that need some sample users in the database.
    /// </summary>
    [TestClass]
    public class TestUsingSampleData
    {
        protected const string _deploymentItem = "TestData/SampleDataSource.xml";
        protected DatabaseHandler _handler;
        protected string _testDatabaseName = Path.GetTempFileName();
        protected const int _numberOfMembersInTestDatabase = 85;

        //[TestInitialize]
        // Don't initialize automatically because the test data file has not been yet deployed
        /// <summary>
        /// Will initialize a test database with 85 users.
        /// </summary>
        public virtual void Initialize()
        {
            _handler = new DatabaseHandler(_testDatabaseName, "password");
            _handler.Open();
            new SampleXmlImport().Import(new FileInfo(_deploymentItem).Name, _handler.Database);
        }

        /// <summary>
        /// Close the database and remove database file from disk.
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            if(_handler != null)
                _handler.CloseDatabase();
            File.Delete(_testDatabaseName);
        }
    }
}
