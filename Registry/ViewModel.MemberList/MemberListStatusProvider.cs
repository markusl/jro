using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using Registry.Interfaces;

namespace Registry.ViewModel
{
    /// <summary>
    /// Provides the interface for implementing status display for a list view.
    /// </summary>
    public interface IMemberListStatus
    {
        string Status { get; }
    }

    /// <summary>
    /// Implement and export the default status bar display.
    /// </summary>
    [Export(typeof(IMemberListStatus))]
    public class DefaultMemberListStatus : IMemberListStatus
    {
        IMemberListView _memberList;

        /// <summary>
        /// Construct new instance of the class to provide the default status bar text.
        /// </summary>
        /// <param name="memberList"></param>
        [ImportingConstructor]
        public DefaultMemberListStatus(IMemberListView memberList)
        {
            _memberList = memberList;
        }

        /// <summary>
        /// Get the status bar text.
        /// </summary>
        public string Status
        {
            get
            {
                IEnumerable<MemberViewModel> members = _memberList.AllItems.Where(m => m.IsMember);

                var info = new Tuple<int, double>(0, 0.0);
                if (_memberList.SelectedItems.Count == 0 && members.Count() > 0)
                {
                    info = GetMemberCountAndAge(members);
                }
                else if (_memberList.SelectedItems.Count() > 0)
                {
                    info = GetMemberCountAndAge(_memberList.SelectedItems);
                }
                return String.Format("{0} jäsentä, keski-ikä: {1:00.0}", info.Item1, info.Item2);
            }
        }

        private static Tuple<int, double> GetMemberCountAndAge(IEnumerable<MemberViewModel> items)
        {
            return new Tuple<int, double>(items.Count(),
                                          items.Select(member => member.Age).Average());
        }
    }
}
