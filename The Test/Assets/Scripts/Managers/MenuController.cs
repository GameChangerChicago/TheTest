using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{

	public GameObject HomeScreen, 
		MessageScreen, 
		DialogueScreen;

	public List<GameObject> MessageNotification = new List<GameObject> ();

	public Text ConvoCharacterName;
	private List<GameObject> _screenList,
		_messageList;
	private GameManager _theGameManager;
	private DialogManager _theDialogManager;
	private GameObject _messageNotification;
	private bool _firstMessage;


	// Use this for initialization
	void Start ()
	{
		_theGameManager = FindObjectOfType<GameManager> ();
		_theDialogManager = FindObjectOfType<DialogManager> ();
		_firstMessage = false;
	

		_screenList = new List<GameObject> ();
		_screenList.Add (HomeScreen);
		_screenList.Add (MessageScreen);
		_screenList.Add (DialogueScreen);

		
		foreach (GameObject g in _screenList) {
			if (g == HomeScreen)
				g.SetActive (true);
			else
				g.SetActive (false);
		}
			

	}



	void ShowNotification ()
	{

		
		if (_theDialogManager.NewNotification) {
			foreach (GameObject g in MessageNotification) {
				if (g.name == (GameManager.CurrentCharacterConvo.ToString () + "Notification")) {
					//Debug.Log (g.ToString ());
					g.SetActive (true);
				} else
					g.SetActive (false);
			}
		} else
			foreach (GameObject g in MessageNotification)
				g.SetActive (false);
			
		
	}

	public void ToMessageScreen ()
	{
		
		ShowNotification ();

		foreach (GameObject g in _screenList) {
			if (g == MessageScreen) {
				g.GetComponent <CanvasGroup> ().alpha = 0;
				g.SetActive (true);
				StartCoroutine (_theGameManager.UIFadeHandler (0f, 1f, .5f, MessageScreen.GetComponent<CanvasGroup> ()));

			} else {
				g.SetActive (false);


			} 


		}

	}

	public void ToHomeScreen ()
	{
		foreach (GameObject g in _screenList) {
			if (g == HomeScreen) {
				g.GetComponent <CanvasGroup> ().alpha = 0;
				g.SetActive (true);
				StartCoroutine (_theGameManager.UIFadeHandler (0f, 1f, .5f, HomeScreen.GetComponent<CanvasGroup> ()));
			} else
				g.SetActive (false);

		}

	}

	public void ToDialogueScreen (string MessageName)
	{
		if (GameManager.CurrentCharacterConvo.ToString () == MessageName) {

			foreach (GameObject g in _screenList) {
				if (g == DialogueScreen) {
					g.GetComponent <CanvasGroup> ().alpha = 0;
					g.SetActive (true);
					ConvoCharacterName.text = MessageName;
					StartCoroutine (_theGameManager.UIFadeHandler (0f, 1f, .5f, DialogueScreen.GetComponent<CanvasGroup> ()));

				} else
					g.SetActive (false);

			}


			if (!_firstMessage) {
				_theDialogManager.LoadPieceOfDialog ();
				//_theDialogManager.Invoke ("LoadPieceOfDialog", .25f);
				_firstMessage = true;
			}
		}



	}
		
}
