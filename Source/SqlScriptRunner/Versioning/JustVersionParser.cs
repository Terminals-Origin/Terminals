using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Versioning
{
    public class JustVersionParser : IParseVersions
    {
        public Version Parse(string version)
        {
            Version v = new Version();
            version = version.Trim();
            string[] parts = version.Split(' ');
            string versionInfo = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                v.Description += parts[i] + " ";
            }
            if (v.Description != null) v.Description = v.Description.Trim();

            string[] vParts = versionInfo.Split('.');
            v.Major = ParsingTools.ParseIndex(vParts, 0);
            if (vParts.Length > 1) v.Minor = ParsingTools.ParseIndex(vParts, 1);
            if (vParts.Length > 2) v.Build = ParsingTools.ParseIndex(vParts, 2);


            return v;
        }

        /*
        /// <summary>
        /// 1.2.3 Description
        /// 1.22.33
        /// 999.3363.4534
        /// </summary>
        /// <param name="Version"></param>
        /// <returns></returns>
        public Version Parse(string Version)
        {
            Version v = new Version();
            //pull off the major
            int majorDot = Version.IndexOf('.');
            string major = Version.Substring(0, majorDot);

            int value = 0;
            if (int.TryParse(major, out value))
            {
                v.Major = value;
            }


            //pull off the minor
            majorDot++;
            int minorDot = Version.IndexOf('.', majorDot);
            string minor = Version.Substring(majorDot, minorDot - majorDot);


            value = 0;
            if (int.TryParse(minor, out value))
            {
                v.Minor = value;
            }

            //pull off the revision
            minorDot++;
            int revDot = Version.IndexOf(' ', minorDot);
            string rev = Version.Substring(minorDot, revDot - minorDot);


            value = 0;
            if (int.TryParse(rev, out value))
            {
                v.Build = value;
            }

            v.Description = Version.Substring(revDot + 1).Trim();

            if (v.Description == "") v.Description = null;

            return v;
        }
*/
    }
}
