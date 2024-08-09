using System.Diagnostics;
using Serilog;

namespace ProfilingSystem;

public static class Profiler
{
    #region Public static methods

    [Conditional("DEBUG")]
    [Conditional("PROFILE")]
    public static void AddToStack(string? methodName)
    {
        if (methodName is null)
        {
            return;
        }
        
        MethodCallData methodCallData = new()
        {
            MethodName = methodName,
            MethodStartCallTime = DateTime.Now,
        };

        s_stack.Push(methodCallData);

        Log.Information("Starting profiling on '{methodName}'", methodName);
    }

    [Conditional("DEBUG")]
    [Conditional("PROFILE")]
    public static void RemoveFromStack(string? methodName)
    {
        if (methodName is null)
        {
            return;
        }

        MethodCallData methodCallData = s_stack.Pop();
        int methodExecTimeMs = (int)(DateTime.Now - methodCallData.MethodStartCallTime).TotalMilliseconds;

        if (methodCallData.MethodName != methodName)
        {
            Log.Error("Given method name doesn't match stack's top-most method. Input: '{inputMethodName}', stack: '{stackMethodName}'",
                methodName,
                methodCallData.MethodName);
            Debug.Assert(false);
        }

        Log.Information("Executed '{methodName}' in {methodExecTimeMs} ms", methodCallData.MethodName, methodExecTimeMs);
    }

    #endregion

    #region Private static fields

    private static readonly Stack<MethodCallData> s_stack = new();

    #endregion
}