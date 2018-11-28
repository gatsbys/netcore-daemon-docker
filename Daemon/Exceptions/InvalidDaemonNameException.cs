using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Daemon.Exceptions
{
    [Serializable]
    public class InvalidDaemonNameException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidDaemonNameException()
        {
        }

        public InvalidDaemonNameException(string message) : base(message)
        {
        }

        public InvalidDaemonNameException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidDaemonNameException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
