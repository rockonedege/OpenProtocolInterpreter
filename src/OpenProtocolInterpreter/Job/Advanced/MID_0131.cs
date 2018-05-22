﻿namespace OpenProtocolInterpreter.Job.Advanced
{
    /// <summary>
    /// MID: Set Job line control start
    /// Description: The integrator can set the line control start in the controller with this message.
    /// Message sent by: Integrator
    /// Answer: MID 0005 Command accepted
    /// </summary>
    public class MID_0131 : Mid, IAdvancedJob
    {
        private const int LAST_REVISION = 1;
        public const int MID = 131;

        public MID_0131() : base(MID, LAST_REVISION) { }

        internal MID_0131(IMid nextTemplate) : this() => NextTemplate = nextTemplate;
    }
}