using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace codesec
{
    class Program
    {
        static Codesec codesec;
        static void Main(string[] args)
        {
            codesec = new Codesec();
            System.Threading.Thread.Sleep(100000);
        }
    }
}
