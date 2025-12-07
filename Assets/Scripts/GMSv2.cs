using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class GMSv2 : MonoBehaviour
{
    [Header("Ink Files")]
    [SerializeField] private TextAsset inkJSONAsset;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI storyText;

    // Choice button prefab as GameObject to make assigning easier
    [SerializeField] private GameObject choiceButtonPrefab;

    [SerializeField] private Transform choiceContainer;

    [SerializeField] private Button continueButton;

    private Story story;

    void Start()
    {
        if (inkJSONAsset == null)
        {
            Debug.LogError("Ink JSON Asset is missing");
            enabled = false;
            return;
        }

        story = new Story(inkJSONAsset.text);

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (continueButton != null)
        {
            continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        }
    }

    public void OnContinueButtonClicked()
    {
        if (story == null) return;

        if (story.currentChoices.Count > 0)
        {
            // If choices are visible, ignore the continue button
            return;
        }

        if (story.canContinue)
        {
            string text = story.Continue().Trim();

            if (storyText != null)
            {
                if (string.IsNullOrEmpty(storyText.text))
                    storyText.text = text;
                else
                    storyText.text += "\n" + text;
            }
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (story == null) return;

        ClearChoiceButtons();

        var currentChoices = story.currentChoices;
        bool hasChoices = currentChoices.Count > 0;

        // Show choices if any
        if (hasChoices && choiceContainer != null && choiceButtonPrefab != null)
        {
            choiceContainer.gameObject.SetActive(true);

            foreach (Choice choice in currentChoices)
            {
                GameObject buttonObj =
                    Instantiate(choiceButtonPrefab, choiceContainer);

                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText =
                    buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                    buttonText.text = choice.text;

                Choice choiceToSelect = choice;
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        OnClickChoiceButton(choiceToSelect);
                    });
                }
            }
        }
        else if (choiceContainer != null)
        {
            choiceContainer.gameObject.SetActive(false);
        }

        // Continue button only active when we can continue and there are no choices
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(story.canContinue && !hasChoices);
        }
    }

    private void ClearChoiceButtons()
    {
        if (choiceContainer == null) return;

        for (int i = choiceContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceContainer.GetChild(i).gameObject);
        }
    }

    private void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshUI();
    }
}
