using System;
using System.Text;

namespace Terminals.Converters
{
    internal static class TextConverter
    {
        internal static String EncodeTo64(String toEncode)
        {
            if (String.IsNullOrEmpty(toEncode))
                return null;

            Byte[] toEncodeAsBytes = Encoding.Unicode.GetBytes(toEncode);
            String returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        internal static String DecodeFrom64(String encodedData)
        {
            try
            {
                return TryDecodeFrom64(encodedData);
            }
            catch (FormatException)
            {
                // the text wasnt encoded, so the original (issue when upgrading from older file)
                return encodedData;
            }
        }

        private static string TryDecodeFrom64(string encodedData)
        {
            if (String.IsNullOrEmpty(encodedData))
                return null;

            Byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
            String returnValue = Encoding.Unicode.GetString(encodedDataAsBytes);
            return returnValue;
        }
    }
}
