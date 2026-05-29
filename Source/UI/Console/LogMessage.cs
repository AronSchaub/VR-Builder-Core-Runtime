namespace VRBuilder.UI.Console
{
    /// <summary>
    /// A message logged in a <see cref="LogConsole"/>.
    /// </summary>
    public struct LogMessage
    {
        /// <summary>
        /// The main message.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Additional information provided in the message.
        /// </summary>
        public string Details { get; private set; }

        /// <summary>
        /// Type of message logged.
        /// </summary>
        public LogLevel LogLevel { get; private set; }

        public LogMessage(string message, string details, LogLevel logLevel)
        {
            Message = message;
            Details = details;
            LogLevel = logLevel;
        }
    }
}
