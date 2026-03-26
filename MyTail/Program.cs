using System;

namespace MyTail
{
    public class Program
    {
        public static int Main(string[] args)
        {
           
            return TailApp.Run(args, Console.In, Console.Out, Console.Error);
        }
    }
}

