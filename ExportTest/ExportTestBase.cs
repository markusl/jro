using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using System.IO;

namespace ExportTest
{
    public abstract class ExportTestBase
    {
        protected DatabaseHandler _handler;
        protected string _testDatabaseName = Path.GetTempFileName();
        protected const string _testOutputName = "./Export.pdf";

        protected void _Initialize()
        {
            _handler = new DatabaseHandler(_testDatabaseName, "password");
            _handler.Open();
            CreateMembers(300);
        }

        protected void _CleanUp()
        {
            if (_handler != null)
                _handler.CloseDatabase();
            File.Delete(_testDatabaseName);
        }

        protected void CreateMembers(int number)
        {
            while (number-- > 0)
            {
                Member member = Member.CreateMember(0, "Aadolf", "Anderson", "", "", "", "", "", DateTime.Now.ToString());
                member.Address = Address.CreateAddress(0, "", "", "", "");
                member.MemberDetais = MemberDetais.CreateMemberDetais(0, "", "", "", "", "", "", "", "");
                _handler.Database.MemberSet.AddObject(member);
            }
            _handler.Database.SaveChanges();
        }
    }
}
