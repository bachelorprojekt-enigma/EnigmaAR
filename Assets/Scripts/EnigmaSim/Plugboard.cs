using System.Collections.Generic;

namespace Enigma_Test_1
{
    public class Plugboard
    {
        private Dictionary<char, char> plugConnections = new Dictionary<char, char>();

        public Plugboard(string connections)
        {
            foreach (string pair in connections.Split(','))
            {
                if (pair.Length == 2)
                {
                    char first = pair[0];
                    char second = pair[1];
                    plugConnections[first] = second;
                    plugConnections[second] = first;
                }
            }
        }

        public char Swap(char input)
        {
            return plugConnections.ContainsKey(input) ? plugConnections[input] : input;
        }
    }
}