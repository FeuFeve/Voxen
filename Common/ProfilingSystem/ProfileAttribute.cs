using Metalama.Framework.Aspects;

namespace ProfilingSystem;

public class ProfileAttribute : OverrideMethodAspect
{
    #region OverrideMethodAspect overrides

    public override dynamic? OverrideMethod()
    {
        Profiler.AddToStack(meta.Target.Method.Name);

        try
        {
            return meta.Proceed();
        }
        finally
        {
            Profiler.RemoveFromStack(meta.Target.Method.Name);
        }
    }

    #endregion
}
