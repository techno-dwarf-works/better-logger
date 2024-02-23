using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Better.Logger.Runtime;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class BetterLogger
{

    #region Log

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void Log(string message)
    {
        LogTypeInternal(LogType.Log, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void Log(object message)
    {
        LogTypeInternal(LogType.Log, message, null);
    }

    [DebuggerNonUserCode]
    public static void Log(string message, Object context)
    {
        LogTypeInternal(LogType.Log, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void Log(object message, Object context)
    {
        LogTypeInternal(LogType.Log, message, context);
    }

    #endregion

    #region Warning

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogWarning(string message)
    {
        LogTypeInternal(LogType.Warning, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogWarning(object message)
    {
        LogTypeInternal(LogType.Warning, message, null);
    }

    [DebuggerNonUserCode]
    public static void LogWarning(string message, Object context)
    {
        LogTypeInternal(LogType.Warning, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogWarning(object message, Object context)
    {
        LogTypeInternal(LogType.Warning, message, context);
    }

    #endregion

    #region Error

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogError(string message)
    {
        LogTypeInternal(LogType.Error, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogError(object message)
    {
        LogTypeInternal(LogType.Error, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogError(string message, Object context)
    {
        LogTypeInternal(LogType.Error, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogError(object message, Object context)
    {
        LogTypeInternal(LogType.Error, message, context);
    }

    #endregion

    #region Exception

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(Exception exception)
    {
        LogExceptionInternal(exception, exception.Message,null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(Exception exception, Object context)
    {
        LogExceptionInternal(exception, exception.Message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException<T>(object message) where T : Exception, new()
    {
        var exception = new T();
        LogExceptionInternal(exception, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException<T>(object message, Object context) where T : Exception, new()
    {
        var exception = new T();
        LogExceptionInternal(exception, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException<T>(string message) where T : Exception, new()
    {
        var exception = new T();
        LogExceptionInternal(exception, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException<T>(string message, Object context) where T : Exception, new()
    {
        var exception = new T();
        LogExceptionInternal(exception, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(object message)
    {
        var exception = new Exception();
        LogExceptionInternal(exception, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(object message, Object context)
    {
        var exception = new Exception();
        LogExceptionInternal(exception, message, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(string message)
    {
        var exception = new Exception();
        LogExceptionInternal(exception, message, null);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    public static void LogException(string message, Object context)
    {
        var exception = new Exception();
        LogExceptionInternal(exception, message, context);
    }

    #endregion

    #region Internal

    [DebuggerHidden]
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogExceptionInternal(Exception exception, object message, Object context)
    {
        LogBuilder.BuildLogException(exception, message);
        Debug.unityLogger.LogException(exception, context);
    }

    [DebuggerHidden]
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void LogTypeInternal(LogType logType, object message, Object context)
    {
        Debug.unityLogger.Log(logType, LogBuilder.BuildLogObject(message), context);
    }

    #endregion
}