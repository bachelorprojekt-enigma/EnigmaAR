using UnityEngine.UI;

namespace Enigma_Test_1
{
    public class ButtonPair
    {
        public Button Button1 { get; }
        public Button Button2 { get; }

        public ButtonPair(Button but1, Button but2)
        {
            Button1 = but1;
            Button2 = but2;
        }

        //Checks if a button is already Used
        public bool ContainsButton(Button button)
        {
            return (button == Button1 || button == Button2);
        }

        //Returns the not clicked Button to be paired to another
        public Button GetOtherButton(Button button)
        {
            if(Button1 != null && Button2 != null)
            {
                if (button == Button1)
                {
                    return Button2;
                }

                return Button1;
            }

            return null;
        }
    }
}