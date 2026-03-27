using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyTail
{
    public static class TailApp
    {
        public static int Run(string[] args, TextReader stdin, TextWriter stdout, TextWriter stderr)
        {
            int linesToRead = 10; 
            string? filePath = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-n")
                {
                    if (i + 1 < args.Length && int.TryParse(args[i + 1], out int n) && n >= 0)
                    {
                        linesToRead = n;
                        i++; 
                    }
                    else
                    {
                        stderr.WriteLine("Error: -n option requires a valid positive numeric argument.");
                        return 2; 
                    }
                }
                else
                {
                    filePath = args[i];
                }
            }

            IEnumerable<string> lines;
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    lines = ReadFromStream(stdin);
                }
                else if (File.Exists(filePath))
                {   
                    lines = File.ReadLines(filePath);
                }
                else
                {
                    stderr.WriteLine($"Error: File '{filePath}' not found.");
                    return 1; 
                }

                var lastLines = lines.TakeLast(linesToRead);
                foreach (var line in lastLines)
                {
                    stdout.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                stderr.WriteLine($"Unexpected error: {ex.Message}");
                return 1;
            }

            return 0; 
        }

        private static IEnumerable<string> ReadFromStream(TextReader reader)
        {
            var lines = new List<string>();
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            return lines;
        }
    }
}
