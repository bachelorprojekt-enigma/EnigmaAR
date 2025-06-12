using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Enigma_Test_1;

//!!!!!!!!!!DIESER MANAGER WAR NUR FÜR 2D Buttons GEDACHT!!!!!!!!!!!!!!!

public class PlugboardManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContainer;

    private Button firstButton = null;
    private List<ButtonPair> combinedButtons = new List<ButtonPair>();
    private char firstChar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private List<Button> buttons = new List<Button>();
    public string combinedChars {get; set; }
    public event Action<string> OnCombinedCharsUpdated; 
    

    void Start()
    {
        // Add GridLayoutGroup component to the buttonContainer
        GridLayoutGroup gridLayoutGroup = buttonContainer.gameObject.AddComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(50, 50); // Set the cell size
        gridLayoutGroup.spacing = new Vector2(10, 10); // Set the spacing between cells
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = 6; // Set the number of columns
        gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        
        CreateButtons();
    }

    private void CreateButtons()
    {
        for (int i = 0; i < 26; i++)
        {
            GameObject buttonObject = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonObject.GetComponent<Button>();
            button.GetComponentInChildren<Text>().text = ((char)('A' + i)).ToString(); // Set button text to A, B, C, etc.
            button.onClick.AddListener(() => OnButtonDown(button)); // Add click listener
            buttonObject.transform.localPosition = new Vector3(30*i, -30 * i, 0); // Adjust position if needed
            buttons.Add(button);
        }
    }
    private bool ButtonInUse (Button button)
    {
        if (combinedButtons == null) return false;
        foreach (var pair in combinedButtons)
        {
            if (pair.ContainsButton(button))
            {
                return true;
            }
        }
        
        return false;
    }

    private ButtonPair GetUsedButtonPair(Button button)
    {
        if (combinedButtons == null) return null;
        foreach (var pair in combinedButtons)
        {
            if (pair.ContainsButton(button)) return pair;
        }

        return null;
    }

    public void ResetPlugboard()
    {
        foreach (var button in buttons)
        {
            button.GetComponent<Image>().color = Color.white;
        }

        combinedButtons = new List<ButtonPair>();
        firstButton = null;
        combinedChars = "";
    }

    private void WriteCombinedChars()
    {
        combinedChars = "";
        if (combinedButtons == null) return;
        foreach (var pair in combinedButtons)
        {
            combinedChars += $"{pair.Button1.GetComponentInChildren<Text>().text}{pair.Button2.GetComponentInChildren<Text>().text},";
        }
        OnCombinedCharsUpdated?.Invoke(combinedChars);
    }
    
    public void OnButtonDown(Button button)
    {
        if (firstButton == null && !ButtonInUse(button))
        {
            firstButton = button;
            button.GetComponent<Image>().color = Color.red;
        }
        else
        {
            if (ButtonInUse(button))
            {
                ButtonPair pair = GetUsedButtonPair(button);
                Button but1 = pair.Button1;
                Button but2 = pair.Button2;
                but1.GetComponent<Image>().color = Color.white;
                but2.GetComponent<Image>().color = Color.white;
                combinedButtons.Remove(pair);
                WriteCombinedChars();
            }
            else if (button != firstButton)
            {
                button.GetComponent<Image>().color = Color.red;
                combinedButtons.Add(new ButtonPair(firstButton, button));
                WriteCombinedChars();
            }
            else
            {
                button.GetComponent<Image>().color = Color.white;
            }
            firstButton = null;
            
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
