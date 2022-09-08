using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jmeterChanger
{
    class Program
    {

        //ver https://stackoverflow.com/questions/334879/how-do-i-get-the-application-exit-code-from-a-windows-command-line

        private const int C_PATH = 0;
        private const int C_THREADS = 1;
        private const int C_SECONDS = 2;

        private const string C_STRFIND_NUM_THREADS = "<stringProp name=\"ThreadGroup.num_threads\">";
        private const string C_STRFIND_NUM_RAMPTIME = "<stringProp name=\"ThreadGroup.ramp_time\">";
        private const string C_STRFIND_NUM_THREADGROUPTIME = "<stringProp name=\"ThreadGroup.duration\">";
        private const string C_STRFIN_BOL_THREADGROUP = "<boolProp name=\"ThreadGroup.scheduler\">";
        

        private const string C_NUM_ENDTAG = "</stringProp>";
        private const string C_BOL_ENDTAG = "</boolProp>";

        static string[] jmeterFile;

        static void Main(string[] args)
        {
            //args 0 path
            //args 1 threads
            //args 2 segundos

            if (args.Count() != 3)
            {
                Console.WriteLine("jmeterChanger path threads seconds");
                Console.WriteLine("jemeterChanger \"c:\\test folder\\jmeter.jmx\" 200 3600");
                System.Environment.Exit(1);
            }

            try
            {
                if (File.Exists(args[C_PATH]))
                {
                    ReadFile(args);
                    WriteFile(args[C_PATH]);
                    System.Environment.Exit(0);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                System.Environment.Exit(1);
            }
        }

        static void ReadFile(string[] args)
        {
            string path = args[C_PATH];
            jmeterFile = File.ReadAllLines(path);

            for (int i =0;i<jmeterFile.Length;i++)
            {
                if (jmeterFile[i].Contains(C_STRFIND_NUM_THREADS))
                {
                    LineChanger(C_STRFIND_NUM_THREADS + args[C_THREADS] + C_NUM_ENDTAG, i);
                }

                if (jmeterFile[i].Contains(C_STRFIND_NUM_THREADGROUPTIME))
                {
                    LineChanger(C_STRFIND_NUM_THREADGROUPTIME + args[C_SECONDS] + C_NUM_ENDTAG, i);  
                }

                if (jmeterFile[i].Contains(C_STRFIN_BOL_THREADGROUP))
                {
                    LineChanger(C_STRFIN_BOL_THREADGROUP + "true" + C_BOL_ENDTAG, i);
                }
            }

        }

        static void LineChanger(string newText,int lineIndex)
        {
            //Console.WriteLine(newText);
            jmeterFile[lineIndex] = newText;
        }

        static void WriteFile(string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath);
            for (int i=0;i<jmeterFile.Length;i++)
            {
                sw.WriteLine(jmeterFile[i]);
            }
            
            sw.Flush();
            sw.Close();
        }
    }
}
