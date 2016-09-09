using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleFrameController : MonoBehaviour
{
    public GameObject SimpleFrame;
    public Text CharacterNameDisplay;
    private GameManager _theGameManager;
    private DialogManager _theDialogManager;
    
    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
        _theDialogManager = FindObjectOfType<DialogManager>();
    }

    public void PlayerSelectButton(string characterName)
    {
        if(characterName == "Marlon")
        {
            GameManager.CurrentCharacterType = CharacterType.MARLON;
        }
        if (characterName == "Felix")
        {
            GameManager.CurrentCharacterType = CharacterType.FELIX;
        }
        if (characterName == "Isaac")
        {
            GameManager.CurrentCharacterType = CharacterType.ISAAC;
        }
        GameManager.CharacterSelected = true;
        CharacterNameDisplay.text = characterName;
    }

    public void ContinueToDialog()
    {
        SimpleFrame.SetActive(false);
        _theDialogManager.LoadPieceOfDialog();
    }
}