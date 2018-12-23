using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DJIWifiBridge
{
    class WifiBridge
    {
        Socket socket;
        bool connected = false;
        private static WifiBridge _instance;
        Semaphore available;
        LinkedList<string> cmdList;
        Thread dataThread;
        public static WifiBridge getInstance()
        {
            if (_instance == null)
            {
                _instance = new WifiBridge();
            }
            return _instance;
        }
        public WifiBridge()
        {
            available = new Semaphore(0, 99999);
            cmdList = new LinkedList<string>();
        }

        public bool Connect(string ip, int port)
        {
            if (connected) {
                return false;
            }
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            this.connected = socket.Connected;
            dataThread = new Thread(runThread);
            dataThread.Start();
            return socket.Connected;
        }

        public bool Disconnect()
        {
            if (!connected)
                return true;
            socket.Close();
            this.connected = false;
            dataThread.Abort();
            socket = null;
            return true;
        }
        public void sendGimbalSpeed(float x, float y, float z)
        {
            string msg = string.Format("{0} {1} {2} {3}", "gimbal", x, y, z);
            sendmsg(msg); 
        }

        public void sendRCData(float roll, float pitch, float yaw, float throtte) {
            string msg = string.Format("{0} {1} {2} {3} {4}", "rc", roll, pitch, yaw, throtte);
            sendmsg(msg);
        }

        void runThread() {
            byte[] buf = new byte[1024];
            while (connected)
            {
                socket.Receive(buf);
                Console.Write(Encoding.ASCII.GetString(buf));
            }
        }
        private void sendmsg(string msg)
        {
            if (!connected)
                return;
            socket.Send(Encoding.ASCII.GetBytes(msg + "\n"));
            //Console.WriteLine(msg);
            //cmdList.AddLast(msg);
            //available.Release();
        }

        public void disableRC()
        {
            string msg = "rc disable";
            sendmsg(msg);
        }

        public void enableRC()
        {
            string msg = "rc enable";
            sendmsg(msg);
        }

        public void resetGimbal()
        {
            string msg = "gimbal reset";
            sendmsg(msg);
        }

        public void takeOff()
        {
            string msg = "takeoff";
            sendmsg(msg);
        }

        public void landing()
        {
            string msg = "landing";
            sendmsg(msg); 
        }

        public void takePhoto() {
            sendmsg("takephoto");
        }
    }
}
