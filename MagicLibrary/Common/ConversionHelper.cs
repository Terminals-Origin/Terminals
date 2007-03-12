// *****************************************************************************
// 
//  (c) Crownwood Consulting Limited 2002 
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Crownwood Consulting 
//	Limited, Haxey, North Lincolnshire, England and are supplied subject to 
//	licence terms.
// 
//  Magic Version 1.7 	www.dotnetmagic.com
// *****************************************************************************

using System;
using System.Text;
using System.Drawing;

namespace Crownwood.Magic.Common
{
    public class ConversionHelper
    {
		// Faster performance to cache the converters and type objects, rather
		// than keep recreating them each time a conversion is required
		protected static SizeConverter _sc = new SizeConverter();
		protected static PointConverter _pc = new PointConverter();
		protected static Type _stringType = Type.GetType("System.String");

		public static string SizeToString(Size size)
		{
			return (string)_sc.ConvertTo(size, _stringType);
		}

		public static Size StringToSize(string str)
		{
			return (Size)_sc.ConvertFrom(str);
		}

		public static string PointToString(Point point)
		{
			return (string)_pc.ConvertTo(point, _stringType);
		}

		public static Point StringToPoint(string str)
		{
			return (Point)_pc.ConvertFrom(str);
		}
    }
}