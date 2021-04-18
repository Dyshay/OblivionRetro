using EasyHook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EasyHook.RemoteHooking;
using static SocketHook.NativeSocketMethods;

namespace SocketHook
{
    public class Main : IEntryPoint
    {
        private HookInterface _interface;
        private LocalHook _connectHook;
        private ushort _redirectionPort;
        private object wSockLock = new object();

        public const int AF_INET = 2;
        public const int SOCK_STREAM = 1;
        public const int PPROTO_TCP = 6;

        public const long FIONBIO = 0x8004667E;
        public const string INADDR_ANY = "0.0.0.0";

        public const int SOCKET_ERROR = -1;
        public const int INVALID_SOCKET = ~0;

        public const int WSANOERROR = 0;
        public const int WSAEINVAL = 10022;
        public const int WSAEWOULDBLOCK = 10035;
        public const int WSAEALREADY = 10037;
        public const int WSAEISCONN = 10056;
        public const int WSAENOTCONN = 10057;

        public Main(IContext context, string channelName, int redirectionPort)
        {
            _interface = IpcConnectClient<HookInterface>(channelName);
            _redirectionPort = (ushort)redirectionPort;

            //_interface.Ping();
        }

        public void Run(IContext context, string channelName, int redirectionPort)
        {
            _interface.NotifyInstalled(Process.GetCurrentProcess().ProcessName);

            try
            {
                _connectHook = LocalHook.Create(
                    LocalHook.GetProcAddress("Ws2_32.dll", "connect"),
                    new WinsockConnectDelegate(_onConnect), this);

                _connectHook.ThreadACL.SetExclusiveACL(new[] { 0 });
            }
            catch (Exception ex) { _interface.Error(ex); }

            WakeUpProcess();

            try
            {
                while (true)
                {
                    Thread.Sleep(500);
                    _interface.Ping();
                }
            }
            catch
            {
                _connectHook.Dispose();

                LocalHook.Release();
            }
        }

        private int _onConnect(IntPtr s, IntPtr sockAddr, int addrsize)
        {
            //lock (wSockLock)
            //{

            //    // retrieve remote ip
            //    sockaddr_in structure = (sockaddr_in)Marshal.PtrToStructure(sockAddr, typeof(sockaddr_in));
            //    string remoteIp = new System.Net.IPAddress(structure.sin_addr.S_addr).ToString();
            //    ushort remotePort = ntohs(structure.sin_port);
            //    HookManager.Log("Ip: " + remoteIp + " Port: " + remotePort.ToString() + " Addrsize: " + addrsize);
            //    if (true)
            //    {

            //        // connect to socks5 server
            //        //IntPtr test = CreateAddr(proxyIp,proxyPort.ToString());
            //        SetAddr(sockAddr, "191.101.89.189", "45786");

            //        IntPtr val = CreateAddr("191.101.89.189", "45786");

            //        var result = connect(s, val, addrsize);

            //        _interface.IpRedirected(new IPEndPoint(new IPAddress(structure.sin_addr.S_addr), htons(structure.sin_port)), Process.GetCurrentProcess().Id, _redirectionPort);

            //        var strucPtr = Marshal.AllocHGlobal(addrsize);
            //        var struc = new sockaddr_in
            //        {
            //            sin_addr = { S_addr = inet_addr(_interface.GetRedirectedIp()) },
            //            sin_port = htons(_redirectionPort),
            //            sin_family = (short)AddressFamily.InterNetworkv4,
            //        };

            //        Marshal.StructureToPtr(struc, strucPtr, true);
            //        return connect(s, strucPtr, addrsize);

            //        // send socks 5 request
            //        IntPtr socksProtocolRequest = SetUpSocks5Request();

            //        result = -1;
            //        while (result == -1)
            //        {
            //            result = send(s, socksProtocolRequest, 4, 0);
            //            var errorcode = WSAGetLastError();
            //            if (errorcode != WSAENOTCONN && errorcode != WSANOERROR)
            //            {
            //                HookManager.Log("Send failed, Error: + " + errorcode);
            //                return -1;
            //            }
            //            Thread.Sleep(1);
            //        }


            //        // retrieve server repsonse
            //        var response = IntPtr.Zero;
            //        while (response == IntPtr.Zero)
            //        {
            //            response = Recieve(s, 2);
            //            var errorcode = WSAGetLastError();
            //            if (errorcode != WSAEWOULDBLOCK && errorcode != WSANOERROR)
            //            {
            //                HookManager.Log("Recieve FAILED response == IntPtr.Zero! Lasterror: " + errorcode.ToString());
            //                return -1;
            //            }
            //            Thread.Sleep(1);
            //        }

            //        byte[] recvBytes = new byte[2] { Marshal.ReadByte(response), Marshal.ReadByte(response, 1) };
            //        if (recvBytes[1] == 255)
            //        {
            //            HookManager.Log("No authentication method was accepted by the proxy server");
            //            return -1;
            //        }
            //        if (recvBytes[0] != 5)
            //        {
            //            HookManager.Log("No SOCKS5 proxy");
            //            return -1;
            //        }

            //        // if auth request response, send authenicate request
            //        if (recvBytes[1] == 2)
            //        {
            //            int length = 0;
            //            var authenticateRequest = SetUpAuthenticateRequest("Seldylcrel", "V7a2EkY", out length);
            //            result = -1;
            //            while (result == -1)
            //            {
            //                result = send(s, authenticateRequest, length, 0);
            //                var errorcode = WSAGetLastError();
            //                if (errorcode != WSAENOTCONN && errorcode != WSANOERROR)
            //                {
            //                    HookManager.Log("Send failed, Error: + " + errorcode);
            //                    return -1;
            //                }
            //                Thread.Sleep(1);
            //            }


            //            response = IntPtr.Zero;
            //            while (response == IntPtr.Zero)
            //            {
            //                response = Recieve(s, 2);
            //                var errorcode = WSAGetLastError();
            //                if (errorcode != WSAEWOULDBLOCK && errorcode != WSANOERROR)
            //                {
            //                    HookManager.Log("Recieve FAILED response == IntPtr.Zero! Lasterror: " + errorcode.ToString());
            //                    return -1;
            //                }
            //                Thread.Sleep(1);
            //            }

            //            recvBytes = new byte[2] { Marshal.ReadByte(response), Marshal.ReadByte(response, 1) };
            //            if (recvBytes[1] != 0)
            //            {
            //                //HookManager.Log("Proxy: incorrect username/password");
            //                HookManager.Log("Proxy: incorrect username/password");
            //                return -1;
            //            }
            //        }

            //        // request bind with server
            //        var bindRequest = SetUpBindWithRemoteHost(remoteIp, remotePort);
            //        result = -1;
            //        while (result == -1)
            //        {
            //            result = send(s, bindRequest, 10, 0);
            //            var errorcode = WSAGetLastError();
            //            if (errorcode != WSAENOTCONN && errorcode != WSANOERROR)
            //            {
            //                HookManager.Log("Send failed (bindRequest), Error: + " + errorcode);
            //                return -1;
            //            }
            //            Thread.Sleep(1);
            //        }


            //        // bind response
            //        response = IntPtr.Zero;
            //        while (response == IntPtr.Zero)
            //        {
            //            response = Recieve(s, 10);
            //            var errorcode = WSAGetLastError();
            //            if (errorcode != WSAEWOULDBLOCK && errorcode != WSANOERROR)
            //            {
            //                HookManager.Log("Recieve FAILED response == IntPtr.Zero! Lasterror: " + errorcode.ToString());
            //                return -1;
            //            }
            //            Thread.Sleep(1);
            //        }


            //        if (!VerifyBindResponse(response))
            //        {
            //            HookManager.Log("VerifyBindResponse failed!");
            //            return -1;
            //        }

            //        // success
            //        WSASetLastError(0);
            //        SetLastError(0);

            //        // clean memory
            //        foreach (var ptr in allocatedMemory)
            //            Marshal.FreeHGlobal(ptr);

            //        allocatedMemory.Clear();
            //        return 0;


            //    }
            //    else
            //    {

            //        return connect(s, sockAddr, addrsize);
            //    }

            //}
            var structure = Marshal.PtrToStructure<sockaddr_in>(sockAddr);
            var ipAddress = new IPAddress(structure.sin_addr.S_addr);
            var port = structure.sin_port;

            if (ipAddress.ToString() is string str_ip &&
                (str_ip == "127.0.0.1" || str_ip == "0.0.0.0" || str_ip == "")) // if local
            {
                return connect(s, sockAddr, addrsize);
            }

            //_interface.Message($"Connection attempt at {ipAddress}:{htons(port)}, redirecting to 127.0.0.1:{_redirectionPort}...");

            _interface.IpRedirected(new IPEndPoint(ipAddress, htons(port)), Process.GetCurrentProcess().Id, _redirectionPort);

            var strucPtr = Marshal.AllocHGlobal(addrsize);
            var struc = new sockaddr_in
            {
                sin_addr = { S_addr = inet_addr(_interface.GetRedirectedIp()) },
                sin_port = htons(_redirectionPort),
                sin_family = (short)AddressFamily.InterNetworkv4,
            };

            Marshal.StructureToPtr(struc, strucPtr, true);
            return connect(s, strucPtr, addrsize);
        }
        private String sockAddrInToString(sockaddr_in sin)
        {

            string family = sin.sin_family.ToString();
            string remoteIp = new System.Net.IPAddress(sin.sin_addr.S_addr).ToString();
            string remotePort = ntohs(sin.sin_port).ToString();
            string w1 = sin.sin_addr.S_un_w.s_w1.ToString();
            string w2 = sin.sin_addr.S_un_w.s_w2.ToString();
            string b1 = sin.sin_addr.S_un_b.s_b1.ToString();
            string b2 = sin.sin_addr.S_un_b.s_b2.ToString();
            string b3 = sin.sin_addr.S_un_b.s_b3.ToString();
            string b4 = sin.sin_addr.S_un_b.s_b4.ToString();
            return "Family: " + family + " Remote Ip: " + remoteIp + " Remote Port: " + remotePort +
                " w1: " + w1 + " w2: " + w2 + " b1: " + b1 + " b2: " + b2 + " b3: " + b3 + " b4: " + b4; // w,b == zero padding

        }


        private static IntPtr StructToPtr(object obj)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }

        private List<IntPtr> allocatedMemory = new List<IntPtr>();
        private IntPtr Recieve(IntPtr socket, int len)
        {
            var buffer = Marshal.AllocHGlobal(len);
            allocatedMemory.Add(buffer);

            var result = recv(socket, buffer, len, 0);
            if (result == -1)
            {
                var errorcode = WSAGetLastError();
                return IntPtr.Zero;
            }

            return buffer;
        }

        private IntPtr RecieveAuth(IntPtr socket, int len)
        {
            var buffer = Marshal.AllocHGlobal(len);
            allocatedMemory.Add(buffer);

            var result = recv(socket, buffer, len, 0);
            if (result == -1)
            {
                var errorcode = WSAGetLastError();
                return IntPtr.Zero; ;
            }

            if (result == 0)
                return buffer;

            if (result != 2)
            {
                HookManager.Log("Bad response from server!");
                return IntPtr.Zero;
            }
            return buffer;
        }

        private IntPtr RecieveBind(IntPtr socket, int len)
        {
            var buffer = Marshal.AllocHGlobal(len);
            allocatedMemory.Add(buffer);

            var result = recv(socket, buffer, len, 0);
            if (result == -1)
            {
                var errorcode = WSAGetLastError();
                return IntPtr.Zero; ;
            }

            if (result == 0)
                return buffer;

            if (result != 10)
            {
                HookManager.Log("Bad response from server!");
                return IntPtr.Zero;
            }
            return buffer;
        }

        private sockaddr_in SetAddr(IntPtr addr, string ip, string port)
        {
            sockaddr_in structure = (sockaddr_in)Marshal.PtrToStructure(addr, typeof(sockaddr_in));
            string originalip = new System.Net.IPAddress(structure.sin_addr.S_addr).ToString();
            ushort originalport = ntohs(structure.sin_port);

            structure.sin_addr.S_addr = inet_addr(ip);
            structure.sin_port = htons(Convert.ToUInt16(port));
            Marshal.StructureToPtr(structure, addr, true);
            structure = (sockaddr_in)Marshal.PtrToStructure(addr, typeof(sockaddr_in));
            return structure;
        }

        private IntPtr CreateAddr(string ip, string port)
        {
            var s = Marshal.AllocHGlobal(16);
            sockaddr_in sockAddr = new sockaddr_in();
            sockAddr.sin_addr.S_addr = inet_addr(ip);
            sockAddr.sin_port = htons(Convert.ToUInt16(port));
            sockAddr.sin_family = 2;
            Marshal.StructureToPtr(sockAddr, s, true);
            return s;
        }

        private IntPtr SetUpSocks5Request()
        {
            var initialRequest = Marshal.AllocHGlobal(4);

            Marshal.WriteByte(initialRequest, Convert.ToByte(5));
            Marshal.WriteByte(initialRequest + 1, Convert.ToByte(2));
            Marshal.WriteByte(initialRequest + 2, Convert.ToByte(0));
            Marshal.WriteByte(initialRequest + 3, Convert.ToByte(2));

            return initialRequest;
        }

        private IntPtr SetUpAuthenticateRequest(string username, string password, out int index)
        {
            index = 0;
            var size = 3 + Encoding.Default.GetBytes(username).Length + Encoding.Default.GetBytes(password).Length;
            var authenticateBuffer = Marshal.AllocHGlobal(size);

            Marshal.WriteByte(authenticateBuffer + index++, Convert.ToByte(1));
            Marshal.WriteByte(authenticateBuffer + index++, Convert.ToByte(username.Length));
            byte[] rawBytes;
            if (username.Length > 0)
            {
                rawBytes = Encoding.Default.GetBytes(username);
                for (int i = 0; i < rawBytes.Length; i++)
                {
                    Marshal.WriteByte(authenticateBuffer + index++, rawBytes[i]);
                }
            }
            Marshal.WriteByte(authenticateBuffer + index++, Convert.ToByte(password.Length));
            if (password.Length > 0)
            {
                rawBytes = Encoding.Default.GetBytes(password);
                for (int i = 0; i < rawBytes.Length; i++)
                {
                    Marshal.WriteByte(authenticateBuffer + index++, rawBytes[i]);
                }
            }

            return authenticateBuffer;
        }

        private IntPtr SetUpBindWithRemoteHost(string eveIP, ushort evePort)
        {
            var bindWithEveBuffer = Marshal.AllocHGlobal(10);
            var iplist = eveIP.Split('.').ToList();
            byte[] portbyte = BitConverter.GetBytes(evePort).Reverse().ToArray();
            byte[] newbyte = new byte[2];
            int indexy = 0;
            foreach (var byty in portbyte)
            {
                newbyte[indexy] = byty;
                indexy++;
            }

            // bind with remote server
            Marshal.WriteByte(bindWithEveBuffer, Convert.ToByte(5));
            Marshal.WriteByte(bindWithEveBuffer + 1, Convert.ToByte(1));
            Marshal.WriteByte(bindWithEveBuffer + 2, Convert.ToByte(0));
            Marshal.WriteByte(bindWithEveBuffer + 3, Convert.ToByte(1));
            Marshal.WriteByte(bindWithEveBuffer + 4, Convert.ToByte(iplist[0]));
            Marshal.WriteByte(bindWithEveBuffer + 5, Convert.ToByte(iplist[1]));
            Marshal.WriteByte(bindWithEveBuffer + 6, Convert.ToByte(iplist[2]));
            Marshal.WriteByte(bindWithEveBuffer + 7, Convert.ToByte(iplist[3]));
            Marshal.WriteByte(bindWithEveBuffer + 8, newbyte[0]);
            Marshal.WriteByte(bindWithEveBuffer + 9, newbyte[1]);

            return bindWithEveBuffer;
        }

        private bool VerifyBindResponse(IntPtr buffer)
        {
            var recvBytes = new byte[10] { Marshal.ReadByte(buffer), Marshal.ReadByte(buffer, 1), Marshal.ReadByte(buffer, 2), Marshal.ReadByte(buffer, 3), Marshal.ReadByte(buffer, 4), Marshal.ReadByte(buffer, 5), Marshal.ReadByte(buffer, 6), Marshal.ReadByte(buffer, 7), Marshal.ReadByte(buffer, 8), Marshal.ReadByte(buffer, 9) };

            if (recvBytes[1] != 0)
            {
                if (recvBytes[1] == 1)
                    HookManager.Log("General failure");
                if (recvBytes[1] == 2)
                    HookManager.Log("connection not allowed by ruleset");
                if (recvBytes[1] == 3)
                    HookManager.Log("network unreachable");
                if (recvBytes[1] == 4)
                    HookManager.Log("host unreachable");
                if (recvBytes[1] == 5)
                    HookManager.Log("connection refused by destination host");
                if (recvBytes[1] == 6)
                    HookManager.Log("TTL expired");
                if (recvBytes[1] == 7)
                    HookManager.Log("command not supported / protocol error");
                if (recvBytes[1] == 8)
                    HookManager.Log("address type not supported");

                HookManager.Log("Proxy: Connection error binding eve server");
                return false;
            }
            return true;
        }
    }
}