using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.Wizard
{
    internal class PasswordStrength
    {
        public static int Strength(string Password)
        {
            int finalStrength=0;
            //returns an int, from 0 to 100 on the strength of the given password
            if(Password.Length == 0 || Password.Trim().Length == 0) return 0;

            if(Password.Length>5)  finalStrength = finalStrength + (Password.Length * 10);

            bool hasUpper = false;
            bool hasLower = false;
            bool hasNumber = false;
            bool hasSpecial = false;
            foreach(char letter in Password)
            {
                if(letter < 47 && letter > 33) hasSpecial = true;
                if(letter < 57 && letter > 48) hasNumber = true;
                if(letter < 64 && letter > 58) hasSpecial = true;
                if(letter < 90 && letter > 65) hasUpper = true;
                if(letter < 96 && letter > 91) hasSpecial = true;
                if(letter < 122 && letter > 97) hasLower = true;
                if(letter < 126 && letter > 123) hasSpecial = true;
            }

            //10 points for each special character
            if(hasSpecial) finalStrength += 10;
            //5 if it has a number
            if(hasNumber) finalStrength += 5;
            //10 if it has both upper and lower
            if(hasUpper && hasLower) finalStrength += 10;

            if(finalStrength > 75)
            {
                //max out the rating if they dont meet these minimums
                if(!hasSpecial) finalStrength = 75;
                if(!hasNumber) finalStrength = 75;
                if(!hasUpper && !hasLower) finalStrength = 75;
            }

            //max is always 100, min is 0
            if(finalStrength > 100) finalStrength = 100;
            if(finalStrength == 0) finalStrength = Password.Length;
            return finalStrength;
        }
    }
}
