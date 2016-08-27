using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    public class Client
    {
        TcpClient sender;
        IFormatter formatter;
        NetworkStream stream;
        
        public Client(string endPointIP, int endPointPort)
        {

            try
            {
                sender = new TcpClient(endPointIP, endPointPort);
                stream = sender.GetStream();
                formatter = new BinaryFormatter();
            }
            catch (SocketException se)
            {

                Console.WriteLine("A socket exception has occured : {0}", se.Message);

            }

        }

        public void sendData(Packet obj)
        {
            try
            {
               formatter.Serialize(stream, obj);
               stream.Flush();
            }
            catch (SocketException se)
            {

                Console.WriteLine("A socket exception has occured : {0}", se.Message);

            }
        }

        public void RecvData(ref Packet obj)
        {
            while (stream.DataAvailable)
            {
                obj = (Packet)formatter.Deserialize(stream);

            }

        }

        public void Disconnect()
        {
            this.sender.Close();
        }

    }
}
