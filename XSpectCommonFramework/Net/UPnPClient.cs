// -*- mode: csharp; encoding: utf-8; -*-
/* XSpect Common Framework - Generic utility class library
 * Copyright © 2008-2009 Takeshi KIRIYA, XSpect Project <takeshik@users.sf.net>
 * All rights reserved.
 * 
 * This file is part of XSpect Common Framework.
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 * 
 * This library is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
 * or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public
 * License for more details. 
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>,
 * or write to the Free Software Foundation, Inc., 51 Franklin Street,
 * Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace XSpect.Net
{
    public class UPnPClient
        : Object,
          IDisposable
    {
        private readonly IPAddress _clientAddr;

        private readonly IPAddress _igdAddr;

        private readonly List<Int32> _openedPorts;

        public UPnPClient()
            : this(NetworkInterface.GetAllNetworkInterfaces()
                .Where(nif => nif.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Single()
            )
        {
        }

        public UPnPClient(NetworkInterface networkInterface)
            : this(networkInterface, networkInterface.GetIPProperties().UnicastAddresses.Single().Address)
        {
        }

        public UPnPClient(NetworkInterface networkInterface, IPAddress clientAddr)
            : this(networkInterface, clientAddr, networkInterface.GetIPProperties().GatewayAddresses.Single().Address)
        {
        }

        public UPnPClient(NetworkInterface networkInterface, IPAddress clientAddr, IPAddress igdAddr)
        {
            this._openedPorts = new List<Int32>();

            if (networkInterface.GetIPProperties().UnicastAddresses
                .Select(addrInfo => addrInfo.Address)
                .Contains(clientAddr)
            )
            {
                this._clientAddr = clientAddr;
            }
            else
            {
                throw new ArgumentException("Invalid IP address", "clientAddr");
            }

            if (networkInterface.GetIPProperties().GatewayAddresses
                .Select(addrInfo => addrInfo.Address)
                .Contains(igdAddr)
            )
            {
                this._igdAddr = igdAddr;
            }
            else
            {
                throw new ArgumentException("Invalid IGD address", "igdAddr");
            }
        }

        public virtual void Open(Int32 port)
        {
            Int32 remotePort = -1;
            XDocument services = this.Discover(out remotePort);
            if (remotePort < 0)
            {
                throw new InvalidOperationException();
            }
            try
            {
                this.AddPortMapping(services, "urn:schemas-upnp-org:service:WANIPConnection:1", ProtocolType.Tcp, port);
            }
            catch (InvalidOperationException)
            {
                this.AddPortMapping(services, "urn:schemas-upnp-org:service:WANPPPConnection:1", ProtocolType.Tcp, port);
            }
        }

        public virtual void Close(Int32 port)
        {
            Int32 remotePort = -1;
            XDocument services = this.Discover(out remotePort);
            if (remotePort < 0)
            {
                throw new InvalidOperationException();
            }
            try
            {
                this.DeletePortMapping(services, "urn:schemas-upnp-org:service:WANIPConnection:1", ProtocolType.Tcp, port);
            }
            catch (InvalidOperationException)
            {
                this.DeletePortMapping(services, "urn:schemas-upnp-org:service:WANPPPConnection:1", ProtocolType.Tcp, port);
            }
        }

        public virtual IPAddress GetIPAddress()
        {
            Int32 remotePort = -1;
            XDocument services = this.Discover(out remotePort);
            if (remotePort < 0)
            {
                throw new InvalidOperationException();
            }
            try
            {
                return this.GetExternalIPAddress(services, "urn:schemas-upnp-org:service:WANIPConnection:1");
            }
            catch (InvalidOperationException)
            {
                return this.GetExternalIPAddress(services, "urn:schemas-upnp-org:service:WANPPPConnection:1");
            }
        }

        protected virtual XDocument Discover(out Int32 responsePort)
        {
            Socket socketClient = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );
            socketClient.SetSocketOption(
                SocketOptionLevel.Socket,
                SocketOptionName.ReceiveTimeout,
                5000
            );
            Byte[] query = Encoding.ASCII.GetBytes(
                "M-SEARCH * HTTP/1.1\r\n" +
                "HOST: " + this._igdAddr.ToString() + ":1900\r\n" +
                "ST: upnp:rootdevice\r\n" +
                "MAN: \"ssdp:discover\"\r\n" +
                "MX:3\r\n" +
                "\r\n" +
                "\r\n"
            );
            socketClient.SendTo(
                query,
                query.Length,
                SocketFlags.None,
                new IPEndPoint(this._igdAddr, 1900)
            );
            Byte[] data = new Byte[1024];
            EndPoint endPoint = (EndPoint) new IPEndPoint(IPAddress.Any, 0);
            socketClient.ReceiveFrom(data, ref endPoint);
            socketClient.Close();

            String locationLine = Encoding.ASCII.GetString(data)
                .Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Single(l => l.ToUpper().StartsWith("LOCATION"));

            HttpClient httpClient = new HttpClient(null);
            Uri uri = new Uri(locationLine.Substring(locationLine.IndexOf(':') + 1));
            responsePort = uri.Port;
            return httpClient.Get(uri, response =>
            {
                return XDocument.Load(XmlReader.Create(response));
            });
        }

        protected virtual void AddPortMapping(XDocument services, String serviceType, ProtocolType protocol, Int32 port)
        {
            Byte[] body = UTF8Encoding.ASCII.GetBytes(String.Format(
                #region SOAP body
@"<?xml version='1.0'?>
<s:Envelope
    xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'
    s:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'
>
    <s:Body>
        <m:AddPortMapping
            xmlns:m='{0}'
        >
            <NewRemoteHost></NewRemoteHost>
            <NewExternalPort>{1}</NewExternalPort>
            <NewProtocol>{2}</NewProtocol>
            <NewInternalPort>{1}</NewInternalPort>
            <NewInternalClient>{3}</NewInternalClient>
            <NewEnabled>1</NewEnabled>
            <NewPortMappingDescription>Set by {4}</NewPortMappingDescription>
            <NewLeaseDuration>0</NewLeaseDuration>
        </m:AddPortMapping>
    </s:Body>
</s:Envelope>"
                #endregion
                , serviceType, port, protocol, this._clientAddr, "XSpect.Net.UPnPClient"
            ));
            HttpClient client = new HttpClient("UPnPClient");
            client.AdditionalHeaders.Add("SOAPAction", String.Format("\"{0}#AddPortMapping\"", serviceType));
            client.Post(new Uri(
                services
                    .Element("{urn:schemas-upnp-org:device-1-0}root")
                    .Element("{urn:schemas-upnp-org:device-1-0}URLBase")
                    .Value
                + services.Descendants()
                    .Where(e => e.Name == "{urn:schemas-upnp-org:device-1-0}serviceType")
                    .Where(e => e.Value == serviceType)
                    .Single()
                    .Parent
                    .Element("{urn:schemas-upnp-org:device-1-0}controlURL")
                    .Value
            ), body);
            // webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            this._openedPorts.Remove(port);
        }

        protected virtual void DeletePortMapping(XDocument services, String serviceType, ProtocolType protocol, Int32 port)
        {
            Byte[] body = UTF8Encoding.ASCII.GetBytes(String.Format(
                #region SOAP body
@"<?xml version='1.0'?>
<s:Envelope
    xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'
    s:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'
>
    <s:Body>
        <m:DeletePortMapping
            xmlns:m='{0}'
        >
            <NewRemoteHost></NewRemoteHost>
            <NewExternalPort>{1}</NewExternalPort>
            <NewProtocol>{2}</NewProtocol>
            <NewInternalPort>{1}</NewInternalPort>
            <NewInternalClient>{3}</NewInternalClient>
        </m:AddPortMapping>
    </s:Body>
</s:Envelope>"
                #endregion
                , serviceType, port, protocol, this._clientAddr, "XSpect.Net.UPnPClient"
            ));
            HttpClient client = new HttpClient("UPnPClient");
            client.AdditionalHeaders.Add("SOAPAction", String.Format("\"{0}#DeletePortMapping\"", serviceType));
            client.Post(new Uri(
                services
                    .Element("{urn:schemas-upnp-org:device-1-0}root")
                    .Element("{urn:schemas-upnp-org:device-1-0}URLBase")
                    .Value
                + services.Descendants()
                    .Where(e => e.Name == "{urn:schemas-upnp-org:device-1-0}serviceType")
                    .Where(e => e.Value == serviceType)
                    .Single()
                    .Parent
                    .Element("{urn:schemas-upnp-org:device-1-0}controlURL")
                    .Value
            ), body);
            this._openedPorts.Add(port);
        }

        protected virtual IPAddress GetExternalIPAddress(XDocument services, String serviceType)
        {
            Byte[] body = UTF8Encoding.ASCII.GetBytes(String.Format(
                #region SOAP body
@"<?xml version='1.0'?>
<s:Envelope
    xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'
    s:encodingStyle='http://schemas.xmlsoap.org/soap/encoding/'>
    <s:Body>
    <m:GetExternalIPAddress
        xmlns:m='{0}'>
    </m:GetExternalIPAddress>
    </s:Body>
</s:Envelope>"
                #endregion
                , serviceType
            ));
            HttpClient client = new HttpClient("UPnPClient");
            client.AdditionalHeaders.Add("SOAPAction", String.Format("\"{0}#GetExternalIPAddress\"", serviceType));
            return client.Post(new Uri(
                services
                    .Element("{urn:schemas-upnp-org:device-1-0}root")
                    .Element("{urn:schemas-upnp-org:device-1-0}URLBase")
                    .Value
                + services.Descendants()
                    .Where(e => e.Name == "{urn:schemas-upnp-org:device-1-0}serviceType")
                    .Where(e => e.Value == serviceType)
                    .Single()
                    .Parent
                    .Element("{urn:schemas-upnp-org:device-1-0}controlURL")
                    .Value
            ), body, response => IPAddress.Parse(XDocument.Load(XmlReader.Create(response)).Elements().Single().Value));
        }

        public virtual void Dispose()
        {
            foreach (Int32 port in this._openedPorts)
            {
                this.Close(port);
            }
        }
    }
}
