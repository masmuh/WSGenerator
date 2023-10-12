using System;
using System.Linq;
using System.Threading.Tasks;

namespace WSGenerator
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"total args: {args.Count()}");
  
            await JobRunner.Execute(args);
        }
    }
}
