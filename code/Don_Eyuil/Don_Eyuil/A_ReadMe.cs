using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_DnspyUserReadMe
{
    public class A_ReadMe
    {
        public static string Note = "If you're using a decompiler like DnSpy to view this text,";
        public static string Note2 = "please consider checking out the open-source code for this project on GitHub:https://github.com/mynameiscjh/MWTK/tree/main";
        public static string Note3 = "You'll find more detailed comments and more sensible (non-decompiled) syntax there.";
        public static string Heart = "<3";

        A_ReadMe(string You,ref string Me,ref string YourUnderstandingDifficulty)
        {
            string Github = "https://github.com/mynameiscjh/MWTK/tree/main";
            Me = You != Github ? "Sad" : "Happy";
            YourUnderstandingDifficulty = You == Github ? "Easy" : "Hard";
        }
    }
}
