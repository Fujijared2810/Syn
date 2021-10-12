using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Synapse_X_Remake.Static;


namespace Synapse_X_Remake.Interface
{
    public static class DataInterface
    {
        public static void Save<T>(string Name, T Data)
        {
            var Serial = JsonConvert.SerializeObject(Data);
            var Protected = ProtectedData.Protect(Encoding.UTF8.GetBytes(Serial),
                Encoding.UTF8.GetBytes(Utils.Sha512(Environment.MachineName + Name)), DataProtectionScope.LocalMachine);
            File.WriteAllText("bin\\" + Name + ".bin", Convert.ToBase64String(Protected));
        }

        public static T Read<T>(string Name)
        {
            var Unprotected = ProtectedData.Unprotect(Convert.FromBase64String(File.ReadAllText("bin\\" + Name + ".bin")),
                Encoding.UTF8.GetBytes(Utils.Sha512(Environment.MachineName + Name)), DataProtectionScope.LocalMachine);
            return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(Unprotected));
        }

        public static bool Exists(string Name)
        {
            return File.Exists("bin\\" + Name + ".bin");
        }

        public static void Delete(string Name)
        {
            if (File.Exists("bin\\" + Name + ".bin")) File.Delete("bin\\" + Name + ".bin");
        }
    }
}
