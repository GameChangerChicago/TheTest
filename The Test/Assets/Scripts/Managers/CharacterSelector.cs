using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{

	public GameObject[] CharacterArray;
	public Button PreviousButton, NextButton;
	private int ArrayIndex;
	private GameObject SelectedCharacter, PreviousCharacter;
	public string CharacterName;
	private GameManager _theGameManager;
	private DialogManager _theDialogManager;

	void Start ()
	{
		ArrayIndex = 0;
		SelectedCharacter = CharacterArray [ArrayIndex];
		SelectedCharacter.SetActive (true);
		CharacterName = SelectedCharacter.name;

		_theGameManager = FindObjectOfType<GameManager> ();
		_theDialogManager = FindObjectOfType<DialogManager> ();
	}

    private void UpdateCharacter()
    {
        if (CharacterName == "Marlon")
        {
            GameManager.CurrentCharacterType = CharacterType.Marlon;
        }
        if (CharacterName == "Felix")
        {
            GameManager.CurrentCharacterType = CharacterType.Felix;
        }
        if (CharacterName == "Isaac")
        {
            GameManager.CurrentCharacterType = CharacterType.Isaac;
        }
    }

	public void ContinueToDialog ()
	{
        UpdateCharacter();
		SceneManager.LoadScene ("MapScene");
		//HACK: As pretty as this looks, this is a hack for a pre map game build
		//if (GameManager.CurrentCharacterType == CharacterType.Marlon)
		//{
		//    SceneManager.LoadScene("RoomEscape");
		//}
		//else
		//{
		//    SimpleFrame.SetActive(false);
		//    _theDialogManager.LoadPieceOfDialog();
		//}
	}

	public void Update ()
	{
		//Debug.Log (ArrayIndex); 
	}

	public void NextChoice ()
	{
		if (PreviousButton.interactable == false) {
			PreviousButton.interactable = true;
		}
		
		if (ArrayIndex < 2) {
			ArrayIndex++;
			PreviousCharacter = SelectedCharacter;
			PreviousCharacter.SetActive (false);
			SelectedCharacter = CharacterArray [ArrayIndex];
			SelectedCharacter.SetActive (true);
			CharacterName = SelectedCharacter.name;
		
		}
		if (ArrayIndex == 2) {
			
			NextButton.interactable = false;

		}
			
	}

	public void PreviousChoice ()
	{
		if (NextButton.interactable == false)
			NextButton.interactable = true;
			

		if (ArrayIndex > 0) {
			ArrayIndex--;
			PreviousCharacter = SelectedCharacter;
			PreviousCharacter.SetActive (false);
			SelectedCharacter = CharacterArray [ArrayIndex];
			SelectedCharacter.SetActive (true);
			CharacterName = SelectedCharacter.name;
		} 

		if (ArrayIndex == 0) {
			ArrayIndex = 0;
			PreviousButton.interactable = false;


		}
	}
}
