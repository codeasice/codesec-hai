using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HAI_Shared;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;

/*
   ___         _                
  / __|___  __| |___ ___ ___ __ 
 | (__/ _ \/ _` / -_|_-</ -_) _|
  \___\___/\__,_\___/__/\___\__|
*/
namespace codesec
{
    class Codesec
    {
        private static clsHAC HAC = null;

        private String HAC_IP = ConfigurationSettings.AppSettings["HAC_IP"];
        private ushort HAC_Port = ushort.Parse(ConfigurationSettings.AppSettings["HAC_Port"]);
        private String HAC_Enc1 = ConfigurationSettings.AppSettings["HAC_Enc1"];
        private String HAC_Enc2 = ConfigurationSettings.AppSettings["HAC_Enc2"];

         private FayeClient rc;
         public Codesec()
         {
            rc = new FayeClient();
            Log("   ___         _                ");
            Log("  / __|___  __| |___ ___ ___ __ ");
            Log(" | (__/ _ \\/ _` / -_|_-</ -_) _|");
            Log("  \\___\\___/\\__,_\\___/__/\\___\\__|");
            Log("Codesec HAI Omni LTe Connector");
            Log("");

            HAC = new clsHAC();
            LogNoLine("Connecting to HAC...");
            Connect();
            Log("[DONE]");

            bool doinstuff = true;
            int timer = 0;
            Log("Strike a key or wait 5 seconds...");
             while (doinstuff)
             {
                 if (Console.KeyAvailable)
                 {
                     ConsoleKeyInfo key;
                     key = Console.ReadKey(true); //true parameter doesn't display key in console
                     doinstuff = false;
                 }
                 System.Threading.Thread.Sleep(100);  // Why waste a whole second 
                 timer++;
                 if (timer > 50) 
                     doinstuff = false;
             }
             Log("[DONE]");

             LogNoLine("Enabling Notification...");
             HAC.Connection.Send(new clsOL2EnableNotifications(HAC.Connection, true), null);
             Log("[DONE]");

             Log("Strike the Escape (Esc) key to quit:");
             doinstuff = true;
             while (doinstuff)
             {
                 if (Console.KeyAvailable)
                 {
                     ConsoleKeyInfo key;
                     key = Console.ReadKey(true); //true parameter doesn't display key in console

                     //if ((key.Modifiers & ConsoleModifiers.Control) != 0)
                     if (key.Key == ConsoleKey.Escape)
                         doinstuff = false;
                 }
                 System.Threading.Thread.Sleep(100);
             }
             Environment.Exit(0);
         }

         private void Connect()
         {
            HAC.Connection.NetworkAddress = HAC_IP;
            HAC.Connection.NetworkPort = HAC_Port;
            HAC.Connection.ControllerKey = clsUtil.HexString2ByteArray(
            String.Concat(HAC_Enc1,HAC_Enc2));
            HAC.Connection.ConnectionType = enuOmniLinkConnectionType.Network_TCP;
            HAC.Connection.Connect(HandleConnectStatus, HandleUnsolicitedPackets);
        }

         private void HandleConnectStatus(enuOmniLinkCommStatus CS)
         {
                 switch (CS)
                 {
                     case enuOmniLinkCommStatus.NoReply:
                         LogLine("CONNECTION STATUS: No Reply");
                         break;
                     case enuOmniLinkCommStatus.UnrecognizedReply:
                         LogLine("CONNECTION STATUS: Unrecognized Reply");
                         break;
                     case enuOmniLinkCommStatus.UnsupportedProtocol:
                         LogLine("CONNECTION STATUS: Unsupported Protocol");
                         break;
                     case enuOmniLinkCommStatus.ClientSessionTerminated:
                         LogLine("CONNECTION STATUS: Client Session Terminated");
                         break;
                     case enuOmniLinkCommStatus.ControllerSessionTerminated:
                         LogLine("CONNECTION STATUS: Controller Session Terminated");
                         break;
                     case enuOmniLinkCommStatus.CannotStartNewSession:
                         LogLine("CONNECTION STATUS: Cannot Start New Session");
                         break;
                     case enuOmniLinkCommStatus.LoginFailed:
                         LogLine("CONNECTION STATUS: Login Failed");
                         break;
                     case enuOmniLinkCommStatus.UnableToOpenSocket:
                         LogLine("CONNECTION STATUS: Unable To Open Socket");
                         break;
                     case enuOmniLinkCommStatus.UnableToConnect:
                         LogLine("CONNECTION STATUS: Unable To Connect");
                         break;
                     case enuOmniLinkCommStatus.SocketClosed:
                         LogLine("CONNECTION STATUS: Socket Closed");
                         break;
                     case enuOmniLinkCommStatus.UnexpectedError:
                         LogLine("CONNECTION STATUS: Unexpected Error");
                         break;
                     case enuOmniLinkCommStatus.UnableToCreateSocket:
                         LogLine("CONNECTION STATUS: Unable To Create Socket");
                         break;
                     case enuOmniLinkCommStatus.Retrying:
                         LogLine("CONNECTION STATUS: Retrying");
                         break;
                     case enuOmniLinkCommStatus.Connected:
                         LogLine("Connected");
                         break;
                     case enuOmniLinkCommStatus.Connecting:
                         LogLine("CONNECTION STATUS: Connecting");
                         break;
                     case enuOmniLinkCommStatus.Disconnected:
                         LogLine("CONNECTION STATUS: Disconnected");
                         break;
                     case enuOmniLinkCommStatus.InterruptedFunctionCall:
                         LogLine("CONNECTION STATUS: Interrupted Function Call");
                         break;
                     case enuOmniLinkCommStatus.PermissionDenied:
                         LogLine("CONNECTION STATUS: Permission Denied");
                         break;
                     case enuOmniLinkCommStatus.BadAddress:
                         LogLine("CONNECTION STATUS: Bad Address");
                         break;
                     case enuOmniLinkCommStatus.InvalidArgument:
                         LogLine("CONNECTION STATUS: Invalid Argument");
                         break;
                     case enuOmniLinkCommStatus.TooManyOpenFiles:
                         LogLine("CONNECTION STATUS: Too Many Open Files");
                         break;
                     case enuOmniLinkCommStatus.ResourceTemporarilyUnavailable:
                         LogLine("CONNECTION STATUS: Resource Temporarily Unavailable");
                         break;
                     case enuOmniLinkCommStatus.OperationNowInProgress:
                         LogLine("CONNECTION STATUS: Operation Now In Progress");
                         break;
                     case enuOmniLinkCommStatus.OperationAlreadyInProgress:
                         LogLine("CONNECTION STATUS: Operation Already In Progress");
                         break;
                     case enuOmniLinkCommStatus.SocketOperationOnNonSocket:
                         LogLine("CONNECTION STATUS: Socket Operation On Non Socket");
                         break;
                     case enuOmniLinkCommStatus.DestinationAddressRequired:
                         LogLine("CONNECTION STATUS: Destination Address Required");
                         break;
                     case enuOmniLinkCommStatus.MessgeTooLong:
                         LogLine("CONNECTION STATUS: Message Too Long");
                         break;
                     case enuOmniLinkCommStatus.WrongProtocolType:
                         LogLine("CONNECTION STATUS: Wrong Protocol Type");
                         break;
                     case enuOmniLinkCommStatus.BadProtocolOption:
                         LogLine("CONNECTION STATUS: Bad Protocol Option");
                         break;
                     case enuOmniLinkCommStatus.ProtocolNotSupported:
                         LogLine("CONNECTION STATUS: Protocol Not Supported");
                         break;
                     case enuOmniLinkCommStatus.SocketTypeNotSupported:
                         LogLine("CONNECTION STATUS: Socket Type Not Supported");
                         break;
                     case enuOmniLinkCommStatus.OperationNotSupported:
                         LogLine("CONNECTION STATUS: Operation Not Supported");
                         break;
                     case enuOmniLinkCommStatus.ProtocolFamilyNotSupported:
                         LogLine("CONNECTION STATUS: Protocol Family Not Supported");
                         break;
                     case enuOmniLinkCommStatus.AddressFamilyNotSupported:
                         LogLine("CONNECTION STATUS: Address Family Not Supported");
                         break;
                     case enuOmniLinkCommStatus.AddressInUse:
                         LogLine("CONNECTION STATUS: Address In Use");
                         break;
                     case enuOmniLinkCommStatus.AddressNotAvailable:
                         LogLine("CONNECTION STATUS: Address Not Available");
                         break;
                     case enuOmniLinkCommStatus.NetworkIsDown:
                         LogLine("CONNECTION STATUS: Network Is Down");
                         break;
                     case enuOmniLinkCommStatus.NetworkIsUnreachable:
                         LogLine("CONNECTION STATUS: Network Is Unreachable");
                         break;
                     case enuOmniLinkCommStatus.NetworkReset:
                         LogLine("CONNECTION STATUS: Network Reset");
                         break;
                     case enuOmniLinkCommStatus.ConnectionAborted:
                         LogLine("CONNECTION STATUS: Connection Aborted");
                         break;
                     case enuOmniLinkCommStatus.ConnectionResetByPeer:
                         LogLine("CONNECTION STATUS: Connection Reset By Peer");
                         break;
                     case enuOmniLinkCommStatus.NoBufferSpaceAvailable:
                         LogLine("CONNECTION STATUS: No Buffer Space Available");
                         break;
                     case enuOmniLinkCommStatus.AlreadyConnected:
                         LogLine("CONNECTION STATUS: Already Connected");
                         break;
                     case enuOmniLinkCommStatus.NotConnected:
                         LogLine("CONNECTION STATUS: Not Connected");
                         break;
                     case enuOmniLinkCommStatus.CannotSendAfterShutdown:
                         LogLine("CONNECTION STATUS: Cannot Send After Shutdown");
                         break;
                     case enuOmniLinkCommStatus.ConnectionTimedOut:
                         LogLine("CONNECTION STATUS: Connection Timed Out");
                         break;
                     case enuOmniLinkCommStatus.ConnectionRefused:
                         LogLine("CONNECTION STATUS: Connection Refused");
                         break;
                     case enuOmniLinkCommStatus.HostIsDown:
                         LogLine("CONNECTION STATUS: Host Is Down");
                         break;
                     case enuOmniLinkCommStatus.HostUnreachable:
                         LogLine("CONNECTION STATUS: Host Unreachable");
                         break;
                     case enuOmniLinkCommStatus.TooManyProcesses:
                         LogLine("CONNECTION STATUS: Too Many Processes");
                         break;
                     case enuOmniLinkCommStatus.NetworkSubsystemIsUnavailable:
                         LogLine("CONNECTION STATUS: Network Subsystem Is Unavailable");
                         break;
                     case enuOmniLinkCommStatus.UnsupportedVersion:
                         LogLine("CONNECTION STATUS: Unsupported Version");
                         break;
                     case enuOmniLinkCommStatus.NotInitialized:
                         LogLine("CONNECTION STATUS: Not Initialized");
                         break;
                     case enuOmniLinkCommStatus.ShutdownInProgress:
                         LogLine("CONNECTION STATUS: Shutdown In Progress");
                         break;
                     case enuOmniLinkCommStatus.ClassTypeNotFound:
                         LogLine("CONNECTION STATUS: Class Type Not Found");
                         break;
                     case enuOmniLinkCommStatus.HostNotFound:
                         LogLine("CONNECTION STATUS: Host Not Found");
                         break;
                     case enuOmniLinkCommStatus.HostNotFoundTryAgain:
                         LogLine("CONNECTION STATUS: Host Not Found Try Again");
                         break;
                     case enuOmniLinkCommStatus.NonRecoverableError:
                         LogLine("CONNECTION STATUS: Non Recoverable Error");
                         break;
                     case enuOmniLinkCommStatus.NoDataOfRequestedType:
                         LogLine("CONNECTION STATUS: No Data Of Requested Type");
                         break;
                     default:
                         break;
                 }
              if ((HAC == null) ||
                    (HAC.Connection.ConnectionState == enuOmniLinkConnectionState.Offline))
                  LogLine("SetOnLineStatus(false);");
         }

        private bool HandleUnsolicitedPackets(byte[] B)
         {
                 if ((B.Length > 3) && (B[0] == 0x21))
                 {
                     switch ((enuOmniLink2MessageType)B[2])
                     {
                         case enuOmniLink2MessageType.ClearNames:
                             break;
                         case enuOmniLink2MessageType.DownloadNames:
                             break;
                         case enuOmniLink2MessageType.UploadNames:
                             break;
                         case enuOmniLink2MessageType.NameData:
                             break;
                         case enuOmniLink2MessageType.ClearVoices:
                             break;
                         case enuOmniLink2MessageType.DownloadVoices:
                             break;
                         case enuOmniLink2MessageType.UploadVoices:
                             break;
                         case enuOmniLink2MessageType.VoiceData:
                             break;
                         case enuOmniLink2MessageType.Command:
                             break;
                         case enuOmniLink2MessageType.EnableNotifications:
                             break;
                         case enuOmniLink2MessageType.SystemInformation:
                             break;
                         case enuOmniLink2MessageType.SystemStatus:
                             break;
                         case enuOmniLink2MessageType.SystemTroubles:
                             break;
                         case enuOmniLink2MessageType.SystemFeatures:
                             break;
                         case enuOmniLink2MessageType.Capacities:
                             break;
                         case enuOmniLink2MessageType.Properties:
                             break;
                         case enuOmniLink2MessageType.Status:
                             break;
                         case enuOmniLink2MessageType.EventLogItem:
                             break;
                         case enuOmniLink2MessageType.ValidateCode:
                             break;
                         case enuOmniLink2MessageType.SystemFormats:
                             break;
                         case enuOmniLink2MessageType.Login:
                             break;
                         case enuOmniLink2MessageType.Logout:
                             break;
                         case enuOmniLink2MessageType.ActivateKeypadEmg:
                             break;
                         case enuOmniLink2MessageType.ExtSecurityStatus:
                             break;
                         case enuOmniLink2MessageType.CmdExtSecurity:
                             break;
                         case enuOmniLink2MessageType.AudioSourceStatus:
                             break;
                         case enuOmniLink2MessageType.SystemEvents:
                             break;
                         case enuOmniLink2MessageType.ZoneReadyStatus:
                             break;
                         case enuOmniLink2MessageType.ExtendedStatus:
                             UpdateZoneStatus(B[6].ToString(), B[7].ToString());
                             break;
                         default:
                             break;
                     }
                 }
             return true;
         }

         private void UpdateZoneStatus(string zone, string status)
         {
             rc.ZoneStatus(zone, status);
             String str = GetFormattedDate() + " ZONE " + zone + " STATUS " + status;
             Console.WriteLine(str);
         }

         private void Log(String str)
         {
             LogLine(str);
         }
         private void LogLine(String str)
         {
             rc.Message(str);
             Console.WriteLine(str);
         }
         private void LogNoLine(String str)
         {
             rc.Message(str);
             Console.Write(str);
         }

         private string GetFormattedDate()
         {
             return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
         }
   }
}
