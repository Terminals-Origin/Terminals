using System;

namespace rpaulo.toolbar
{
	[Flags]
	public enum AllowedBorders
	{
		None	= 0x00, // Only floating
		Top		= 0x01,
		Left	= 0x02,
		Bottom	= 0x04,
		Right	= 0x08,
		All		= Top|Left|Bottom|Right
	}
}
