using System;
using System.Collections.Generic;

using System.Security.Principal;
using System.Diagnostics;

namespace Common
{
    public static class AdministratorTool
    {
        public static void sendArgs(string[] args)
        {
            var exeName = Process.GetCurrentProcess().MainModule.FileName;
            ProcessStartInfo startInfo = new ProcessStartInfo(exeName);
            startInfo.Verb = "runas";

            startInfo.Arguments = "adminStart ";

            foreach (var item in startInfo.Arguments)
                startInfo.Arguments += item + " ";

            startInfo.Arguments = startInfo.Arguments.TrimEnd(' ');

            Process.Start(startInfo).WaitForExit();
        }

        public static bool isAdminStart(string[] args, EventHandler<string[]> ev)
        {
            if (ev == null)
                throw new Exception("ev == null");

            if (args != null && args.Length != 0 && args[0] == "adminStart" && IsAdministrator())
            {
                Array.Copy(args,1,args,0,args.Length-1);
                Array.Resize<string>(ref args,args.Length-1);
                if (ev != null)
                    ev(null, args);
                return true;
            }
            return false;
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
