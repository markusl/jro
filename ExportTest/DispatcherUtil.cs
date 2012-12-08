using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Windows.Threading;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    /// <summary>
    /// Runs the WPF message queue in unit tests.
    /// </summary>
    public static class DispatcherUtil
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background,
                new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }

        /// <summary>
        /// Runs the events until the specified event has been set or until the maximum time has elapsed.
        /// Fails the assertion if event is not set in the specified time limit.
        /// </summary>
        /// <param name="evt">The event to wait</param>
        /// <param name="maximum">The maximum amount of time to wait</param>
        public static void DoEventsUntil(AutoResetEvent evt, int maximum = 2500)
        {
            DateTime start = DateTime.Now;

            while (!evt.WaitOne(10))
            {
                DispatcherUtil.DoEvents();
                if ((DateTime.Now - start).TotalMilliseconds > maximum)
                    Assert.Fail("Event {0} was not set in {1} ms", evt, maximum);
            }
        }
    }
}
