using System;
using System.Threading;
using qPapelEasyAuth;

namespace EasyAuth_CS_Example
{
    internal class Program
    {
        private static int CalculateSecurePayload(int a, int b)
        {
            return ((a * b) ^ (a + b)) + 42;
        }

        static void Main(string[] args)
        {
            var cfg = new EasyAuthConfig
            {
                Flags = ProtectionFlags.All,
                HoneypotDelayMs = 5000,
                VmpMode = false,
                VmpEnforceProtected = false,
                ClientVersion = "2.0.0"
            };
            qPapelEasyAuth.qPapelEasyAuth.SetConfig(cfg);

            try
            {
                qPapelEasyAuth.qPapelEasyAuth.Initialize();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[-] Security Alert: {ex.GetType().Name}: {ex.Message}");
                Console.WriteLine("[-] Build Release and run the packed output (EasyAuth-CS-Example_packed.exe).");
                Thread.Sleep(3000);
                return;
            }

            Console.WriteLine("[*] Connecting to qPapelEasyAuth Server...");
            bool net_ok = qPapelEasyAuth.qPapelEasyAuth.Connect();
            if (!net_ok)
            {
                Console.WriteLine("[-] Connection: OFFLINE");
                Thread.Sleep(3000);
                return;
            }

            Console.WriteLine("[+] Connection : CONNECTED");

            string api_key = "pk_00000067_7d65a6ce769c3ee4b0c6acebf96620f0";
            var init_res = qPapelEasyAuth.qPapelEasyAuth.InitSession(api_key);
            if (!init_res.Success)
            {
                Console.WriteLine($"[-] Session Init Failed: {init_res.Message}");
                Thread.Sleep(3000);
                return;
            }

            bool is_vm = qPapelEasyAuth.qPapelEasyAuth.IsVirtualMachine(out int vm_score, out string vm_vendor);

            Console.WriteLine($"[+] Security   : {(qPapelEasyAuth.qPapelEasyAuth.IsDebuggerDetected() ? "FLAGGED" : "CLEAN")}");
            Console.WriteLine($"[+] VM Machine : {(is_vm ? $"DETECTED (VM: {vm_vendor}, Score: {vm_score})" : "PHYSICAL PC")}\n");

            Console.Write("Enter License Key: ");
            string license_key = Console.ReadLine()?.Trim() ?? "";

            Console.WriteLine("[*] Authenticating...");
            var auth_res = qPapelEasyAuth.qPapelEasyAuth.Authenticate(license_key, api_key);

            if (auth_res.Success)
            {
                Console.WriteLine("\n[SUCCESS] Authentication Verified!");
                Console.WriteLine($"[*] Expire Date : {(string.IsNullOrEmpty(auth_res.ExpireDate) ? "Lifetime" : auth_res.ExpireDate)}\n");

                int test_val = CalculateSecurePayload(10, 20);
                Console.WriteLine($"[+] Protected Function Result: {test_val}");
            }
            else
            {
                Console.WriteLine($"\n[-] Auth Failed: {auth_res.Message} (Code: {auth_res.ErrorCode})");
                Thread.Sleep(3000);
                return;
            }

            // Other features reference:

            // Server-side variables
            // string val = qPapelEasyAuth.qPapelEasyAuth.GetVariable("my_variable_access_id", license_key, api_key);
            // var val_ex = qPapelEasyAuth.qPapelEasyAuth.GetVariableEx("my_variable_access_id", license_key, api_key);

            // Server-side file download
            // var file = qPapelEasyAuth.qPapelEasyAuth.GetFile("file_access_id", license_key, api_key);
            // qPapelEasyAuth.qPapelEasyAuth.DownloadFileToDisk("file_access_id", "C:\\output.dll", license_key, api_key);

            // Server-side manual map
            // var map = qPapelEasyAuth.qPapelEasyAuth.ServerMap("file_access_id", "target.exe", license_key, api_key);

            // Protection checks
            // bool tampered = qPapelEasyAuth.qPapelEasyAuth.ProtectionCheck();
            // bool dbg = qPapelEasyAuth.qPapelEasyAuth.IsDebuggerDetected();

            // Reporting
            // qPapelEasyAuth.qPapelEasyAuth.ReportSuspicious("Cheat engine detected", "tamper", "critical");

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}
