using System;
namespace Enigma_Test_1
{
    public class Rotor
    {
        private char[] Alphabet =   "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private char translationNotch;
        private char[] SwitchedAlpha;
        private int counter;

        public Rotor(string SwitchedAlpha, char translationNotch, int position)
        {
            this.translationNotch = translationNotch;
            this.SwitchedAlpha = SwitchedAlpha.ToCharArray();
            counter = position;
        }

        public Rotor(Rotor rotor)
        {
            this.translationNotch = rotor.translationNotch;
            this.SwitchedAlpha = rotor.SwitchedAlpha;
            counter = rotor.Counter;
        }

        public int Counter
        {
            get{ return counter; }
            set { counter = value; }
        }
        
        public void turnRotor()
        {
            counter = (counter + 1) % 26;
            //Console.WriteLine("Counter = " + counter);
        }

        public void turnRotorBack()
        {
            counter = (counter-1) < 0 ? 25 : counter-1;
            //Console.WriteLine("Counter = " + counter);
        }
        //Gets the Index of the Letter in the Alphabet
        public int GetIndexOfLetter(char input)
        {
            if (input < 65)
            {
                input = (char)(input + 26);
            } else if (input > 90)
            {
                input = (char)(input - 26);
            }
            return Array.IndexOf(Alphabet, input);
        }
        
        //Gets the Index of the Letter on the Rotor (Needed for SwitchedCharBack)
        public int GetIndexOfRotor(char input)  //Benötigt für den Rückweg
        {
            return Array.IndexOf(SwitchedAlpha, input);
        }

        public char SwitchedChar(char input)
        {
            return SwitchedAlpha[GetIndexOfLetter(input)];
        }
        
        public char SwitchedCharBack(char input)
        {
            if (input < 65) input = (char) (input +26);
            int index = Array.IndexOf(SwitchedAlpha, input);
            Console.WriteLine("index = " + index + " input = " + input);
            if (index < 0)
            {
                index += 26;
            }
            else if (index >= SwitchedAlpha.Length)
            {
                index -= 26;
            }
            return (char)(Alphabet[index]);
        }
        
        //Translation notch check, (if transNotch == Q rotate rotor2 on Q -> R)
        public bool TransNotch(int varSum) //varSum [0,1,2] 0 = on backwards, 1 = normal forwards, 2 = extra condition 
        {
            return Alphabet[counter] == translationNotch+varSum;
        }

        //Translation notch check for special case where R2 rotates back to back
        public bool TransNotch2()
        {
            return Alphabet[counter] == translationNotch+2;
        }

        //Translation notch check for reversing text input
        public bool TransNotchBack()
        {
            return Alphabet[counter] == translationNotch;
        }
    }
}