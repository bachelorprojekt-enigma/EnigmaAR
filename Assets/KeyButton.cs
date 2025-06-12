using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Collections; 

public class KeyButton : MonoBehaviour
{
    
    public string keyLetter;

    private Vector3 originalePosition;

    public float druckTiefe = 0.002f;

    public float druckDauer;

    public AudioSource buttonSound;

    public AudioClip keyPressSound;

    
    public TextMeshProUGUI outputText;

    private void Start()
    {

        originalePosition = transform.localPosition;

        
        if (outputText == null)
        {
            outputText = GameObject.Find("OutputText").GetComponent<TextMeshProUGUI>(); 
        }

        if (buttonSound == null) {

            buttonSound = gameObject.AddComponent<AudioSource>();
        }

        if (keyPressSound != null) {
            buttonSound.clip = keyPressSound;
        }
    }

    
    private void OnMouseDown()
    {
        // Wenn die Taste Backspace ist, lösche den letzten Buchstaben
        if (keyLetter == "Backspace")
        {
            if (outputText.text.Length > 0)
            {
                outputText.text = outputText.text.Substring(0, outputText.text.Length - 1);
            }
        }
        // Wenn es eine normale Taste ist, füge den Buchstaben hinzu
        else
        {
            outputText.text += keyLetter;
            
            
        }

        StartCoroutine(BewegeTaste());

        PlaySound();
    }

    private IEnumerator BewegeTaste() {

        transform.localPosition = new Vector3(originalePosition.x, originalePosition.y - druckTiefe, originalePosition.z);

        yield return new WaitForSeconds(druckDauer);

        transform.localPosition = originalePosition;

    }
    
    private void PlaySound() {

        if (buttonSound != null && keyPressSound != null) {

            buttonSound.Play();
        }
    }

}