using System;


namespace Lw.Diagnostics
{
    public interface ITraceManager
    {
        TraceScope StartActivity(string activityName);
        TraceScope StartActivity(string activityName, Guid activityId);
    }
}
