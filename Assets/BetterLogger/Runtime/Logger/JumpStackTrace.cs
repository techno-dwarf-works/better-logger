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
            _stackFrame = new StackFrame(_jumpCount, false);
            return _stackFrame;
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