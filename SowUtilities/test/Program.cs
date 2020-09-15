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

            Stopwatch x = new Stopwatch();

            x.Start();

            Cryptography.DecryptFile(@"C:\Users\jimyw\Desktop\nueva carpeta xd\Prueba encriptado.txt");

            x.Stop();

            Console.WriteLine("Archivo encriptado en : {0} milisegundos", x.ElapsedMilliseconds);

            Console.ReadKey();

        }
    }
}
