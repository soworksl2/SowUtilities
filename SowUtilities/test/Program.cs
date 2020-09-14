using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SowUtilities.Security;
using System.IO;
using System.Diagnostics;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {

            string x = Cryptography.Encript("Hola", "Hola");

            Console.WriteLine(Cryptography.Decrypt(x, "Hola"));

            Console.ReadKey();

        }
    }
}
