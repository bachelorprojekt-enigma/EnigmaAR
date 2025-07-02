using System.Collections.Generic;
using UnityEngine;

public class KeyboardVisualizer : MonoBehaviour
{
    public Transform keysParent; // Parent-Objekt mit allen Tasten (Housing)
    private Dictionary<char, Transform> keyMap = new Dictionary<char, Transform>();

    void Start()
    {
        // Durchläuft alle Kinder des Tasten-Parents und speichert sie
        foreach (Transform key in keysParent)
        {
            string name = key.name.ToLower().Replace("key_", "");
            if (name.Length == 1)
            {
                char c = name[0];
                keyMap[c] = key;
            }
        }
    }

    public void PressKey(char c)
    {
        c = char.ToLower(c);
        if (keyMap.TryGetValue(c, out Transform keyTransform))
        {
            StartCoroutine(AnimateKeyPress(keyTransform));
        }
    }

    private System.Collections.IEnumerator AnimateKeyPress(Transform key)
    {
        Vector3 originalPos = key.localPosition;
        Vector3 pressedPos = originalPos + Vector3.down * 0.02f;

        float duration = 0.1f;
        float elapsed = 0f;

        // Runter bewegen
        while (elapsed < duration)
        {
            key.localPosition = Vector3.Lerp(originalPos, pressedPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        //kurz warten
        key.localPosition = pressedPos;
        yield return new WaitForSeconds(0.05f);

        // Taste wieder hoch bewegen
        elapsed = 0f;
        while (elapsed < duration)
        {
            key.localPosition = Vector3.Lerp(pressedPos, originalPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        key.localPosition = originalPos;
    }
}