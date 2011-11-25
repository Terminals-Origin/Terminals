using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Terminals
{
    static partial class Program
    {
        /// <summary>
        /// Represents the program assembly info object.
        /// </summary>
        public static class Info
        {
            private static Assembly aAssembly = Assembly.GetExecutingAssembly();

            public static string DLLVersion
            {
                get
                {
                    return aAssembly.GetName().Version.ToString(); ;
                }
            }
            public static string AboutText
            {
                get
                {
                    return string.Format("{0} ({1}) - {2}", Info.TitleVersion, Info.Description, Info.BuildDate.ToShortDateString());
                }
            }

            public static DateTime BuildDate
            {
                get
                {
                    return RetrieveLinkerTimestamp();
                }
            }

            public static string Description
            {
                get
                {
                    AssemblyDescriptionAttribute desc = (AssemblyDescriptionAttribute)AssemblyDescriptionAttribute.GetCustomAttribute(aAssembly, typeof(AssemblyDescriptionAttribute));
                    return desc.Description;
                }
            }

            public static string Title
            {
                get
                {
                    AssemblyTitleAttribute title = (AssemblyTitleAttribute)AssemblyTitleAttribute.GetCustomAttribute(aAssembly, typeof(AssemblyTitleAttribute));
                    return title.Title;
                }
            }

            public static string TitleVersion
            {
                get
                {
                    return string.Format("{0} {1}", Title, VersionString);
                }
            }

            public static string VersionString
            {
                get
                {
                    return Program.TerminalsVersion;
                }
            }

            public static Version Version
            {
                get
                {
                    return Assembly.GetExecutingAssembly().GetName().Version;
                }
            }

            /// <summary>
            /// Taken from http://stackoverflow.com/questions/1600962/c-displaying-the-build-date
            /// (code by Joe Spivey)
            /// </summary>
            /// <returns></returns>
            private static DateTime RetrieveLinkerTimestamp()
            {
                string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;
                byte[] b = new byte[2048];
                System.IO.Stream s = null;

                try
                {
                    s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    s.Read(b, 0, 2048);
                }
                finally
                {
                    if (s != null)
                        s.Close();
                }

                int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
                int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds(secondsSince1970);
                dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
                return dt;
            }
        }
    }
}
