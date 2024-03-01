using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Better.Logger.Runtime
{
    public class JumpStackFrame : IDisposable
    {
        private StackFrame _stackFrame;
        private int _jumpCount;
        private string _message;
        
        //TODO: Maybe move to Settings
        private const int MaxEvaluateDepth = 5;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Jump(int jumpCount = 1)
        {
            _jumpCount += jumpCount;
        }

        public JumpStackFrame()
        {
            Jump();
        }

        public StackFrame GetStackFrame()
        {
            Jump();
            var original = new StackFrame(_jumpCount, false);

            _stackFrame = EvaluateFrame(original, _jumpCount) ?? original;

            return _stackFrame;
        }

        private StackFrame EvaluateFrame(StackFrame original, int jumpCount)
        {
            var bufferDepth = 0;
            while (bufferDepth <= MaxEvaluateDepth)
            {
                bufferDepth++;
                if (original == null) return null;
                var methodBase = original.GetMethod();
                if (methodBase == null || !ShouldBeExcluded(methodBase)) return original;
                jumpCount++;
                var bufferStack = new StackFrame(jumpCount, false);

                original = bufferStack;
            }

            return null;
        }

        private static bool ShouldBeExcluded(MethodBase methodBase)
        {
            return methodBase.GetCustomAttribute<DebuggerNonUserCodeAttribute>() != null || methodBase.GetCustomAttribute<DebuggerHiddenAttribute>() != null;
        }

        public MethodBase GetMethod()
        {
            Jump();
            return GetStackFrame().GetMethod();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _stackFrame = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}