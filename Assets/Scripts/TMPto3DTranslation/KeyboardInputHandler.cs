using UnityEngine;
using TMPro;

public class KeyboardInputHandler : MonoBehaviour
{
    // script haengt an einem InuptHandler im Prefab 
    // und referenziert das inputfeld und die enigma
    public TMP_InputField inputField;
    public KeyboardVisualizer keyboardVisualizer;

    public AudioSource audio;

    void Start()
    {
        //wird aktiviert, wenn sich im Inputfeld was verändert
        inputField.onValueChanged.AddListener(HandleInput);
    }

    void HandleInput(string input)
    {
        if (input.Length > 0)
        {
            // Nimmt das zuletzt eingegebene Zeichen und übergibt es an den Keybirad Visualizer
            char lastChar = input[input.Length - 1];
            keyboardVisualizer.PressKey(lastChar);
            
            audio.Play();
            
        }
    }
}
