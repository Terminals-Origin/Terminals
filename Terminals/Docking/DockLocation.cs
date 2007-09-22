using System;

namespace rpaulo.toolbar
{
	public enum DockLocation : byte
	{
		Top		 = 1,
		Left	 = 2,
		Bottom	 = 3,
		Right	 = 4,
		Floating = 5
	}

	[Flags]
	public enum AllowedDockLocation : byte
	{
		Top		 = 0x01,
		Left	 = 0x02,
		Bottom	 = 0x04,
		Right	 = 0x08,
		Floating = 0x10
	}
}
