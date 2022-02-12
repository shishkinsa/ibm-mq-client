using System;
using System.Collections;
using IBM.WMQ;

namespace MQSample
{
    class Program
    {
        // Queue manager QM1
        // Queue DEV.QUEUE.1
        // Channel: DEV.APP.SVRCONN
        // Listener: SYSTEM.LISTENER.TCP.1 on port 1414

        // The type of connection to use, this can be:-
        // MQC.TRANSPORT_MQSERIES_BINDINGS for a server connection.
        // MQC.TRANSPORT_MQSERIES_CLIENT for a non-XA client connection
        // MQC.TRANSPORT_MQSERIES_XACLIENT for an XA client connection
        // MQC.TRANSPORT_MQSERIES_MANAGED for a managed client connection
        const String connectionType = MQC.TRANSPORT_MQSERIES_CLIENT;

        // Define the name of the queue manager to use (applies to all connections)
        const String QManager = "QM1";

        // Define the name of your host connection (applies to client connections only)
        const String HostName = "localhost(1414)"; //192.168.1.124:1414

        // Define the name of the channel to use (applies to client connections only)
        const String Channel = "DEV.APP.SVRCONN";

        /// <summary>
        /// Initialise the connection properties for the connection type requested
        /// </summary>
        /// <param name="connectionType">One of the MQC.TRANSPORT_MQSERIES_ values</param>
        static Hashtable Init(String connectionType)
        {
            Hashtable  connectionProperties = new Hashtable();

            // Add the connection type
            connectionProperties.Add(MQC.TRANSPORT_PROPERTY, connectionType);

            // Set up the rest of the connection properties, based on the
            // connection type requested
            switch (connectionType)
            {
                case MQC.TRANSPORT_MQSERIES_BINDINGS:
                    break;
                case MQC.TRANSPORT_MQSERIES_CLIENT:
                case MQC.TRANSPORT_MQSERIES_XACLIENT:
                case MQC.TRANSPORT_MQSERIES_MANAGED:
                    connectionProperties.Add(MQC.HOST_NAME_PROPERTY, HostName);
                    connectionProperties.Add(MQC.CHANNEL_PROPERTY, Channel);
                    break;
            }

            return connectionProperties;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            try
            {
                Hashtable  connectionProperties = Init(connectionType);

                // Create a connection to the queue manager using the connection
                // properties just defined
                MQQueueManager  qMgr = new MQQueueManager(QManager, connectionProperties);

                // Set up the options on the queue we want to open
                int openOptions = MQC.MQOO_INPUT_AS_Q_DEF | MQC.MQOO_OUTPUT;

                // Now specify the queue that we want to open,and the open options
                var systemDefaultLocalQueue =
                    qMgr.AccessQueue("SYSTEM.DEFAULT.LOCAL.QUEUE", openOptions);

                // Define an IBM MQ message, writing some text in UTF format
                var helloWorld = new MQMessage();
                helloWorld.WriteUTF("Hello World!");

                // Specify the message options
                var pmo = new MQPutMessageOptions(); // accept the defaults,
                // same as MQPMO_DEFAULT

                // Put the message on the queue
                systemDefaultLocalQueue.Put(helloWorld, pmo);


                // Get the message back again

                // First define an IBM MQ message buffer to receive the message
                var retrievedMessage = new MQMessage();
                retrievedMessage.MessageId = helloWorld.MessageId;

                // Set the get message options
                var gmo = new MQGetMessageOptions(); //accept the defaults
                //same as MQGMO_DEFAULT

                // Get the message off the queue
                systemDefaultLocalQueue.Get(retrievedMessage, gmo);

                // Prove we have the message by displaying the UTF message text
                String msgText = retrievedMessage.ReadUTF();
                Console.WriteLine("The message is: {0}", msgText);

                // Close the queue
                systemDefaultLocalQueue.Close();

                // Disconnect from the queue manager
                qMgr.Disconnect();
            }

            //If an error has occurred,try to identify what went wrong.

            //Was it an IBM MQ error?
            catch (MQException ex)
            {
                Console.WriteLine("An IBM MQ error occurred: {0}", ex.ToString());
            }

            catch (System.Exception ex)
            {
                Console.WriteLine("A System error occurred: {0}", ex.ToString());
            }

            return 0;
        }
    }
}