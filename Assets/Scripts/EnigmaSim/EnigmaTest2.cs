using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Enigma_Test_1;
using UnityEngine;
namespace Enigma_Test_1

{
    public class EnigmaTest2
    {
        //Walzen sind für ENIGMA I (WALZEN = I, II, III, IV, V)
        private readonly string[] SRotors = 
        {
            "EKMFLGDQVZNTOWYHXUSPAIBRCJ",       //Walze I   | Übertragungskerbe = Q
            "AJDKSIRUXBLHWTMCQGZNPYFVOE",       //Walze II  | Übertragungskerbe = E
            "BDFHJLCPRTXVZNYEIWGAKMUSQO",       //Walze III | Übertragungskerbe = V
            "ESOVPZJAYQUIRHXLNFTGKDCMWB",       //Walze IV  | Übertragungskerbe = J
            "VZBRGITYUPSDNHLXAWMJQOFECK"        //Walze V   | Übertragungskerbe = Z
        };

        private readonly string[] UKWs = 
        {
            "EJMZALYXVBWFCRQUONTSPIKHGD",       //UKW A
            "YRUHQSLDPXNGOKMIEBFZCWVJAT",       //UKW B
            "FVPJIAOYEDRZXWGCTKUQSBNMHL"        //UKW C
        };

        //Translation notches
        private readonly char[] transNotches = { 'Q', 'E', 'V', 'J', 'Z' };
        
        private Rotor RotorOne;
        private Rotor RotorTwo;
        private Rotor RotorThree;
        private Rotor UKW;
        private Rotor[] Rotors;
        private Rotor[] RotorsPreList;
        private Rotor[] UKWPreList;

        private Plugboard plugboard;

        public EnigmaTest2()
        {
            PreloadRotors();
        }

        public void PreloadRotors()
        {
            RotorsPreList = new Rotor[SRotors.Length];
            UKWPreList = new Rotor[UKWs.Length];
            for (int i = 0; i < SRotors.Length; i++) 
            {
                RotorsPreList[i] = new Rotor(SRotors[i], transNotches[i], 0);

                if (i < UKWs.Length)
                {
                    UKWPreList[i] = new Rotor(UKWs[i], '?', 0);
                }
            }
        }

        public void LoadStandardSettings()
        {
            ResetRotors();
            plugboard = new Plugboard(" ");
            RotorOne = RotorsPreList[0];;
            RotorTwo = RotorsPreList[1];
            RotorThree = RotorsPreList[2];
            UKW = UKWPreList[0];
            Rotors = new Rotor[] {RotorOne, RotorTwo, RotorThree};
        }

        public void ResetRotors()
        {
            foreach(Rotor rotor in RotorsPreList)
            {
                rotor.Counter = 0;
            }
        }

        public int[] getRotorPositions()
        {
            int[] positions = new int[3];
            for (int i = 0; i < Rotors.Length; i++) 
            {
                positions[i] = Rotors[i].Counter;
            }
            return positions;
        }

        public void ChooseRotor(int rotor,int n, string type)
        {
            if (type == "UKW")
            {
                UKW = UKWPreList[rotor-1];
            }

            if (type == "ROTOR")
            {
                if (n == 0)
                {
                    RotorOne = new Rotor(RotorsPreList[rotor-1]);
                } 
                else if (n == 1)
                {
                    RotorTwo = new Rotor(RotorsPreList[rotor-1]);
                } 
                else if (n == 2)
                {
                    RotorThree = new Rotor(RotorsPreList[rotor-1]);
                }
            }
            Rotors = new Rotor[] {RotorOne, RotorTwo, RotorThree};
        }

        public void CreatePlugboard(string plugs)
        {
            plugboard = new Plugboard(plugs);
        }

        public void ChooseRotorPosition(int n, int pos)
        {
            if (pos == 26) pos = 0;
            if (n == 0)
            {
                RotorOne.Counter = pos;
            }
            else if (n == 1)
            {
                RotorTwo.Counter = pos;
            }
            else
            {
                RotorThree.Counter = pos;
            }
        }

        private void RotateRotor()
        {
            //Special case where Rotor2 rotates back to back (happens when Rotor2 reaches its transNotch)
            //Logic is bad, but it works
            if (RotorOne.TransNotch(1) && RotorTwo.TransNotch(0))
            {
                RotorOne.turnRotor();
                RotorTwo.turnRotor();
                RotorThree.turnRotor();
            }
            //Normal Rotor rotation
            else {
                RotorOne.turnRotor();
                
                if (RotorOne.TransNotch(1))
                {
                    RotorTwo.turnRotor();

                    if (RotorTwo.TransNotch(1))
                    {
                        RotorThree.turnRotor();
                    }
                }
            }
        }

        public void RotateRotorBack()
        {
            //Special case where Rotor2 rotates back to back (happens when Rotor2 reaches its transNotch)
            if (RotorOne.TransNotch2() && RotorTwo.TransNotch(1))
            {
                RotorOne.turnRotorBack();
                RotorTwo.turnRotorBack();
                RotorThree.turnRotorBack();
            }
            //Normal Rotor rotation
            else 
            {
                RotorOne.turnRotorBack();
                
                if (RotorOne.TransNotchBack())
                {
                    RotorTwo.turnRotorBack();
                    
                    if (RotorTwo.TransNotchBack())
                    {
                        RotorThree.turnRotorBack();
                    }
                }
            }
        }

        public char SwitchChar2(char input)
        {
            char scrambled = plugboard.Swap(char.ToUpper(input));
            if (scrambled > 90 || scrambled < 65)
            {
                return '?';
            }
            RotateRotor();
            scrambled  = RotorOne.SwitchedChar((char)((scrambled - 'A' + RotorOne.Counter) % 26 + 'A'));
            scrambled = RotorTwo.SwitchedChar((char)((scrambled - 'A' - RotorOne.Counter + RotorTwo.Counter) %26 + 'A'));
            scrambled = RotorThree.SwitchedChar((char)((scrambled - 'A' - RotorTwo.Counter + RotorThree.Counter) % 26 + 'A'));
            scrambled = UKW.SwitchedChar((char)((scrambled - 'A' - RotorThree.Counter) % 26 + 'A'));
            scrambled = RotorThree.SwitchedCharBack((char)((scrambled - 'A' + RotorThree.Counter) % 26 + 'A'));
            scrambled = RotorTwo.SwitchedCharBack((char)((scrambled - 'A' + RotorTwo.Counter - RotorThree.Counter) % 26 + 'A'));
            scrambled = RotorOne.SwitchedCharBack((char)((scrambled - 'A' + RotorOne.Counter - RotorTwo.Counter) % 26 + 'A'));
            if ((scrambled - 'A' - RotorOne.Counter) % 26 + 'A' < 65)
            {
                return plugboard.Swap((char)((scrambled - 'A' - RotorOne.Counter) % 26 + 'A'+ 26));
            } 
            return plugboard.Swap((char)((scrambled - 'A' - RotorOne.Counter) % 26 + 'A'));
        }
    }
}