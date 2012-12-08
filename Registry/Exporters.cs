using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;
using Database;
using Registry.Interfaces;
using System.Globalization;

namespace Registry
{
    static class Exporters
    {
        private static string FormatFileNameWithDate(string name)
        {
            string fileName = String.Format(CultureInfo.CurrentCulture,
                                    "{0}{1}{2}-{3}.pdf",
                                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                    Path.DirectorySeparatorChar,
                                    name,
                                    DateTime.Now.AsDate());
            return fileName;
        }

        internal static void ExportAllMemberGroupLists(IMainWindow mainWindow)
        {
            MembersContainer container = mainWindow.ActiveDatabase;
            var data = container.MemberSet.AsEnumerable();
            foreach (string group in data.Select(member => member.MemberDetais.membergroup).Distinct())
            {
                ExportSingleMemberGroupList(group, mainWindow);
            }
        }

        internal static void ExportSingleMemberGroupList(string group, IMainWindow mainWindow)
        {
            MembersContainer container = mainWindow.ActiveDatabase;
            Organization org = container.OrganizationSet.Where(o => o.key.Equals("OrgName")).FirstOrDefault();

            Export.GroupMembersExport exporter = new Export.GroupMembersExport();
            exporter.DocumentDate = DateTime.Now.AsDate();
            exporter.OrgName = org.value;

            exporter.Members = container.MemberSet.AsEnumerable().Where(m => m.MemberDetais.membergroup.Equals(group));
            exporter.OutputPath = FormatFileNameWithDate("jasenlista-" + group);
            mainWindow.ExportService.QueueExportJob(exporter);
        }

        internal static void ExportMemberAddress(IMainWindow mainWindow)
        {
            MembersContainer container = mainWindow.ActiveDatabase;
            Organization org = container.OrganizationSet.Where(o => o.key.Equals("OrgName")).FirstOrDefault();

            Export.MemberAddressExport exporter = new Export.MemberAddressExport();
            exporter.DocumentDate = DateTime.Now.AsDate();
            exporter.OrgName = org.value;
            exporter.Members = container.MemberSet.AsEnumerable();
            exporter.OutputPath = FormatFileNameWithDate("jasenluettelo-osoitetiedot");
            mainWindow.ExportService.QueueExportJob(exporter);
        }

        internal static void ExportMemberList(IMainWindow mainWindow)
        {
            MembersContainer container = mainWindow.ActiveDatabase;
            Organization org = container.OrganizationSet.Where(o => o.key.Equals("OrgName")).FirstOrDefault();

            Export.MemberListExport exporter = new Export.MemberListExport();
            exporter.DocumentDate = DateTime.Now.AsDate();
            exporter.OrgName = org.value;
            exporter.Members = container.MemberSet.AsEnumerable().Where(m => m.IsMember());
            exporter.OutputPath = FormatFileNameWithDate("jasenluettelo");
            mainWindow.ExportService.QueueExportJob(exporter);
        }

        internal static void ExportCustom(IMainWindow mainWindow)
        {
            //string fileName = String.Format("{0}{1}export-{2}.pdf",
            //                        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            //                        Path.DirectorySeparatorChar,
            //                        DateTime.Now.AsDate());

            //Export.CsvExport csv = new Export.CsvExport();
            //csv.Export(membersContainer.MemberSet.AsEnumerable(), fileName);
        }
    }
}
