using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.mkysoft.nChiper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var encryption = new Encryption("OCSP PIN", "PUBLIC KEY NAME");
                string text = "test verisi";
                var encrypted = encryption.Encrypt(Encoding.UTF8.GetBytes(text));
                Console.WriteLine(BitConverter.ToString(encrypted));
                Console.WriteLine("Encryption complated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
