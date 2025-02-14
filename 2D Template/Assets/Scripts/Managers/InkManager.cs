using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class InkManager : MonoBehaviour
{
    public TextAsset inkJSON; // The compiled Ink file
    public TMP_Text dialogueText;  // TextMeshPro for dialogue
    public Button choicePrefab; // Button prefab for choices
    public Transform choiceContainer; // Parent object for choice buttons
    public Button continueButton; // Button to advance dialogue

    private Story story; // Ink story instance
    private bool waitingForPlayerInput = false; // Ensures the player clicks to advance

    void Start()
    {
        if (inkJSON != null)
        {
            story = new Story(inkJSON.text);
            RefreshUI();
        }
        else
        {
            Debug.LogError("Ink JSON file not assigned!");
        }
        
        continueButton.onClick.AddListener(ContinueDialogue);
        continueButton.gameObject.SetActive(false); // Ensure it's hidden at start
    }

    void RefreshUI()
    {
        // Clear previous choices
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }

        // If there is more dialogue, wait for player input to continue
        if (story.canContinue)
        {
            dialogueText.text = story.Continue();
            waitingForPlayerInput = true;
            continueButton.gameObject.SetActive(true); // Show continue button
        }
        else
        {
            // No more dialogue, clear the text box
            waitingForPlayerInput = false;
            continueButton.gameObject.SetActive(false); // Hide continue button
            dialogueText.text = ""; // Clear dialogue box
            gameObject.SetActive(false); // Hide dialogue box
        }

        // Show choices if available
        if (story.currentChoices.Count > 0)
        {
            waitingForPlayerInput = false; // Prevent auto-continuing when choices appear
            continueButton.gameObject.SetActive(false); // Hide continue button

            foreach (Choice choice in story.currentChoices)
            {
                Button choiceButton = Instantiate(choicePrefab, choiceContainer);
                choiceButton.GetComponentInChildren<TMP_Text>().text = choice.text;
                choiceButton.onClick.AddListener(() => ChooseChoice(choice.index));
            }
        }
    }

    void ContinueDialogue()
    {
        if (waitingForPlayerInput)
        {
            waitingForPlayerInput = false;
            RefreshUI();
        }
    }

    void ChooseChoice(int choiceIndex)
    {
        story.ChooseChoiceIndex(choiceIndex);
        story.Continue();
        RefreshUI();
    }
}
