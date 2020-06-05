﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace Suprema
{
    using System.Net;
    using BS2_CONFIG_MASK = UInt32;

    public class ConfigControl : FunctionModule
    {
        protected override List<KeyValuePair<string, Action<IntPtr, UInt32, bool>>> getFunctionList(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            List<KeyValuePair<string, Action<IntPtr, UInt32, bool>>> functionList = new List<KeyValuePair<string, Action<IntPtr, uint, bool>>>();

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get AllConfig", getAllConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get Card1xConfig", getCard1xConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set Card1xConfig", setCard1xConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get SystemExtConfig", getSystemExtConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set SystemExtConfig", setSystemExtConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get VoipConfig", getVoipConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set VoipConfig", setVoipConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get FaceConfig", getFaceConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set FaceConfig", setFaceConfig));            
            
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get auth group", getAuthGroup));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Remove auth group", removeAuthGroup));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set auth group", setAuthGroup));            

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("disable ssl", disbleSSL));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get RS485ConfigEx", getRS485ConfigEx));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set RS485ConfigEx", setRS485ConfigEx));      

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get CardConfigEx", getCardConfigEx));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set CardConfigEx", setCardConfigEx));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get supported Config Mask", getConfigMask));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get DstConfig", getDstConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set DstConfig", setDstConfig));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get DesFireCardConfigEx", getDesFireCardConfigEx));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set DesFireCardConfigEx", setDesFireCardConfigEx));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get SystemConfig", getSystemConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set SystemConfig", setSystemConfig));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get InputConfig", getInputConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set InputConfig", setInputConfig));

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get DataEncryptKey", getDataEncryptKey));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set DataEncryptKey", setDataEncryptKey));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Remove DataEncryptKey", removeDataEncryptKey));

            //[IPv6] 
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get IPConfig", getIPConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get IPV6Config", getIPV6Config));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set IPV6Config", setIPV6Config));
            //<=

            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Get AuthConfig", getAuthConfig));
            functionList.Add(new KeyValuePair<string, Action<IntPtr, uint, bool>>("Set AuthConfig", setAuthConfig));

            return functionList;
        }

        //[IPv6]
        void print(IntPtr sdkContext, BS2IpConfig config)
        {
            Console.WriteLine(">>>> IP configuration ");
            Console.WriteLine("     |--connectionMode : {0}", config.connectionMode);
            Console.WriteLine("     |--useDHCP : {0}", config.useDHCP);
            Console.WriteLine("     |--useDNS : {0}", config.useDNS);
            Console.WriteLine("     |--ipAddress : {0}", Encoding.UTF8.GetString(config.ipAddress), BitConverter.ToString(config.ipAddress));
            Console.WriteLine("     |--gateway : {0}", Encoding.UTF8.GetString(config.gateway), BitConverter.ToString(config.gateway));
            Console.WriteLine("     |--subnetMask : {0}", Encoding.UTF8.GetString(config.subnetMask), BitConverter.ToString(config.subnetMask));
            Console.WriteLine("     |--serverAddr : {0}", Encoding.UTF8.GetString(config.serverAddr), BitConverter.ToString(config.serverAddr));
            Console.WriteLine("     |--port : {0}", config.port);
            Console.WriteLine("     |--serverPort : {0}", config.serverPort);
            Console.WriteLine("     |--mtuSize : {0}", config.mtuSize);
            Console.WriteLine("     |--baseband : {0}", config.baseband);
            Console.WriteLine("     |--sslServerPort : {0}", config.sslServerPort);
            Console.WriteLine("<<<< ");
        }

        void getIPConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2IpConfig config;
            Console.WriteLine("Trying to get IPConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetIPConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        void print(IntPtr sdkContext, BS2IPV6Config config)
        {
            Console.WriteLine(">>>> IPV6 configuration ");
            Console.WriteLine("     |--useIPV6 : {0}", config.useIPV6);
            Console.WriteLine("     |--reserved1 : {0}", config.reserved1);// useIPV4);
            Console.WriteLine("     |--useDhcpV6 : {0}", config.useDhcpV6);
            Console.WriteLine("     |--useDnsV6 : {0}", config.useDnsV6);
            Console.WriteLine("     |--staticIpAddressV6 : {0}", Encoding.UTF8.GetString(config.staticIpAddressV6), BitConverter.ToString(config.staticIpAddressV6));
            Console.WriteLine("     |--staticGatewayV6 : {0}", Encoding.UTF8.GetString(config.staticGatewayV6), BitConverter.ToString(config.staticGatewayV6));
            Console.WriteLine("     |--dnsAddrV6 : {0}", Encoding.UTF8.GetString(config.dnsAddrV6), BitConverter.ToString(config.dnsAddrV6));
            Console.WriteLine("     |--serverIpAddressV6 : {0}", Encoding.UTF8.GetString(config.serverIpAddressV6), BitConverter.ToString(config.serverIpAddressV6));
            Console.WriteLine("     |--serverPortV6 : {0}", config.serverPortV6);
            Console.WriteLine("     |--sslServerPortV6 : {0}", config.sslServerPortV6 );			
            Console.WriteLine("     |--portV6 : {0}", config.portV6);
            Console.WriteLine("     |--numOfAllocatedAddressV6 : {0}", config.numOfAllocatedAddressV6);
            Console.WriteLine("     |--numOfAllocatedGatewayV6 : {0}", config.numOfAllocatedGatewayV6);
            byte[] tempIPV6 = new byte[BS2Environment.BS2_IPV6_ADDR_SIZE];
            for (int idx = 0; idx < config.numOfAllocatedAddressV6; ++idx)
            {
                Array.Copy(config.allocatedIpAddressV6, idx * BS2Environment.BS2_IPV6_ADDR_SIZE, tempIPV6, 0, BS2Environment.BS2_IPV6_ADDR_SIZE);
                Console.WriteLine("     |--allocatedIpAddressV6[{0}] : {1}", idx, Encoding.UTF8.GetString(tempIPV6), BitConverter.ToString(tempIPV6));
            }
            for (int idx = 0; idx < config.numOfAllocatedGatewayV6; ++idx)
            {
                Array.Copy(config.allocatedGatewayV6, idx * BS2Environment.BS2_IPV6_ADDR_SIZE, tempIPV6, 0, BS2Environment.BS2_IPV6_ADDR_SIZE);
                Console.WriteLine("     |--allocatedGatewayV6[{0}] : {1}]", idx, Encoding.UTF8.GetString(tempIPV6), BitConverter.ToString(tempIPV6));
            }
            Console.WriteLine("<<<< ");
        }

        void getIPV6Config(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2IPV6Config config;
            Console.WriteLine("Trying to get IPV6Config");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetIPV6Config(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        public void setIPV6Config(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2IPV6Config config = Util.AllocateStructure<BS2IPV6Config>();
            Console.WriteLine("Trying to get Current IPV6Config");
            BS2ErrorCode result = BS2ErrorCode.BS_SDK_SUCCESS;
            result = (BS2ErrorCode)API.BS2_GetIPV6Config(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }

            do
            {
                Console.WriteLine("useDhcpV6 ? [Y/N, (Blank:{0})]", config.useDhcpV6);
                Console.Write(">>>> ");
                bool bInput = config.useDhcpV6 != 0;
                if (bInput)
                    bInput = Util.IsYes();
                else
                    bInput = !Util.IsNo();
                config.useDhcpV6 = (byte)(bInput ? 1 : 0);

                Console.WriteLine("useDnsV6 ? [Y/N, (Blank:{0})]", config.useDnsV6);
                Console.Write(">>>> ");
                bInput = config.useDnsV6 != 0;
                if (bInput)
                    bInput = Util.IsYes();
                else
                    bInput = !Util.IsNo();
                config.useDnsV6 = (byte)(bInput ? 1 : 0);

                string strInput;
                byte[] bytesInput = null;
                if (config.useDhcpV6 == 0)
                { 
                    Console.WriteLine("staticIpAddressV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.staticIpAddressV6));
                    Console.Write(">>>> ");
                    strInput = Console.ReadLine();                    
                    if (strInput.Length == 0)
                    {
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / N(clear), (Blank:Y)]");
                        Console.Write("   >>>> ");
                        if (!Util.IsYes())
                        {
                            Array.Clear(config.staticIpAddressV6, 0, config.staticIpAddressV6.Length);
                        }
                    }
                    else
                    {
                        Array.Clear(config.staticIpAddressV6, 0, config.staticIpAddressV6.Length);
                        bytesInput = Encoding.UTF8.GetBytes(strInput);
                        Array.Copy(bytesInput, 0, config.staticIpAddressV6, 0, Math.Min(bytesInput.Length, config.staticIpAddressV6.Length));                    
                    }
                    if (Encoding.UTF8.GetString(config.staticIpAddressV6).Length > 0)
                    {
                        IPAddress dummy;
                        if (IPAddress.TryParse(Encoding.UTF8.GetString(config.staticIpAddressV6).TrimEnd('\0'), out dummy) == false)
                        {
                            Console.WriteLine("Wrong staticIpAddressV6: {0})", Encoding.UTF8.GetString(config.staticIpAddressV6));
                            return;
                        }
                    }


                    Console.WriteLine("staticGatewayV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.staticGatewayV6));
                    Console.Write(">>>> ");
                    strInput = Console.ReadLine();
                    bytesInput = null;
                    if (strInput.Length == 0)
                    {
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / N(clear), (Blank:Y)]");
                        Console.Write("   >>>> ");
                        if (!Util.IsYes())
                        {
                            Array.Clear(config.staticGatewayV6, 0, config.staticGatewayV6.Length);
                        }
                    }
                    else
                    {
                        Array.Clear(config.staticGatewayV6, 0, config.staticGatewayV6.Length);
                        bytesInput = Encoding.UTF8.GetBytes(strInput);
                        Array.Copy(bytesInput, 0, config.staticGatewayV6, 0, Math.Min(bytesInput.Length, config.staticGatewayV6.Length));
                    }
                    if (Encoding.UTF8.GetString(config.staticGatewayV6).Length > 0)
                    {
                        IPAddress dummy;
                        if (IPAddress.TryParse(Encoding.UTF8.GetString(config.staticGatewayV6).TrimEnd('\0'), out dummy) == false)
                        {
                            Console.WriteLine("Wrong staticGatewayV6: {0})", Encoding.UTF8.GetString(config.staticGatewayV6));
                            return;
                        }
                    }
                }

                if (config.useDnsV6 == 1)
                {
                    Console.WriteLine("dnsAddrV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.dnsAddrV6));
                    Console.Write(">>>> ");
                    strInput = Console.ReadLine();
                    bytesInput = null;
                    if (strInput.Length == 0)
                    {
                        Console.WriteLine("   Do you want to keep the value? [Y(keep) / N(clear), (Blank:Y)]");
                        Console.Write("   >>>> ");
                        if (!Util.IsYes())
                        {
                            Array.Clear(config.dnsAddrV6, 0, config.dnsAddrV6.Length);
                        }
                    }
                    else
                    {
                        Array.Clear(config.dnsAddrV6, 0, config.dnsAddrV6.Length);
                        bytesInput = Encoding.UTF8.GetBytes(strInput);
                        Array.Copy(bytesInput, 0, config.dnsAddrV6, 0, Math.Min(bytesInput.Length, config.dnsAddrV6.Length));
                    }
                    if (Encoding.UTF8.GetString(config.dnsAddrV6).Length > 0)
                    {
                        IPAddress dummy;
                        if (IPAddress.TryParse(Encoding.UTF8.GetString(config.dnsAddrV6).TrimEnd('\0'), out dummy) == false)
                        {
                            Console.WriteLine("Wrong dnsAddrV6: {0})", Encoding.UTF8.GetString(config.dnsAddrV6));
                            return;
                        }
                    }
                }

                Console.WriteLine("serverIpAddressV6 ? [(Blank:{0})]", Encoding.UTF8.GetString(config.serverIpAddressV6));
                Console.Write(">>>> ");
                strInput = Console.ReadLine();
                bytesInput = null;
                if (strInput.Length == 0)
                {
                    Console.WriteLine("   Do you want to keep the value? [Y(keep) / N(clear), (Blank:Y)]");
                    Console.Write("   >>>> ");
                    if (!Util.IsYes())
                    {
                        Array.Clear(config.serverIpAddressV6, 0, config.serverIpAddressV6.Length);
                    }
                }
                else
                {
                    Array.Clear(config.serverIpAddressV6, 0, config.serverIpAddressV6.Length);
                    bytesInput = Encoding.UTF8.GetBytes(strInput);
                    Array.Copy(bytesInput, 0, config.serverIpAddressV6, 0, Math.Min(bytesInput.Length, config.serverIpAddressV6.Length));
                }
                if (Encoding.UTF8.GetString(config.serverIpAddressV6).TrimEnd('\0').Length > 0)
                {
                    IPAddress dummy;
                    if (IPAddress.TryParse(Encoding.UTF8.GetString(config.serverIpAddressV6), out dummy) == false)
                    {
                        Console.WriteLine("Wrong serverIpAddressV6: {0})", Encoding.UTF8.GetString(config.serverIpAddressV6));
                        return;
                    }
                }

                Console.WriteLine("serverPortV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT_V6);
                Console.Write(">>>> ");
                int nInput = Util.GetInput(BS2Environment.BS2_TCP_SERVER_PORT_DEFAULT_V6);
                config.serverPortV6 = (UInt16)nInput;

                Console.WriteLine("sslServerPortV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6);
                Console.Write(">>>> ");
                nInput = Util.GetInput(BS2Environment.BS2_TCP_SSL_SERVER_PORT_DEFAULT_V6);
                config.sslServerPortV6 = (UInt16)nInput;

                Console.WriteLine("portV6 ? [1~65535 (Blank:{0})]", BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6);
                Console.Write(">>>> ");
                nInput = Util.GetInput(BS2Environment.BS2_TCP_DEVICE_PORT_DEFAULT_V6);
                config.portV6 = (UInt16)nInput;

                config.numOfAllocatedAddressV6 = 0;
                config.numOfAllocatedGatewayV6 = 0;

            } while (false);

            Console.WriteLine("Trying to set IPV6Config");
            result = (BS2ErrorCode)API.BS2_SetIPV6Config(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }

            Console.WriteLine("Trying to get Changed IPV6Config");
            result = (BS2ErrorCode)API.BS2_GetIPV6Config(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }
        //<=

        public void getConfigMask(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2_CONFIG_MASK configMask = 0;

            Console.WriteLine("Trying to get supported config mask");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetSupportedConfigMask(sdkContext, deviceID, out configMask);
            if (result == BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Supported config Mask: 0x{0:X}", configMask);
            }
            else
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }
        }

        void getAllConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2Configs configs = Util.AllocateStructure<BS2Configs>();
            configs.configMask = (uint)BS2ConfigMaskEnum.ALL;
            Console.WriteLine("Trying to get AllConfig");

            Type structureType = typeof(BS2Configs);
            int structSize = Marshal.SizeOf(structureType);

            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetConfig(sdkContext, deviceID, ref configs);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("BS2_GetConfig failed. Error : {0}", result);
            }
            else
            {
                Console.WriteLine("BS2_GetConfig Success  : {0}", configs.factoryConfig.deviceID);
            }
        }

        void getCard1xConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS1CardConfig config;
            Console.WriteLine("Trying to get Card1xConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetCard1xConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        #region Card1x Utils
        void entercard1xKey(byte[] dst)
        {
            int index = 0;
            string[] keys = Console.ReadLine().Split('-');
            foreach (string key in keys)
            {
                dst[index++] = Convert.ToByte(key, 16);
                if (index > dst.Length)
                {
                    return;
                }
            }

            for (; index < dst.Length; ++index)
            {
                dst[index] = 0xFF;
            }
        }
        #endregion

        public void setCard1xConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS1CardConfig card1xConfig = Util.AllocateStructure<BS1CardConfig>();
            
            card1xConfig.magicNo = 0;
            card1xConfig.disabled = 0;
            card1xConfig.useCSNOnly = 0;
            card1xConfig.bioentryCompatible = 0;
            card1xConfig.useSecondaryKey = 1;

            card1xConfig.cisIndex = 0;
            card1xConfig.numOfTemplate = 1;
            card1xConfig.templateSize = 32;
            card1xConfig.templateStartBlock[0] = 4;
            card1xConfig.templateStartBlock[1] = 8;
            card1xConfig.templateStartBlock[2] = 12;
            card1xConfig.templateStartBlock[3] = 0;

            Console.WriteLine("Trying to set card1x configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetCard1xConfig(sdkContext, deviceID, ref card1xConfig);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }          
        
        void getSystemExtConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2SystemConfigExt config;
            Console.WriteLine("Trying to get SystemExtConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetSystemExtConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }


        public void setSystemExtConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2SystemConfigExt config = Util.AllocateStructure<BS2SystemConfigExt>();

            config.primarySecureKey[0] = 0x01;
            config.primarySecureKey[1] = 0xFE;

            config.secondarySecureKey[0] = 0x01;
            config.secondarySecureKey[1] = 0x1E;

            Console.WriteLine("Trying to set SystemExt configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetSystemExtConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

        void getVoipConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2VoipConfig config;
            Console.WriteLine("Trying to get VoipConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetVoipConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        public void setVoipConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2VoipConfig config = Util.AllocateStructure<BS2VoipConfig>();            

            string url = "192.168.0.1";
            byte[] str = Encoding.UTF8.GetBytes(url);
            Array.Clear(config.serverUrl, 0, BS2Environment.BS2_URL_SIZE);
            Array.Copy(str, 0, config.serverUrl, 0, str.Length);

            config.serverPort = 5061; 

            string userId = "홍길동";
            byte[] str2 = Encoding.UTF8.GetBytes(userId);
            Array.Clear(config.userID, 0, BS2Environment.BS2_USER_ID_SIZE);
            Array.Copy(str2, 0, config.userID, 0, str2.Length);

            string pwd = "123456";
            byte[] str3 = Encoding.UTF8.GetBytes(pwd);
            Array.Clear(config.userPW, 0, BS2Environment.BS2_USER_ID_SIZE);
            Array.Copy(str3, 0, config.userPW, 0, str3.Length);

            config.bUse = 1;
            config.dtmfMode = 0;
            config.exitButton = 1;

            config.numPhonBook = 2;
            string phoneNumber = "010-1234-5678";
            byte[] str4 = Encoding.UTF8.GetBytes(phoneNumber);
            Array.Clear(config.phonebook[0].phoneNumber, 0, BS2Environment.BS2_USER_ID_SIZE);
            Array.Copy(str4, 0, config.phonebook[0].phoneNumber, 0, str4.Length);

            string descript = "홍길동 나아가신다.";
            byte[] str5 = Encoding.UTF8.GetBytes(descript);
            Array.Clear(config.phonebook[0].descript, 0, BS2Environment.BS2_MAX_DESCRIPTION_NAME_LEN);
            Array.Copy(str5, 0, config.phonebook[0].descript, 0, str5.Length);

            string phoneNumber2 = "010-9874-1234";
            byte[] str6 = Encoding.UTF8.GetBytes(phoneNumber2);
            Array.Clear(config.phonebook[1].phoneNumber, 0, BS2Environment.BS2_USER_ID_SIZE);
            Array.Copy(str6, 0, config.phonebook[1].phoneNumber, 0, str6.Length);

            string descript2 = "사임당 나아가신다.";
            byte[] str7 = Encoding.UTF8.GetBytes(descript2);
            Array.Clear(config.phonebook[1].descript, 0, BS2Environment.BS2_MAX_DESCRIPTION_NAME_LEN);
            Array.Copy(str7, 0, config.phonebook[1].descript, 0, str7.Length);  
         

            Console.WriteLine("Trying to set Voip configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetVoipConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

        void getFaceConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2FaceConfig config;
            Console.WriteLine("Trying to get SystemExtConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetFaceConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        public void setFaceConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2FaceConfig config = Util.AllocateStructure<BS2FaceConfig>();

            Console.WriteLine("Trying to set FaceConfig configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetFaceConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }        

        #region Auth Group
        public void getAuthGroup(IntPtr sdkContext, uint deviceID, bool isMasterDevice)
        {
            IntPtr authGroupObj = IntPtr.Zero;
            UInt32 numAuthGroup = 0;
            BS2ErrorCode result = BS2ErrorCode.BS_SDK_SUCCESS;

            Console.WriteLine("Do you want to get all auth groups? [Y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                Console.WriteLine("Trying to get all auth gruops from device.");
                result = (BS2ErrorCode)API.BS2_GetAllAuthGroup(sdkContext, deviceID, out authGroupObj, out numAuthGroup);
            }
            else
            {
                Console.WriteLine("Enter the ID of the access group which you want to get: [ID_1,ID_2 ...]");
                Console.Write(">>>> ");
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                string[] authGroupIDs = Console.ReadLine().Split(delimiterChars);
                List<UInt32> authGroupIDList = new List<UInt32>();

                foreach (string authGroupID in authGroupIDs)
                {
                    if (authGroupID.Length > 0)
                    {
                        UInt32 item;
                        if (UInt32.TryParse(authGroupID, out item))
                        {
                            authGroupIDList.Add(item);
                        }
                    }
                }

                if (authGroupIDList.Count > 0)
                {
                    IntPtr authGroupIDObj = Marshal.AllocHGlobal(4 * authGroupIDList.Count);
                    IntPtr curAuthGroupIDObj = authGroupIDObj;
                    foreach (UInt32 item in authGroupIDList)
                    {
                        Marshal.WriteInt32(curAuthGroupIDObj, (Int32)item);
                        curAuthGroupIDObj = (IntPtr)((long)curAuthGroupIDObj + 4);
                    }

                    Console.WriteLine("Trying to get auth gruops from device.");
                    result = (BS2ErrorCode)API.BS2_GetAuthGroup(sdkContext, deviceID, authGroupIDObj, (UInt32)authGroupIDList.Count, out authGroupObj, out numAuthGroup);

                    Marshal.FreeHGlobal(authGroupIDObj);
                }
                else
                {
                    Console.WriteLine("Invalid parameter");
                }
            }

            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else if (numAuthGroup > 0)
            {
                IntPtr curAuthGroupObj = authGroupObj;
                int structSize = Marshal.SizeOf(typeof(BS2AuthGroup));

                for (int idx = 0; idx < numAuthGroup; ++idx)
                {
                    BS2AuthGroup item = (BS2AuthGroup)Marshal.PtrToStructure(curAuthGroupObj, typeof(BS2AuthGroup));
                    print(sdkContext, item);
                    curAuthGroupObj = (IntPtr)((long)curAuthGroupObj + structSize);
                }

                API.BS2_ReleaseObject(authGroupObj);
            }
            else
            {
                Console.WriteLine(">>> There is no auth group in the device.");
            }
        }

        public void removeAuthGroup(IntPtr sdkContext, uint deviceID, bool isMasterDevice)
        {
            BS2ErrorCode result = BS2ErrorCode.BS_SDK_SUCCESS;

            Console.WriteLine("Do you want to remove all auth groups? [Y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                Console.WriteLine("Trying to remove all auth gruops from device.");
                result = (BS2ErrorCode)API.BS2_RemoveAllAuthGroup(sdkContext, deviceID);
            }
            else
            {
                Console.WriteLine("Enter the ID of the auth group which you want to remove: [ID_1,ID_2 ...]");
                Console.Write(">>>> ");
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                string[] authGroupIDs = Console.ReadLine().Split(delimiterChars);
                List<UInt32> authGroupIDList = new List<UInt32>();

                foreach (string authGroupID in authGroupIDs)
                {
                    if (authGroupID.Length > 0)
                    {
                        UInt32 item;
                        if (UInt32.TryParse(authGroupID, out item))
                        {
                            authGroupIDList.Add(item);
                        }
                    }
                }

                if (authGroupIDList.Count > 0)
                {
                    IntPtr authGroupIDObj = Marshal.AllocHGlobal(4 * authGroupIDList.Count);
                    IntPtr curAuthGroupIDObj = authGroupIDObj;
                    foreach (UInt32 item in authGroupIDList)
                    {
                        Marshal.WriteInt32(curAuthGroupIDObj, (Int32)item);
                        curAuthGroupIDObj = (IntPtr)((long)curAuthGroupIDObj + 4);
                    }

                    Console.WriteLine("Trying to remove auth gruops from device.");
                    result = (BS2ErrorCode)API.BS2_RemoveAuthGroup(sdkContext, deviceID, authGroupIDObj, (UInt32)authGroupIDList.Count);

                    Marshal.FreeHGlobal(authGroupIDObj);
                }
                else
                {
                    Console.WriteLine("Invalid parameter");
                }
            }

            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

        public void setAuthGroup(IntPtr sdkContext, uint deviceID, bool isMasterDevice)
        {
            Console.WriteLine("How many auth groups do you want to set? [1(default)-128]");
            Console.Write(">>>> ");
            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            int amount = Util.GetInput(1);
            List<BS2AuthGroup> authGroupList = new List<BS2AuthGroup>();

            for (int idx = 0; idx < amount; ++idx)
            {
                BS2AuthGroup authGroup = Util.AllocateStructure<BS2AuthGroup>();

                Console.WriteLine("Enter a value for auth group[{0}]", idx);
                Console.WriteLine("  Enter the ID for the auth group which you want to set");
                Console.Write("  >>>> ");
                authGroup.id = (UInt32)Util.GetInput();
                Console.WriteLine("  Enter the name for the auth group which you want to set");
                Console.Write("  >>>> ");
                string authGroupName = Console.ReadLine();
                if (authGroupName.Length == 0)
                {
                    Console.WriteLine("  [Warning] auth group name will be displayed as empty.");
                }
                else if (authGroupName.Length > BS2Environment.BS2_MAX_AUTH_GROUP_NAME_LEN)
                {
                    Console.WriteLine("  Name of auth group should less than {0} words.", BS2Environment.BS2_MAX_AUTH_GROUP_NAME_LEN);
                    return;
                }
                else
                {
                    byte[] authGroupArray = Encoding.UTF8.GetBytes(authGroupName);
                    Array.Clear(authGroup.name, 0, BS2Environment.BS2_MAX_AUTH_GROUP_NAME_LEN);
                    Array.Copy(authGroupArray, authGroup.name, authGroupArray.Length);
                }

                authGroupList.Add(authGroup);
            }

            int structSize = Marshal.SizeOf(typeof(BS2AuthGroup));
            IntPtr authGroupListObj = Marshal.AllocHGlobal(structSize * authGroupList.Count);
            IntPtr curAuthGroupListObj = authGroupListObj;
            foreach (BS2AuthGroup item in authGroupList)
            {
                Marshal.StructureToPtr(item, curAuthGroupListObj, false);
                curAuthGroupListObj = (IntPtr)((long)curAuthGroupListObj + structSize);
            }

            Console.WriteLine("Trying to set auth groups to device.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetAuthGroup(sdkContext, deviceID, authGroupListObj, (UInt32)authGroupList.Count);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }

            Marshal.FreeHGlobal(authGroupListObj);
        }        
        #endregion

        public void disbleSSL(IntPtr sdkContext, uint deviceID, bool isMasterDevice)
        {
            Console.WriteLine("Trying to disable ssl");

            BS2ErrorCode result = (BS2ErrorCode)API.BS2_DisableSSL(sdkContext, deviceID);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

       
        void getRS485ConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2Rs485ConfigEX config;
            Console.WriteLine("Trying to get RS485ConfigEx");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetRS485ConfigEx(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }

        }

        public void setRS485ConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {

            BS2Rs485ConfigEX config = Util.AllocateStructure<BS2Rs485ConfigEX>();
            Console.WriteLine("Trying to get RS485ConfigEx");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetRS485ConfigEx(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }

            Console.WriteLine("Do you want to change settings of device? [Y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                for (int i = 0; i < 5; i++)
                {
                    config.mode[i] = 1;
                }

                config.numOfChannels = 5;
                config.channels[0].baudRate = 115200;
                config.channels[0].channelIndex = 0;
                config.channels[0].useRegistance = 0;
                config.channels[0].numOfDevices = 1;
                config.channels[0].slaveDevices[0].deviceID = 541531029;
                config.channels[0].slaveDevices[0].deviceType = 9;
                config.channels[0].slaveDevices[0].enableOSDP = 0;
                config.channels[0].slaveDevices[0].connected = 1;
                config.channels[0].slaveDevices[0].channelInfo = 0;
            }

            Console.WriteLine("Trying to set RS485ConfigEx configuration.");
            result = (BS2ErrorCode)API.BS2_SetRS485ConfigEx(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }

        }        

        void getCardConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2CardConfigEx config;
            Console.WriteLine("Trying to get CardConfigEx");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetCardConfigEx(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(sdkContext, config);
            }
        }

        public void setCardConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2CardConfigEx config = Util.AllocateStructure<BS2CardConfigEx>();

            config.seos.oid_ADF[0] = 0x01;
            config.seos.oid_ADF[1] = 0x02;

            config.seos.oid_DataObjectID[0] = 0xD0;
            config.seos.oid_DataObjectID[1] = 0xD1;

            config.seos.size_DataObject[0] = 90;
            config.seos.size_DataObject[1] = 100;

            config.seos.primaryKeyAuth[0] = 0x01;
            config.seos.primaryKeyAuth[1] = 0xFE;

            config.seos.secondaryKeyAuth[0] = 0x01;
            config.seos.secondaryKeyAuth[1] = 0x1E;

            Console.WriteLine("Trying to set CardConfigEx configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetCardConfigEx(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }

        }

        public void getDstConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2DstConfig config;

            Console.WriteLine("Trying to get daylight saving time configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetDstConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(sdkContext, config);
        }

        public void setDstConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            Console.WriteLine("How many daylight saving time schedules do you want to set? [1(default)-2]");
            Console.Write(">>>> ");
            BS2DstConfig config = Util.AllocateStructure<BS2DstConfig>();
            config.numSchedules = (byte)Util.GetInput(1);
            if (config.numSchedules < 0 || config.numSchedules > 2)
            {
                Console.WriteLine("Invalid parameter");
                return;
            }

            for (int idx = 0; idx < config.numSchedules; ++idx)
            {
                Console.WriteLine("Configure DST schedule #{0}", idx);
                Console.WriteLine("   Enter the OFFSET of the time in seconds. [Ex) 3600 means it will add 1 hour after the DST starts.]");
                Console.Write(">>>");
                config.schedules[idx].timeOffset = (int)Util.GetInput();

                Console.WriteLine("   Please enter the value for the STARTING TIME.");
                Console.WriteLine("      Enter the YEAR to start the DST schedule #{0}. [0(default) means every year]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.year = Util.GetInput((UInt16)0);
                Console.WriteLine("      Enter the MONTH to start the DST schedule #{0}. [0(Jan), 1(Feb), ... , 11(Dec)]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.month = (byte)Util.GetInput();
                Console.WriteLine("      Enter the ORDINAL of the WEEK to start the DST schedule #{0}. [0(1st week), 1(2nd week), ... , -1(Last week)]", idx);
                Console.WriteLine("      The start of the week is based on Monday.");
                Console.Write("   >>>");
                config.schedules[idx].startTime.ordinal = (sbyte)Util.GetInput();
                Console.WriteLine("      Enter the DAY of the WEEK to start the DST schedule #{0}. [0(Sun), 1(Mon), ... , 6(Sat)]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.weekDay = (byte)Util.GetInput();
                Console.WriteLine("      Enter the HOUR to start the DST schedule #{0}. [0 ~ 23]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.hour = (byte)Util.GetInput();
                Console.WriteLine("      Enter the MINUTE to start the DST schedule #{0}. [0 ~ 59]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.minute = (byte)Util.GetInput();
                Console.WriteLine("      Enter the SECOND to start the DST schedule #{0}. [0 ~ 59]", idx);
                Console.Write("   >>>");
                config.schedules[idx].startTime.second = (byte)Util.GetInput();

                Console.WriteLine("   Please enter the value for the ENDING TIME.");
                Console.WriteLine("      Enter the YEAR to end the DST schedule #{0}. [0(default) means every year]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.year = Util.GetInput((UInt16)0);
                Console.WriteLine("      Enter the MONTH to end the DST schedule #{0}. [0(Jan), 1(Feb), ... , 11(Dec)]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.month = (byte)Util.GetInput();
                Console.WriteLine("      Enter the ORDINAL of the WEEK to end the DST schedule #{0}. [0(1st week), 1(2nd week), ... , -1(Last week)]", idx);
                Console.WriteLine("      The start of the week is based on Monday.");
                Console.Write("   >>>");
                config.schedules[idx].endTime.ordinal = (sbyte)Util.GetInput();
                Console.WriteLine("      Enter the DAY of the WEEK to end the DST schedule #{0}. [0(Sun), 1(Mon), ... , 6(Sat)]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.weekDay = (byte)Util.GetInput();
                Console.WriteLine("      Enter the HOUR to end the DST schedule #{0}. [0 ~ 23]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.hour = (byte)Util.GetInput();
                Console.WriteLine("      Enter the MINUTE to end the DST schedule #{0}. [0 ~ 59]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.minute = (byte)Util.GetInput();
                Console.WriteLine("      Enter the SECOND to end the DST schedule #{0}. [0 ~ 59]", idx);
                Console.Write("   >>>");
                config.schedules[idx].endTime.second = (byte)Util.GetInput();
            }

            Console.WriteLine("Trying to set Daylight Saving Time configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetDstConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }
            else
            {
                Console.WriteLine("Set DstConfig Succeeded");
            }
        }

        public void getDesFireCardConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2DesFireCardConfigEx config;

            Console.WriteLine("Trying to get DesFire card configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetDesFireCardConfigEx(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(config);
        }

        public void setDesFireCardConfigEx(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2DesFireCardConfigEx config = Util.AllocateStructure<BS2DesFireCardConfigEx>();

            Console.WriteLine("Enter the hexadecimal application master key for DesFireCardConfigEx. [KEY1-KEY2-...-KEY16]");
            Console.Write(">>>> ");
            enterSmartcardKey(config.desfireAppKey.appMasterKey);

            Console.WriteLine("Enter the hexadecimal file read key. [KEY1-KEY2-...-KEY16]");
            Console.Write(">>>> ");
            enterSmartcardKey(config.desfireAppKey.fileReadKey);

            Console.WriteLine("Enter the file read key index.");
            Console.Write(">>>> ");
            config.desfireAppKey.fileReadKeyNumber = (byte)Util.GetInput();

            Console.WriteLine("Enter the hexadecimal file write key. [KEY1-KEY2-...-KEY16]");
            Console.Write(">>>> ");
            enterSmartcardKey(config.desfireAppKey.fileWriteKey);

            Console.WriteLine("Enter the file write key index.");
            Console.Write(">>>> ");
            config.desfireAppKey.fileWriteKeyNumber = (byte)Util.GetInput();

            Console.WriteLine("Trying to set DesFire card configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetDesFireCardConfigEx(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }
            else
            {
                Console.WriteLine("Set DesFire card configuration succeeded");
            }
        }

        void enterSmartcardKey(byte[] dst)
        {
            int index = 0;
            string[] keys = Console.ReadLine().Split('-');
            foreach (string key in keys)
            {
                dst[index++] = Convert.ToByte(key, 16);
                if (index > dst.Length)
                {
                    return;
                }
            }

            for (; index < dst.Length; ++index)
            {
                dst[index] = 0xFF;
            }
        }

        void getSystemConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2SystemConfig config;
            Console.WriteLine("Trying to get SystemConfig");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetSystemConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
            else
            {
                print(config);
            }
        }

        public void setSystemConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2SystemConfig config = Util.AllocateStructure<BS2SystemConfig>();

            Console.WriteLine("Enter a timezone in seconds. [Ex. Seoul: 32400 (= 9 * 60 * 60)]");
            Console.Write(">>>> ");
            config.timezone = Util.GetInput();

            Console.WriteLine("Do you want to synchronize time with server? [y/n]");
            Console.Write(">>>> ");
            config.syncTime = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Do you want to use interphone? [y/n]");
            Console.Write(">>>> ");
            config.useInterphone = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Do you want to use OSDP secure key? [y/n]");
            Console.Write(">>>> ");
            config.keyEncrypted = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Do you want to use job codes? [y/n]");
            Console.Write(">>>> ");
            config.useJobCode = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Do you want to use alphanumeric ID? [y/n]");
            Console.Write(">>>> ");
            config.useAlphanumericID = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Enter frequency of camera [1: 50Hz, 2: 60Hz]");
            Console.Write(">>>> ");
            config.cameraFrequency = (UInt32)Util.GetInput();

            Console.WriteLine("Do you want to use security tamper? [y/n]");
            Console.Write(">>>> ");
            config.secureTamper = Util.IsYes() ? (byte)1 : (byte)0;

            Console.WriteLine("Do you want to change the type of card the device reads? [y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                Console.WriteLine("Enter the card combination you wish to set.");
                Console.WriteLine("    DEFAULT : 0xFFFFFFFF");
                Console.WriteLine("    BLE : 0x00000200");
                Console.WriteLine("    NFC : 0x00000100");
                Console.WriteLine("    SEOS : 0x00000080");
                Console.WriteLine("    SR_SE : 0x00000040");
                Console.WriteLine("    DESFIRE_EV1 : 0x00000020");
                Console.WriteLine("    CLASSIC_PLUS : 0x00000010");
                Console.WriteLine("    ICLASS : 0x00000008");
                Console.WriteLine("    MIFARE_FELICA : 0x00000004");
                Console.WriteLine("    HIDPROX : 0x00000002");
                Console.WriteLine("    EM : 0x00000001");
                Console.Write(">>>> ");

                UInt32 defaultMask = 0xFFFFFFFF;
                config.useCardOperationMask = (UInt32)Util.GetInput(defaultMask);
                config.useCardOperationMask |= (UInt32)BS2SystemConfigCardOperationMask.CARD_OPERATION_USE; // Card operation apply
            }
            else
            {
                config.useCardOperationMask = (UInt32)BS2SystemConfigCardOperationMask.CARD_OPERATION_MASK_DEFAULT;
            }

            Console.WriteLine("Trying to set System configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetSystemConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

        public void getInputConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2InputConfig config;

            Console.WriteLine("Trying to get input configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetInputConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(sdkContext, config);
        }

        public void setInputConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2InputConfig config = Util.AllocateStructure<BS2InputConfig>();

            Console.WriteLine("Trying to get input configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetInputConfig(sdkContext, deviceID, out config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(sdkContext, config);

            Console.WriteLine("Do you want to change settings of device? [Y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                Console.WriteLine("Please enter Number of inputs.");
                Console.Write(">>>> ");
                config.numInputs = (byte)Util.GetInput();
                if (config.numInputs < 0 || config.numInputs > 10)
                {
                    Console.WriteLine("Invalid parameter (numInputs)");
                    return;
                }

                Console.WriteLine("Please enter Number of supervised-inputs.");
                Console.Write(">>>> ");
                config.numSupervised = (byte)Util.GetInput();
                if (config.numSupervised < 0 || config.numSupervised > 8)
                {
                    Console.WriteLine("Invalid parameter (numSupervised)");
                    return;
                }

                for (int idx = 0; idx < BS2Environment.BS2_MAX_INPUT_NUM; ++idx)
                {
                    if (idx < config.numSupervised)
                    {
                        Console.WriteLine(">>>> supervised_inputs[{0}]", idx);

                        config.supervised_inputs[idx].portIndex = (byte)idx;

                        Console.Write("    Please enter enabled (0, 1) : ");
                        config.supervised_inputs[idx].enabled = (byte)Util.GetInput();

                        Console.Write("    Please enter superviced_index : ");
                        config.supervised_inputs[idx].supervised_index = (byte)Util.GetInput();

                        if (255 == config.supervised_inputs[idx].supervised_index)
                        {
                            Console.Write("    Please enter shortInput.minValue : ");
                            config.supervised_inputs[idx].config.shortInput.minValue = (UInt16)Util.GetInput();
                            Console.Write("    Please enter shortInput.maxValue : ");
                            config.supervised_inputs[idx].config.shortInput.maxValue = (UInt16)Util.GetInput();

                            Console.Write("    Please enter openInput.minValue : ");
                            config.supervised_inputs[idx].config.openInput.minValue = (UInt16)Util.GetInput();
                            Console.Write("    Please enter openInput.maxValue : ");
                            config.supervised_inputs[idx].config.openInput.maxValue = (UInt16)Util.GetInput();

                            Console.Write("    Please enter onInput.minValue : ");
                            config.supervised_inputs[idx].config.onInput.minValue = (UInt16)Util.GetInput();
                            Console.Write("    Please enter onInput.maxValue : ");
                            config.supervised_inputs[idx].config.onInput.maxValue = (UInt16)Util.GetInput();

                            Console.Write("    Please enter offInput.minValue : ");
                            config.supervised_inputs[idx].config.offInput.minValue = (UInt16)Util.GetInput();
                            Console.Write("    Please enter offInput.maxValue : ");
                            config.supervised_inputs[idx].config.offInput.maxValue = (UInt16)Util.GetInput();
                        }
                        else
                        {
                            config.supervised_inputs[idx].config.shortInput.minValue = 0;
                            config.supervised_inputs[idx].config.shortInput.maxValue = 0;
                            config.supervised_inputs[idx].config.openInput.minValue = 0;
                            config.supervised_inputs[idx].config.openInput.maxValue = 0;
                            config.supervised_inputs[idx].config.onInput.minValue = 0;
                            config.supervised_inputs[idx].config.onInput.maxValue = 0;
                            config.supervised_inputs[idx].config.offInput.minValue = 0;
                            config.supervised_inputs[idx].config.offInput.maxValue = 0;
                        }
                    }
                    else
                    {
                        config.supervised_inputs[idx].portIndex = (byte)idx;
                        config.supervised_inputs[idx].enabled = 0;
                        config.supervised_inputs[idx].supervised_index = 1;
                        config.supervised_inputs[idx].config.shortInput.minValue = 0;
                        config.supervised_inputs[idx].config.shortInput.maxValue = 0;
                        config.supervised_inputs[idx].config.openInput.minValue = 0;
                        config.supervised_inputs[idx].config.openInput.maxValue = 0;
                        config.supervised_inputs[idx].config.onInput.minValue = 0;
                        config.supervised_inputs[idx].config.onInput.maxValue = 0;
                        config.supervised_inputs[idx].config.offInput.minValue = 0;
                        config.supervised_inputs[idx].config.offInput.maxValue = 0;
                    }
                }
            }

            Console.WriteLine("Trying to set input configuration.");
            result = (BS2ErrorCode)API.BS2_SetInputConfig(sdkContext, deviceID, ref config);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }
            else
            {
                Console.WriteLine("Set InputConfig Succeeded");
            }
        }

        public void getDataEncryptKey(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            Console.WriteLine("Trying to get data encrypt key.");

            BS2EncryptKey keyInfo;
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetDataEncryptKey(sdkContext, deviceID, out keyInfo);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(keyInfo);
        }

        public void setDataEncryptKey(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            Console.WriteLine("Trying to set data encrypt key.");

            Console.WriteLine("If the key change is successful, all users of the device will be deleted.");
            Console.WriteLine("Do you want to continue? [y/n]");
            Console.Write(">>>> ");
            if (Util.IsYes())
            {
                BS2EncryptKey keyInfo = Util.AllocateStructure<BS2EncryptKey>();

                Console.WriteLine("Write please key string.");
                Console.Write(">>>> ");
                string keyString = Console.ReadLine();
                byte[] buff = Encoding.UTF8.GetBytes(keyString);

                Array.Clear(keyInfo.key, 0, BS2Environment.BS2_ENC_KEY_SIZE);
                Array.Copy(buff, 0, keyInfo.key, 0, keyString.Length);

                BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetDataEncryptKey(sdkContext, deviceID, ref keyInfo);
                if (result != BS2ErrorCode.BS_SDK_SUCCESS)
                {
                    Console.WriteLine("Got error({0}).", result);
                    return;
                }
            }
        }

        public void removeDataEncryptKey(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            Console.WriteLine("Trying to remove data encrypt key.");

            BS2ErrorCode result = (BS2ErrorCode)API.BS2_RemoveDataEncryptKey(sdkContext, deviceID);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }
        }

        public void getAuthConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2AuthConfig authConfig;
            Console.WriteLine("Trying to get authentication configuration");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_GetAuthConfig(sdkContext, deviceID, out authConfig);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
                return;
            }

            print(authConfig);
        }

        public void setAuthConfig(IntPtr sdkContext, UInt32 deviceID, bool isMasterDevice)
        {
            BS2AuthConfig authConfig = Util.AllocateStructure<BS2AuthConfig>();
            bool stop = false;

            do
            {
                Console.WriteLine("Select auth mode.");
                Console.WriteLine("  0. Biometric Only");
                Console.WriteLine("  1. Biometric + PIN");
                Console.WriteLine("  2. Card Only");
                Console.WriteLine("  3. Card + Biometric");
                Console.WriteLine("  4. Card + PIN");
                Console.WriteLine("  5. Card + Biometric/PIN");
                Console.WriteLine("  6. Card + Biometric + PIN");
                Console.WriteLine("  7. ID + Biometric");
                Console.WriteLine("  8. ID + PIN");
                Console.WriteLine("  9. ID + Biometric/PIN");
                Console.WriteLine(" 10. ID + Biometric + PIN");
                Console.WriteLine("999. No more changes.");
                Console.Write(">> ");
                UInt32 mode = (UInt32)Util.GetInput();
                if ((UInt32)BS2IDAuthModeEnum.ID_BIOMETRIC_PIN < mode && 999 != mode)
                {
                    Console.WriteLine("Invalid auth mode");
                    return;
                }

                if (999 == mode)
                {
                    stop = true;
                }
                else
                {
                    Console.WriteLine("0. Off (No time)");
                    Console.WriteLine("1. On (Always)");
                    Console.Write(">> ");
                    UInt32 onoff = (UInt32)Util.GetInput();
                    if (0 == onoff || 1 == onoff)
                    {
                        authConfig.authSchedule[mode] = onoff;
                    }
                }
            } while (!stop);

            Console.WriteLine("Insert global APB option. (0: Not use, 1: Use)");
            Console.Write(">> ");
            authConfig.useGlobalAPB = Util.GetInput((byte)0);

            Console.WriteLine("Insert global APB fail action. (0: Not use, 1: Soft APB, 2: Hard APB)");
            Console.Write(">> ");
            authConfig.globalAPBFailAction = Util.GetInput((byte)0);

            Console.WriteLine("Insert private authentication. (0: Not use, 1: Use)");
            Console.Write(">> ");
            authConfig.usePrivateAuth = Util.GetInput((byte)0);

            Console.WriteLine("Insert face detection level. (0: Not use, 1: Normal mode, 2: Strict mode)");
            Console.Write(">> ");
            authConfig.faceDetectionLevel = Util.GetInput((byte)0);

            Console.WriteLine("Insert server matching option. (0: Not use, 1: Use)");
            Console.Write(">> ");
            authConfig.useServerMatching = Util.GetInput((byte)0);

            Console.WriteLine("Using full access. (0: Not use, 1: Use)");
            Console.Write(">> ");
            authConfig.useFullAccess = Util.GetInput((byte)0);

            Console.WriteLine("Insert matching timeout in seconds");
            Console.Write(">> ");
            authConfig.matchTimeout = Util.GetInput((byte)5);

            Console.WriteLine("Insert authentication timeout in seconds");
            Console.Write(">> ");
            authConfig.authTimeout = Util.GetInput((byte)10);

            authConfig.numOperators = 0;

            Console.WriteLine("Trying to set authentication configuration.");
            BS2ErrorCode result = (BS2ErrorCode)API.BS2_SetAuthConfig(sdkContext, deviceID, ref authConfig);
            if (result != BS2ErrorCode.BS_SDK_SUCCESS)
            {
                Console.WriteLine("Got error({0}).", result);
            }
        }

        void print(IntPtr sdkContext, BS2DstConfig config)
        {
            Console.WriteLine(">>>> Daylight saving time configuration ");
            Console.WriteLine("     |--numSchedules : {0}", config.numSchedules);
            for (int idx = 0; idx < BS2Environment.BS2_MAX_DST_SCHEDULE; idx++)
            {
                Console.WriteLine("     |--schedules[{0}]", idx);
                Console.WriteLine("         |--timeOffset : {0}", config.schedules[idx].timeOffset);
                Console.WriteLine("         |--startTime");
                Console.WriteLine("             |--year : {0}", config.schedules[idx].startTime.year);
                Console.WriteLine("             |--month : {0}", config.schedules[idx].startTime.month);
                Console.WriteLine("             |--ordinal : {0}", config.schedules[idx].startTime.ordinal);
                Console.WriteLine("             |--weekDay : {0}", config.schedules[idx].startTime.weekDay);
                Console.WriteLine("             |--hour : {0}", config.schedules[idx].startTime.hour);
                Console.WriteLine("             |--minute : {0}", config.schedules[idx].startTime.minute);
                Console.WriteLine("             |--second : {0}", config.schedules[idx].startTime.second);
                Console.WriteLine("         |--endTime");
                Console.WriteLine("             |--year : {0}", config.schedules[idx].endTime.year);
                Console.WriteLine("             |--month : {0}", config.schedules[idx].endTime.month);
                Console.WriteLine("             |--ordinal : {0}", config.schedules[idx].endTime.ordinal);
                Console.WriteLine("             |--weekDay : {0}", config.schedules[idx].endTime.weekDay);
                Console.WriteLine("             |--hour : {0}", config.schedules[idx].endTime.hour);
                Console.WriteLine("             |--minute : {0}", config.schedules[idx].endTime.minute);
                Console.WriteLine("             |--second : {0}", config.schedules[idx].endTime.second);
            }
            Console.WriteLine("<<<< ");
        }

        void print(BS2DesFireCardConfigEx config)
        {
            Console.WriteLine(">>>> DesFire card configuration ");
            Console.WriteLine("     |--appMasterKey : {0}", config.desfireAppKey.appMasterKey);
            Console.WriteLine("     |--fileReadKey : {0}", config.desfireAppKey.fileReadKey);
            Console.WriteLine("     |--fileWriteKey : {0}", config.desfireAppKey.fileWriteKey);
            Console.WriteLine("     |--fileReadKeyNumber : {0}", config.desfireAppKey.fileReadKeyNumber);
            Console.WriteLine("     +--fileWriteKeyNumber : {0}", config.desfireAppKey.fileWriteKeyNumber);
            Console.WriteLine("<<<< ");
        }

        void print(BS2SystemConfig config)
        {
            Console.WriteLine(">>>> System configuration ");
            Console.WriteLine("     |--timezone : {0}", config.timezone);
            Console.WriteLine("     |--syncTime : {0}", config.syncTime);
            Console.WriteLine("     |--serverSync : {0}", config.serverSync);
            Console.WriteLine("     |--deviceLocked : {0}", config.deviceLocked);
            Console.WriteLine("     |--useInterphone : {0}", config.useInterphone);
            //Console.WriteLine("     |--useUSBConnection : {0}", config.useUSBConnection);
            Console.WriteLine("     |--keyEncrypted : {0}", config.keyEncrypted);
            Console.WriteLine("     |--useJobCode : {0}", config.useJobCode);
            Console.WriteLine("     |--useAlphanumericID : {0}", config.useAlphanumericID);
            Console.WriteLine("     |--cameraFrequency : {0}", config.cameraFrequency);
            Console.WriteLine("     |--secureTamper : {0}", config.secureTamper);
            Console.WriteLine("     +--useCardOperationMask : {0}", config.useCardOperationMask);
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2InputConfig config)
        {
            Console.WriteLine(">>>> Input configuration ");
            Console.WriteLine("     |--numInputs     : {0}", config.numInputs);
            Console.WriteLine("     |--numSupervised : {0}", config.numSupervised);
            for (int idx = 0; idx < BS2Environment.BS2_MAX_INPUT_NUM; idx++)
            {
                Console.WriteLine("     +--supervised_inputs[{0}]", idx);
                Console.WriteLine("     |--    portIndex        : {0}", config.supervised_inputs[idx].portIndex);
                Console.WriteLine("     |--    enabled          : {0}", config.supervised_inputs[idx].enabled);
                Console.WriteLine("     |--    supervised_index : {0}", config.supervised_inputs[idx].supervised_index);
                Console.WriteLine("     |--    config.shortInput.minValue : {0}", config.supervised_inputs[idx].config.shortInput.minValue);
                Console.WriteLine("     |--    config.shortInput.maxValue : {0}", config.supervised_inputs[idx].config.shortInput.maxValue);
                Console.WriteLine("     |--    config.openInput.minValue  : {0}", config.supervised_inputs[idx].config.openInput.minValue);
                Console.WriteLine("     |--    config.openInput.maxValue  : {0}", config.supervised_inputs[idx].config.openInput.maxValue);
                Console.WriteLine("     |--    config.onInput.minValue    : {0}", config.supervised_inputs[idx].config.onInput.minValue);
                Console.WriteLine("     |--    config.onInput.maxValue    : {0}", config.supervised_inputs[idx].config.onInput.maxValue);
                Console.WriteLine("     |--    config.offInput.minValue   : {0}", config.supervised_inputs[idx].config.offInput.minValue);
                Console.WriteLine("     +--    config.offInput.maxValue   : {0}", config.supervised_inputs[idx].config.offInput.maxValue);
            }
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS1CardConfig config)
        {
            Console.WriteLine(">>>> BS1Card configuration ");
            Console.WriteLine("     |--magicNo : {0}", config.magicNo);
            Console.WriteLine("     |--disabled : {0}", config.disabled);
            Console.WriteLine("     |--useCSNOnly : {0}", config.useCSNOnly);
            Console.WriteLine("     |--bioentryCompatible : {0}", config.bioentryCompatible);
            Console.WriteLine("     |--useSecondaryKey : {0}", config.useSecondaryKey);
            Console.WriteLine("     |--primaryKey : {0}", BitConverter.ToString(config.primaryKey));
            Console.WriteLine("     |--secondaryKey : {0}", BitConverter.ToString(config.secondaryKey));
            Console.WriteLine("     |--cisIndex : {0}", config.cisIndex);
            Console.WriteLine("     |--numOfTemplate : {0}", config.numOfTemplate);
            Console.WriteLine("     |--templateSize : {0}", config.templateSize);
            Console.WriteLine("     |--templateStartBlock : {0},{1},{2},{3}", config.templateStartBlock[0], config.templateStartBlock[1], config.templateStartBlock[2], config.templateStartBlock[3]);
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2SystemConfigExt config)
        {
            Console.WriteLine(">>>> SystemExt configuration ");
            Console.WriteLine("     |--primarySecureKey : {0}", BitConverter.ToString(config.primarySecureKey));
            Console.WriteLine("     |--secondarySecureKey : {0}", BitConverter.ToString(config.secondarySecureKey));           
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2VoipConfig config)
        {
            Console.WriteLine(">>>> Voip configuration ");            
            Console.WriteLine("     |--serverUrl : {0}", Encoding.UTF8.GetString(config.serverUrl).TrimEnd('\0'));
            Console.WriteLine("     |--serverPort : {0}", config.serverPort);
            Console.WriteLine("     |--userID : {0}", Encoding.UTF8.GetString(config.userID).TrimEnd('\0'));
            Console.WriteLine("     |--userPW : {0}", Encoding.UTF8.GetString(config.userPW).TrimEnd('\0'));
            Console.WriteLine("     |--exitButton : {0}", config.exitButton);
            Console.WriteLine("     |--dtmfMode : {0}", config.dtmfMode);
            Console.WriteLine("     |--bUse : {0}", config.bUse);
            Console.WriteLine("     |--reseverd : {0}", config.reseverd[0]);
            Console.WriteLine("     |--numPhonBook : {0}", config.numPhonBook);            
            for (int idx = 0; idx < config.numPhonBook; ++idx)
            {
                Console.WriteLine("     |++PhoneItem[{0}]", idx);
                Console.WriteLine("         |--phoneNumber : {0}", Encoding.UTF8.GetString(config.phonebook[idx].phoneNumber).TrimEnd('\0'));
                Console.WriteLine("         |--descript : {0}", Encoding.UTF8.GetString(config.phonebook[idx].descript).TrimEnd('\0'));                
            }
           
            
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2FaceConfig config)
        {          
            Console.WriteLine(">>>> Face configuration ");            
            Console.WriteLine("     |--securityLevel : {0}", config.securityLevel);           
            Console.WriteLine("     |--lightCondition : {0}", config.lightCondition);
            Console.WriteLine("     |--enrollThreshold : {0}", config.enrollThreshold);            
            Console.WriteLine("     |--detectSensitivity : {0}", config.detectSensitivity);
            Console.WriteLine("     |--enrollTimeout : {0}", config.enrollTimeout);
            Console.WriteLine("     |--lfdLevel : {0}", config.lfdLevel);
            Console.WriteLine("     |--quickEnrollment : {0}", config.quickEnrollment);
            Console.WriteLine("     |--checkDuplicate : {0}", config.checkDuplicate);
            Console.WriteLine("     |--previewOption : {0}", config.previewOption);

            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2CardConfigEx config)
        {
            Console.WriteLine(">>>> CardEx configuration ");
            Console.WriteLine("     |--oid_ADF : {0}", BitConverter.ToString(config.seos.oid_ADF));
            Console.WriteLine("     |--size_ADF : {0}", config.seos.size_ADF);
            Console.WriteLine("     |--oid_DataObjectID : {0}", BitConverter.ToString(config.seos.oid_DataObjectID));
            Console.WriteLine("     |++size_DataObject");
            for (int i = 0; i < 8; ++i)
            {
                Console.WriteLine("     |--size{0} : {1}", i, config.seos.size_DataObject[i]);
            }

            Console.WriteLine("     |--primaryKeyAuth : {0}", BitConverter.ToString(config.seos.primaryKeyAuth));
            Console.WriteLine("     |--secondaryKeyAuth : {0}", BitConverter.ToString(config.seos.secondaryKeyAuth));

            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2AuthGroup authGroup)
        {
            Console.WriteLine(">>>> AuthGroup id[{0}] name[{1}]", authGroup.id, Encoding.UTF8.GetString(authGroup.name).TrimEnd('\0'));            
        }

        void print(IntPtr sdkContext, BS2DisplayConfig config)
        {
            Console.WriteLine(">>>> Display configuration ");
            Console.WriteLine("     |--useUserPhrase : {0}", config.useUserPhrase);            
            Console.WriteLine("<<<< ");
        }

        void print(IntPtr sdkContext, BS2Rs485ConfigEX config)
        {
            Console.WriteLine(">>>> Rs485ConfigEX configuration ");            
            Console.WriteLine("     |--mode : {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", config.mode[0], config.mode[1], config.mode[2], config.mode[3], config.mode[4], config.mode[5], config.mode[6], config.mode[7]);
            Console.WriteLine("     |--numOfChannels : {0}", config.numOfChannels);

            for (int idx = 0; idx < config.numOfChannels; ++idx)
            {
                Console.WriteLine("     |++channels[{0}]", idx);
                Console.WriteLine("         |--baudRate : {0}", config.channels[idx].baudRate);
                Console.WriteLine("         |--channelIndex : {0}", config.channels[idx].channelIndex);
                Console.WriteLine("         |--useRegistance : {0}", config.channels[idx].useRegistance);
                Console.WriteLine("         |--numOfDevices : {0}", config.channels[idx].numOfDevices);

                for (int idx2 = 0; idx2 < config.channels[idx].numOfDevices; ++idx2)
                {
                    Console.WriteLine("          |++slaveDevices[{0}]", idx2);
                    Console.WriteLine("                  |--deviceID : {0}", config.channels[idx].slaveDevices[idx2].deviceID);
                    Console.WriteLine("                  |--deviceType : {0}", config.channels[idx].slaveDevices[idx2].deviceType);
                    Console.WriteLine("                  |--enableOSDP : {0}", config.channels[idx].slaveDevices[idx2].enableOSDP);
                    Console.WriteLine("                  |--connected : {0}", config.channels[idx].slaveDevices[idx2].connected);
                    Console.WriteLine("                  |--channelInfo : {0}", config.channels[idx].slaveDevices[idx2].channelInfo);
                }
            }
            
            Console.WriteLine("<<<< ");
        }

        void print(BS2EncryptKey keyInfo)
        {
            Console.WriteLine(">>>> EncryptKey Information");
            Console.WriteLine("     +--key : {0}", BitConverter.ToString(keyInfo.key));
            //Console.WriteLine("     +--key : {0}", Encoding.UTF8.GetString(keyInfo.key).TrimEnd('\0'));

            Console.WriteLine("<<<< ");
        }

        void print(BS2AuthConfig config)
        {
            Console.WriteLine(">>>> AuthConfig configuration ");
            Console.WriteLine("     +--authSchedule");
            Console.WriteLine("        +--Biometric Only : {0}", config.authSchedule[(int)BS2FingerAuthModeEnum.BIOMETRIC_ONLY]);
            Console.WriteLine("        |--Biometric + PIN : {0}", config.authSchedule[(int)BS2FingerAuthModeEnum.BIOMETRIC_PIN]);
            Console.WriteLine("        |--Card Only : {0}", config.authSchedule[(int)BS2CardAuthModeEnum.CARD_ONLY]);
            Console.WriteLine("        |--Card + Biometric : {0}", config.authSchedule[(int)BS2CardAuthModeEnum.CARD_BIOMETRIC]);
            Console.WriteLine("        |--Card + PIN : {0}", config.authSchedule[(int)BS2CardAuthModeEnum.CARD_PIN]);
            Console.WriteLine("        |--Card + Biometric/PIN : {0}", config.authSchedule[(int)BS2CardAuthModeEnum.CARD_BIOMETRIC_OR_PIN]);
            Console.WriteLine("        |--Card + Biometric + PIN : {0}", config.authSchedule[(int)BS2CardAuthModeEnum.CARD_BIOMETRIC_PIN]);
            Console.WriteLine("        |--ID + Biometric : {0}", config.authSchedule[(int)BS2IDAuthModeEnum.ID_BIOMETRIC]);
            Console.WriteLine("        |--ID + PIN : {0}", config.authSchedule[(int)BS2IDAuthModeEnum.ID_PIN]);
            Console.WriteLine("        |--ID + Biometric/PIN : {0}", config.authSchedule[(int)BS2IDAuthModeEnum.ID_BIOMETRIC_OR_PIN]);
            Console.WriteLine("        +--ID + Biometric + PIN : {0}", config.authSchedule[(int)BS2IDAuthModeEnum.ID_BIOMETRIC_PIN]);
            Console.WriteLine("     +--useGlobalAPB : {0}", config.useGlobalAPB);
            Console.WriteLine("     |--globalAPBFailAction : {0}", config.globalAPBFailAction);
            Console.WriteLine("     |--usePrivateAuth : {0}", config.usePrivateAuth);
            Console.WriteLine("     |--faceDetectionLevel : {0}", config.faceDetectionLevel);
            Console.WriteLine("     |--useServerMatching : {0}", config.useServerMatching);
            Console.WriteLine("     |--useFullAccess : {0}", config.useFullAccess);
            Console.WriteLine("     |--matchTimeout : {0}", config.matchTimeout);
            Console.WriteLine("     |--authTimeout : {0}", config.authTimeout);
            Console.WriteLine("     +--numOperators : {0}", config.numOperators);
            Console.WriteLine("<<<< ");
        }
    }
}
