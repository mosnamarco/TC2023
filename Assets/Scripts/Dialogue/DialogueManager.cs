using Ink.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public TextAsset inkAsset;
    public GameObject profileImage;
    public GameObject backgroundImage;
    public GameObject choiceButton;
    public GameObject choicePanel;

    private Story _inkStory;
    private void Awake()
    {
        _inkStory = new Story(inkAsset.text);    
        dialogueText.text = _inkStory.Continue();
        parseTags();
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Space)) { 
            continueStory();
        }
    }

    private void parseTags()
    {
        List<string> tags = _inkStory.currentTags;
        string[] tagparts;
        foreach (string tag in tags)
        {
            tagparts = tag.Split(':');
            if (tagparts[0] == "name")
            {
                nameText.text = tagparts[1];
            }

            if (tagparts[0] == "profile")
            {
                profileImage.SetActive(true);
                string imagePath = "Assets/Sprites/" + tagparts[1] + ".jpg";
                if (!File.Exists(imagePath))
                {
                    imagePath = "Assets/Sprites/" + tagparts[1] + ".png";
                }
                Texture2D texture = LoadTextureFromFile(imagePath);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                profileImage.GetComponent<Image>().sprite = sprite;
            }

            if (tagparts[0] == "bg")
            {
                string imagePath = "Assets/Sprites/BG/" + tagparts[1] + ".jpg";
                if (!File.Exists(imagePath))
                {
                    imagePath = "Assets/Sprites/BG/" + tagparts[1] + ".png";
                }
                Texture2D texture = LoadTextureFromFile(imagePath);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                backgroundImage.GetComponent<Image>().sprite = sprite;
            }

            if (tagparts[0] == "clearProfile")
            {
                profileImage.SetActive(false);
            }
        }
    }

    public void continueStory()
    {
        if (_inkStory.canContinue)
        {
            string text = _inkStory.Continue();
            text = text?.Trim();
            dialogueText.text = text;
            parseTags();
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

    private Texture2D LoadTextureFromFile(string path)
    {
        Texture2D texture;

        // Load the image from the file path
        byte[] fileData = System.IO.File.ReadAllBytes(path);

        // Create a new texture and load the image data into it
        texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        return texture;
    }
}
