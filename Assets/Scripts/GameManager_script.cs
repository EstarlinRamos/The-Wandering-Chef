using UnityEngine;
using UnityEngine.UI; 
using TMPro;          
using Ink.Runtime;    

public class GameManager_script : MonoBehaviour
{
    [Header("Ink Files")]
    [SerializeField]
    private TextAsset inkJSONAsset; 

    [Header("UI Components")]
    [SerializeField]
    private TextMeshProUGUI storyText; 
    
  [SerializeField]
private GameObject choiceButtonPrefab; // This accepts ANY prefab

    [SerializeField]
    private Transform choiceContainer; 
    
    // --- NEW: Slot for the Continue Button ---
    [SerializeField]
    private Button continueButton; 

    private Story story;

    void Start()
    {
        if (inkJSONAsset == null) 
        {
            Debug.LogError("Ink JSON Asset is missing!");
            return;
        }

        story = new Story(inkJSONAsset.text);

        // --- NEW: Make the Continue button call the RefreshUI function ---
        continueButton.onClick.AddListener(RefreshUI);

        // Start the story
        RefreshUI();
    }

    void RefreshUI()
    {
        // 1. HIDE OPTIONS INITIALLY
        // We assume we are reading text, so hide choices and the continue button for a split second.
        choiceContainer.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        // 2. CHECK: IS THERE TEXT TO READ?
        if (story.canContinue)
        {
            // Read ONE line of text (instead of the 'while' loop)
            string text = story.Continue();
            
            // Append it to the log (so we can use the Scroll View)
            // "\n" adds a new line so it doesn't bunch up.
            storyText.text += text + "\n"; 

            // SHOW THE CONTINUE BUTTON
            // So the player can click to get the next line.
            continueButton.gameObject.SetActive(true);
        }
        // 3. CHECK: ARE THERE CHOICES?
        else if (story.currentChoices.Count > 0)
        {
            // If no more text, but we have choices, show them!
            choiceContainer.gameObject.SetActive(true);
            RefreshChoices();
        }
        else 
        {
            // If no text AND no choices, the story is over.
            Debug.Log("End of Story");
        }
    }

   void RefreshChoices()
    {
        // 1. Clear existing buttons
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (Choice choice in story.currentChoices)
        {
            // --- FIX: Remove ".gameObject" because it is ALREADY a GameObject ---
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
            
            // Get the button component from the new object
            Button button = buttonObj.GetComponent<Button>();

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;

            // Freeze the variable
            Choice choiceToSelect = choice;

            button.onClick.AddListener(delegate {
                OnClickChoiceButton(choiceToSelect);
            });
        }
    }

    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index); 
        RefreshUI(); 
    }
}