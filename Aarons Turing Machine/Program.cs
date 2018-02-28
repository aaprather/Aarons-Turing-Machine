using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aarons_Turing_Machine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a word - ");
            string WORD = Console.ReadLine();

            Console.Write("\nEnter file path - ");
            string FILEPATH = Console.ReadLine();
            Console.WriteLine();



            /*Grab Config*/
            string line2 = "";
            Initialization config2 = new Initialization();
            try
            {
                using (StreamReader sr = new StreamReader(FILEPATH))
                {
                    while (!sr.EndOfStream)
                    {
                        string[] channels = line2.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        Initialization config = new Initialization();
                        if (sr.Peek() != '{')
                            sr.ReadLine();
                        else
                        {
                            config.states = sr.ReadLine();
                            config.start = sr.ReadLine();
                            config.accept = sr.ReadLine();
                            config.reject = sr.ReadLine();
                            config.alpha = sr.ReadLine();
                            config.tapeAlpha = sr.ReadLine();
                            config2 = config;
                        }
                    }
                    
                }
            }
            catch
            {
                
                   
            }
            



            /*Grab Algorithm*/
            List<Instruction> algorithm = new List<Instruction>();
            string line = "";
            try
            {
                using (StreamReader sr = new StreamReader(FILEPATH))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] channels = line.Split(' ');

                        if (channels[0] == "rwRt" || channels[0] == "rwLt") //4 letter instruction, transition
                            algorithm.Add(new Instruction() { instruct = channels[0], currentState = channels[1], readingString = channels[2], writingString = channels[3], transitionState = channels[4] });
                        else if (channels[0] == "rRt" || channels[0] == "rLt") //3 letter instruction, transition
                            algorithm.Add(new Instruction() { instruct = channels[0], currentState = channels[1], readingString = channels[2], writingString = channels[2], transitionState = channels[3] });
                        else if (channels[0] == "rRl" || channels[0] == "rLl") //3 letter instruction, loop
                            algorithm.Add(new Instruction() { instruct = channels[0], currentState = channels[1], readingString = channels[2], writingString = channels[2], transitionState = channels[1] });
                    }
                }
            }
            catch
            {

            }
            

            foreach(char c in WORD)
            {
                if(config2.alpha == null)
                {
                    Console.WriteLine("Invalid file format. Please restart the program and try again");
                    waitForRestart();
                }
                if (!config2.alpha.Contains(c))
                {
                    Console.WriteLine("Word contains characters that do not exist in the provided input alphabet. Please restart the program and try again");
                    waitForRestart();
                }
                if (config2.alpha.Contains("_"))
                {
                    Console.WriteLine("Input alphabet contains illegal character '_'. Please restart the program and try again");
                    waitForRestart();
                }
                if(!config2.states.Contains(config2.start))
                {
                    Console.WriteLine("Start state does not exist in the given set of states. Please restart the program and try again");
                    waitForRestart();
                }
                if (!config2.states.Contains(config2.accept))
                {
                    Console.WriteLine("Accept state does not exist in the given set of states. Please restart the program and try again");
                    waitForRestart();
                }
                if (!config2.states.Contains(config2.reject))
                {
                    Console.WriteLine("Reject state does not exist in the given set of states. Please restart the program and try again");
                    waitForRestart();
                }
            }
            foreach (char c in config2.alpha)
            {
                if (!config2.tapeAlpha.Contains(c))
                {
                    Console.WriteLine("Alphabet character(s) missing from tape alphabet. Please restart the program and try again");
                    waitForRestart();
                }
            }
            Console.WriteLine("[" + config2.start + "]" + WORD + "_");
            WORD = config2.start + WORD + "_";







            bool end = false;
            while (end != true) 
            {

                foreach (Instruction i in algorithm)
                {
                    if (WORD.Contains(i.currentState + i.readingString))
                    {
                        if (i.instruct == "rwRt")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.writingString + "[" + i.transitionState + "]");
                            WORD = WORD.Insert(index, i.writingString + i.transitionState);
                            Console.WriteLine(toOutput);


                        }
                        else if (i.instruct == "rwLt")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.writingString);
                            toOutput = toOutput.Insert(index - 1, "[" + i.transitionState + "]");
                            WORD = WORD.Insert(index, i.writingString);
                            WORD = WORD.Insert(index - 1, i.transitionState);
                            Console.WriteLine(toOutput);

                        }
                        else if (i.instruct == "rRl")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.readingString + "[" + i.currentState + "]");
                            WORD = WORD.Insert(index, i.readingString + i.currentState);
                            Console.WriteLine(toOutput);

                        }
                        else if (i.instruct == "rLl")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.readingString);
                            toOutput = toOutput.Insert(index - 1, "[" + i.currentState + "]");
                            WORD = WORD.Insert(index, i.readingString);
                            WORD = WORD.Insert(index - 1, i.currentState);
                            Console.WriteLine(toOutput);

                        }
                        else if (i.instruct == "rRt")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.readingString + "[" + i.transitionState + "]");
                            WORD = WORD.Insert(index, i.readingString + i.transitionState);
                            Console.WriteLine(toOutput);

                        }
                        else if (i.instruct == "rLt")
                        {
                            var index = WORD.IndexOf(i.currentState + i.readingString);
                            string toOutput = WORD;
                            WORD = WORD.Replace(i.currentState + i.readingString, "");
                            toOutput = WORD.Insert(index, i.readingString);
                            toOutput = toOutput.Insert(index - 1, "[" + i.transitionState + "]");
                            WORD = WORD.Insert(index, i.readingString);
                            WORD = WORD.Insert(index - 1, i.transitionState);
                            Console.WriteLine(toOutput);


                        }
                        if (config2.accept.Contains(i.transitionState))
                        {
                            Console.WriteLine("ACCEPT!");
                            end = true;
                        }
                        if (config2.reject.Contains(i.transitionState))
                        {
                            Console.WriteLine("REJECT!");
                            end = true;
                        }

                    }
                }
            }



            Console.ReadKey();
        }
        public static void waitForRestart()
        {
            while(true)
            {

            }
        }
    }


    class Initialization
    {
        private string STATES;
        public string states
        {
            get { return STATES; }
            set
            {
                string test = value.Replace("{states: ", string.Empty);
                STATES = test.Replace("}", string.Empty);
            }
        }

        private string START;
        public string start
        {
            get { return START; }
            set
            {
                string test = value.Replace("{start: ", string.Empty);
                START = test.Replace("}", string.Empty);
            }
        }

        private string ACCEPT;
        public string accept
        {
            get { return ACCEPT; }
            set
            {
                string test = value.Replace("{accept: ", string.Empty);
                ACCEPT = test.Replace("}", string.Empty);
            }
        }

        private string REJECT;
        public string reject
        {
            get { return REJECT; }
            set
            {
                string test = value.Replace("{reject: ", string.Empty);
                REJECT = test.Replace("}", string.Empty);
            }
        }

        private string ALPHA;
        public string alpha
        {
            get { return ALPHA; }
            set
            {
                string test = value.Replace("{alpha: ", string.Empty);
                ALPHA = test.Replace("}", string.Empty);
            }
        }

        private string TAPEALPHA;
        public string tapeAlpha
        {
            get { return TAPEALPHA; }
            set
            {
                string test = value.Replace("{tape-alpha: ", string.Empty);
                TAPEALPHA = test.Replace("}", string.Empty);
            }
        }

        public Initialization() { }

    }


    class Instruction
    {
        private string INSTRUCT;
        public string instruct
        {
            get
            {
                return INSTRUCT;
            }
            set
            {
                INSTRUCT = value.Replace(";", string.Empty);
            }
        }

        private string CURRENTSTATE;
        public string currentState
        {
            get
            {
                return CURRENTSTATE;
            }
            set
            {
                CURRENTSTATE = value.Replace(";", string.Empty);
            }
        }

        private string READINGSTRING;
        public string readingString
        {
            get
            {
                return READINGSTRING;
            }
            set
            {
                READINGSTRING = value.Replace(";", string.Empty);
            }
        }

        private string WRITINGSTRING;
        public string writingString
        {
            get
            {
                return WRITINGSTRING;
            }
            set
            {
                WRITINGSTRING = value.Replace(";", string.Empty);
            }
        }
        private string TRANSITIONSTATE;
        public string transitionState
        {
            get
            {
                return TRANSITIONSTATE;
            }
            set
            {
                TRANSITIONSTATE = value.Replace(";", string.Empty);

            }
        }
    }


}
