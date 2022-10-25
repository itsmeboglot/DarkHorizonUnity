using System.Net.NetworkInformation;
using UnityEngine;

public class EditorLocalhostHandler
{
   #if UNITY_EDITOR
   private string batPath = "/Test/startHttpServer.bat";
   private string simpleServerPath = "/Test/SimpleWebServer.exe";
   private int localhostPort;

   private System.Diagnostics.Process process;
   
   public void Init(int localhostPort = 8081)
   {
      this.localhostPort = localhostPort;
      StartHttpServer();
   }

   private bool CheckPort(int port)
   {
      IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
      TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

      foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
      {
         if (tcpi.LocalEndPoint.Port == port)
         {
            Debug.Log($"port is busy: {port}");
            return false;
         }
      }

      return true;
   }

   private void StartHttpServer()
   {
      bool isLocalhostPortAvailable = CheckPort(localhostPort);
      if (isLocalhostPortAvailable)
      {
         if (process == null || process.HasExited)
         {
            process = new System.Diagnostics.Process
            {
               StartInfo =
               {
                  Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\"", Application.dataPath + simpleServerPath,
                     Application.dataPath , localhostPort)
               }
            };
            process.StartInfo.FileName = Application.dataPath + batPath;
            process.Start();
         }
      }
   }

   public void Dispose()
   {
      if (process != null)
      {
         process.Kill();
         process = null;
      }
   }
   
   #endif
}
