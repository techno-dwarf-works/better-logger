using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Better.Logger.Runtime
{
    public static class BetterLogger
    {
        [DebuggerHidden][DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(string message, 
            [CallerMemberName] string memberName = "", 
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            
        }
    }
}