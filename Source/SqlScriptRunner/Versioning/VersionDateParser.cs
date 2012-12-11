using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlScriptRunner.Versioning
{
    public class VersionDateParser : IParseVersions
    {
        public Version Parse(string version)
        {
            if (string.IsNullOrEmpty(version))
                new System.ArgumentNullException("version", "Version was null or empty.");
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
            if (vParts.Length > 0) v.Major = ParsingTools.ParseIndex(vParts, 0);
            if (vParts.Length > 1) v.Minor = ParsingTools.ParseIndex(vParts, 1);
            if (vParts.Length > 2) v.Build = ParsingTools.ParseIndex(vParts, 2);
            int year = 0;
            if (vParts.Length > 3) year = ParsingTools.ParseIndex(vParts, 3);
            int month = 0;
            if (vParts.Length > 4) month = ParsingTools.ParseIndex(vParts, 4);
            int day = 0;
            if (vParts.Length > 5) day = ParsingTools.ParseIndex(vParts, 5);

            if (year > 0 && month > 0 && day > 0 && month < 13 && day < 32) v.DateTime = new DateTime(year, month, day);

            return v;
        }

        /*
        /// <summary>
        /// 1.2.3.YY.MM.DD Description
        /// 1.22.33.YY.MM.DD
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
            int revDot = Version.IndexOf('.', minorDot);
            string rev = Version.Substring(minorDot, revDot - minorDot);

            value = 0;
            if (int.TryParse(rev, out value))
            {
                v.Build = value;
            }

            //pull off the year
            revDot++;
            int yearDot = Version.IndexOf('.', revDot);
            string year = Version.Substring(revDot, yearDot - revDot);
            int iYear = 0;
            value = 0;
            if (int.TryParse(year, out value))
            {
                iYear = value;
            }

            //pull off the month
            yearDot++;
            int monthDot = Version.IndexOf('.', yearDot);
            string month = Version.Substring(yearDot, monthDot - yearDot);
            int iMonth = 0;
            value = 0;
            if (int.TryParse(month, out value))
            {
                iMonth = value;
            }


            v.Description = Version.Substring(revDot + 1).Trim();

            if (v.Description == "") v.Description = null;

            return v;
        }
        */
    }
}
