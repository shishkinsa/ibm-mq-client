using System;
using System.Collections;

using IBM.WMQ;

namespace MQSample
{
    class Program 
    {
        public static void Main(string[] args)
        {
            // The type of connection to use, this can be:-
            // MQC.TRANSPORT_MQSERIES_BINDINGS for a server connection.
            // MQC.TRANSPORT_MQSERIES_CLIENT for a non-XA client connection
            // MQC.TRANSPORT_MQSERIES_XACLIENT for an XA client connection
            // MQC.TRANSPORT_MQSERIES_MANAGED for a managed client connection
            const String connectionType = MQC.TRANSPORT_MQSERIES_CLIENT;
            
            
        }
    }
}

