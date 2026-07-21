namespace VRBuilder.Core.ProcessRunning
{
    /// <summary>
    /// Every Service by the <see cref="VRBuilder.Core.Configuration.VRBuilderServices"/> needs this Interface to be implemented.
    /// It will get it's Configuration to be set, and then it will be initialized.
    /// </summary>
    public interface IVRBService
    {
        /// <summary>
        /// Configuration callback. The services needs to cast it to it's own Configuration Interface.
        /// </summary>
        /// <param name="configuration"></param>
        void SetConfiguration(object configuration);

        /// <summary>
        /// Initialisation callback. This is just to make sure the service can do some basic bootstrapping after the config set and before the Process run.
        /// </summary>
        void Initialize();
    }
}