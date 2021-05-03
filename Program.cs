using IPFixCollector.NetflowCollection;
using System;

namespace IPFixCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            new NetflowWorker().Start();
        }
    }
}
