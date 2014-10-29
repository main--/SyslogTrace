using System;
using System.Diagnostics;
using Mono.Unix.Native;

namespace SyslogTrace
{
	public class SyslogTraceListener : TraceListener
	{
		private readonly NativeString LoggerName;

		public SyslogTraceListener(string name)
		{
			LoggerName = new NativeString(name);
			Syscall.openlog(LoggerName.Pointer, SyslogOptions.LOG_PID, SyslogFacility.LOG_DAEMON);
		}

		~SyslogTraceListener()
		{
			Dispose();
		}

		public void DisposeThis()
		{
			if (!LoggerName.Disposed) {
				WriteLine("going down", "SyslogTraceListener");
				Syscall.closelog();
				LoggerName.Dispose();
			}
		}

		protected override void Dispose(bool disposing)
		{
			DisposeThis();
			base.Dispose(disposing);
		}

		public override bool IsThreadSafe { get { return true; } }

		public override void Write(string message)
		{
			WriteLine(message);
		}

		public override void WriteLine(string message)
		{
			Syscall.syslog(SyslogLevel.LOG_INFO, message);
		}
	}
}

