namespace AdventureLandCore.Domain
{
    /// <summary>
    /// Enumeration that defines the different types that a sentence token can be.
    /// </summary>
    public enum WordType
    {
        /// <summary>
        /// Word is not yet identified
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Word is an adventure command
        /// </summary>
        Command=1,
        /// <summary>
        /// Word is a replaced command for a one word command such as North
        /// </summary>
        ReplacedCommand = 2,
        /// <summary>
        /// Noise word such as at/to
        /// </summary>
        StopWord = 3,
    }
}