using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Experiment
{
    public class Program
    {
        private static List<Experiment> Experiments = new List<Experiment>
        {
            //new ConvertInputAndOutputDataExperimental(),
            new UnsafeReferenceMatrix(),
        };

        public static void Main(string[] args)
        {
            foreach (var item in Experiments)
            {
                item.Run();
            }
            Console.ReadKey();
        }
    }
}
