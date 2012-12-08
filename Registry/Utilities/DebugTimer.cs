using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Registry.Utilities
{
    /// <summary>
    /// Helper class to print debug timings of an operation. To be used with using-expression.
    /// </summary>
    class DebugTimer : IDisposable
    {
        private DateTime _startTime = DateTime.Now;
        private string _name;

        /// <summary>
        /// Construct new timer with some name that identifies the operation.
        /// </summary>
        /// <param name="name"></param>
        public DebugTimer(string name)
        {
            _name = name;
        }

        public void Dispose()
        {
            TimeSpan ts = DateTime.Now - _startTime;
            Debug.Print("{0} completed in {1}", _name, ts);
        }
    }
}
