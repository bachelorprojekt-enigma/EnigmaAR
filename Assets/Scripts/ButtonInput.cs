using TMPro;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonInput : MonoBehaviour
{
    
    
    [SerializeField] public TextMeshProUGUI m_TMPText; //m_ = member variable. to avoid name conflicts with variable
    bool toggle = false;    // Transparent Toggle
    
    
    void Start()
    {
        
    }
     
    public void AddLetter(string letter)
    {
        m_TMPText.text += letter;
        Debug.Log("AddLetter: " + letter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LightOn()
    {
        Material glow = GetComponent<Renderer>().material;  // a new instance of the material
        glow.EnableKeyword("_EMISSION");
    }

    public void LightOff()
    {
        Material glow = GetComponent<Renderer>().material;
        glow.DisableKeyword("_EMISSION");
    }

    public void DelayedLightOff()
    {
        Invoke(nameof(LightOff), 0.5f); // LightOff() delayed call
    }


    public void ButtonPressed()
    {
        Debug.Log("Button Pressed");
    }
    
    
    public void TransparentToggle()
    {
        Material transparency = GetComponent<Renderer>().material;  // a new instance of the material

        if (!toggle)
        {
            transparency.SetFloat("_Mode", 3); // For Standard Shader, 3 = Transparent mode
            transparency.color = new Color(transparency.color.r, transparency.color.g, transparency.color.b, 0.5f); // Semi-transparent
            transparency.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            transparency.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            transparency.SetInt("_ZWrite", 0);
            transparency.DisableKeyword("_ALPHATEST_ON");
            transparency.EnableKeyword("_ALPHABLEND_ON");
            transparency.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparency.renderQueue = 3000;
            
            toggle = true;
        }
        
        else
        {
            transparency.SetFloat("_Mode", 0); // For Standard Shader, 0 = Opaque mode
            transparency.color = new Color(transparency.color.r, transparency.color.g, transparency.color.b, 1f); // Fully opaque
            transparency.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            transparency.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            transparency.SetInt("_ZWrite", 1);
            transparency.EnableKeyword("_ALPHATEST_ON");
            transparency.DisableKeyword("_ALPHABLEND_ON");
            transparency.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            transparency.renderQueue = -1;
            
            toggle = false;
        }
    }
}
