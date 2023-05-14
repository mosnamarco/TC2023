using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextAsset inkAsset;
    public GameObject choiceButton;
    public GameObject choicePanel;

    private Story _inkStory;
    private void Awake()
    {
        _inkStory = new Story(inkAsset.text);    
        dialogueText.text = _inkStory.Continue();
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space)) { 
            continueStory();
        }
    }

    public void continueStory()
    {
        if (_inkStory.canContinue)
        {
            string text = _inkStory.Continue();
            text = text?.Trim();
            dialogueText.text = text;
        }

        if (_inkStory.currentChoices.Count > 0)
        {
            handleChoices();
        }
    }

    private bool isChoicesGenerated = false;
    private void handleChoices()
    {
        if (!isChoicesGenerated)
        {
            foreach (Choice choice in _inkStory.currentChoices)
            {
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
                GameObject newObject = Instantiate(choiceButton, choicePanel.transform);

                newObject.GetComponent<Button>().onClick.AddListener(() => {
                    _inkStory.ChooseChoiceIndex(choice.index);
                    continueStory();
                    destroyChoices();
                });

                isChoicesGenerated = true;
            }
        }
    }

    private void destroyChoices()
    {
        foreach (Button button in choicePanel.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
        isChoicesGenerated = false;
    }
}
