using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LondonTube
{
    class Program
    {
        static void Main(string[] args)
        {
            // parameters

            string home = "East Ham";
            int stops = 5;

            Dictionary<string, List<string>> links = new Dictionary<string, List<string>>();
            string path = @"C:\Users\ron2\Downloads\london tube lines.csv";
            bool first = true;
            using (FileStream fs = File.OpenRead(path))
            {
                using (var reader = new StreamReader(fs))
                {
                    while (!reader.EndOfStream)
                    {
                        // skip first record

                        if (first)
                        {
                            first = false;
                            continue;
                        }
                        var line = reader.ReadLine();
                        string[] tokens = line.Split(',');
                        string p1 = tokens[1];
                        string p2 = tokens[2];
                        if (links.ContainsKey(p1))
                        {
                            var destinations = links[p1];
                            destinations.Add(p2);
                        }
                        else
                        {
                            List<string> destinations = new List<string>();
                            destinations.Add(p2);
                            links.Add(p1, destinations);
                        }
                        if (links.ContainsKey(p2))
                        {
                            var destinations = links[p2];
                            destinations.Add(p1);
                        }
                        else
                        {
                            List<string> destinations = new List<string>();
                            destinations.Add(p1);
                            links.Add(p2, destinations);
                        }
                    }
                }
            }

            // routed station dictionary

            Dictionary<string, int> stations = new Dictionary<string, int>();
            stations.Add(home, 0);

            // main loop

            for (int i = 0; i < stops; i++)
            {
                foreach (string which in stations.Where(xx => xx.Value == i).Select(yy => yy.Key).ToList())
                {
                    foreach (string destination in links[which])
                    {
                        if (stations.ContainsKey(destination))
                        {
                            continue;
                        }
                        stations.Add(destination, i + 1);
                    }
                }
            }
            foreach (var kvp in stations.Where(xx => xx.Value == stops).OrderBy(xx => xx.Key))
            {
                Console.WriteLine(kvp.Key);
            }
            Console.ReadKey();
        }
    }
}
