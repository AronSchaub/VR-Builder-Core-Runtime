namespace VRBuilder.Core.Utils.Logging
{
    public interface ILifeCycleLoggingConfiguration
    {
        bool LogBehaviors { get; }
        bool LogConditions { get; }
        bool LogChapters { get; }
        bool LogSteps { get; }
        bool LogTransitions { get; }
        bool LogDataPropertyChanges { get; }
        bool LogLockState { get; }
    }
}