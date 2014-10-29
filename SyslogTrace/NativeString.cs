using System;
using System.Text;
using System.Runtime.InteropServices;

namespace SyslogTrace
{
	public class NativeString : IDisposable
	{
		private readonly byte[] Chars;
		private GCHandle CharHandle;
		public IntPtr Pointer { get { return CharHandle.AddrOfPinnedObject(); } }
		public bool Disposed { get { return !CharHandle.IsAllocated; } }

		public NativeString(string s)
		{
			Chars = Encoding.ASCII.GetBytes(s + "\0");
			CharHandle = GCHandle.Alloc(Chars, GCHandleType.Pinned);
		}

		~NativeString()
		{
			Dispose();
		}

		public void Dispose()
		{
			if (CharHandle.IsAllocated)
				CharHandle.Free();
		}
	}
}

