using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class GMSv3 : MonoBehaviour
{
    [Header("Ink Files")]
    [SerializeField] private TextAsset inkJSONAsset;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private GameObject choiceButtonPrefab;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backButton;

    [Header("Backgrounds (optional, can leave empty for now)")]
    [SerializeField] private Image backgroundImage;

    private Story story;

    // Snapshot of story state so we can undo choices
    [System.Serializable]
    private class StorySnapshot
    {
        public string jsonState;
        public string visibleText;
    }

    private Stack<StorySnapshot> snapshotStack = new Stack<StorySnapshot>();

    private void Start()
    {
        if (inkJSONAsset == null)
        {
            Debug.LogError("[GMSv2] Ink JSON Asset is missing");
            enabled = false;
            return;
        }

        try
        {
            story = new Story(inkJSONAsset.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[GMSv2] Failed to create Story from JSON: " + ex.Message);
            enabled = false;
            return;
        }

        if (storyText != null)
        {
            storyText.text = string.Empty;
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        if (backButton != null)
        {
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        // Show the first line on start
        ShowNextLine();
        RefreshChoicesAndButtons();
    }

    private void OnDestroy()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }

    private void OnContinueButtonClicked()
    {
        if (story == null)
        {
            Debug.LogWarning("[GMSv2] Continue pressed but story is null");
            return;
        }

        // Ignore Next if choices are visible
        if (story.currentChoices.Count > 0)
        {
            Debug.Log("[GMSv2] Choices visible, Next ignored");
            return;
        }

        ShowNextLine();
        RefreshChoicesAndButtons();
    }

    private void OnBackButtonClicked()
    {
        if (story == null)
        {
            Debug.LogWarning("[GMSv2] Back pressed but story is null");
            return;
        }

        if (snapshotStack.Count == 0)
        {
            Debug.Log("[GMSv2] No previous state to go back to");
            return;
        }

        StorySnapshot snapshot = snapshotStack.Pop();

        // Restore story state
        try
        {
            story.state.LoadJson(snapshot.jsonState);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[GMSv2] Failed to load story state: " + ex.Message);
            return;
        }

        // Restore visible text
        if (storyText != null)
        {
            storyText.text = snapshot.visibleText;
        }

        RefreshChoicesAndButtons();
    }

    private void ShowNextLine()
    {
        if (story == null) return;

        if (story.canContinue)
        {
            string line = story.Continue().Trim();

            if (storyText != null)
            {
                if (string.IsNullOrEmpty(storyText.text))
                    storyText.text = line;
                else
                    storyText.text += "\n" + line;
            }
        }
        else
        {
            Debug.Log("[GMSv2] No more content to continue");
        }
    }

    private void RefreshChoicesAndButtons()
    {
        if (story == null) return;

        ClearChoiceButtons();

        List<Choice> currentChoices = story.currentChoices;
        bool hasChoices = currentChoices.Count > 0;

        // Handle choices
        if (hasChoices && choiceContainer != null && choiceButtonPrefab != null)
        {
            choiceContainer.gameObject.SetActive(true);

            foreach (Choice choice in currentChoices)
            {
                GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);

                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI label =
                    buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (label != null)
                    label.text = choice.text;

                Choice capturedChoice = choice;

                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        OnChoiceButtonClicked(capturedChoice);
                    });
                }
            }
        }
        else if (choiceContainer != null)
        {
            choiceContainer.gameObject.SetActive(false);
        }

        // Continue button only when story can continue and no choices are present
        if (continueButton != null)
        {
            bool showContinue = story.canContinue && !hasChoices;
            continueButton.gameObject.SetActive(showContinue);
        }

        // Back button is enabled only if we have something to go back to
        if (backButton != null)
        {
            backButton.gameObject.SetActive(snapshotStack.Count > 0);
        }
    }

    private void OnChoiceButtonClicked(Choice choice)
    {
        if (story == null) return;

        // Save snapshot before applying the choice
        StorySnapshot snapshot = new StorySnapshot
        {
            jsonState = story.state.ToJson(),
            visibleText = storyText != null ? storyText.text : string.Empty
        };
        snapshotStack.Push(snapshot);

        // Apply the choice
        story.ChooseChoiceIndex(choice.index);

        // After choosing, move forward once into that branch
        ShowNextLine();
        RefreshChoicesAndButtons();
    }

    private void ClearChoiceButtons()
    {
        if (choiceContainer == null) return;

        for (int i = choiceContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceContainer.GetChild(i).gameObject);
        }
    }
}