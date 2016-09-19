using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
            GameManager.CurrentCharacterType = CharacterType.Marlon;
        }
        if (characterName == "Felix")
        {
            GameManager.CurrentCharacterType = CharacterType.Felix;
        }
        if (characterName == "Isaac")
        {
            GameManager.CurrentCharacterType = CharacterType.Isaac;
        }
        GameManager.CharacterSelected = true;
        CharacterNameDisplay.text = characterName;
    }

    public void ContinueToDialog()
    {
        //SceneManager.LoadScene("MapScene");
        //HACK: As pretty as this looks, this is a hack for a pre map game build
        if (GameManager.CurrentCharacterType == CharacterType.Marlon)
        {
            SceneManager.LoadScene("RoomEscape");
        }
        else
        {
            SimpleFrame.SetActive(false);
            _theDialogManager.LoadPieceOfDialog();
        }
    }
}