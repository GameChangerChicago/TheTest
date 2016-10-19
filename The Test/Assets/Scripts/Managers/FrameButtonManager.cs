using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrameButtonManager : MonoBehaviour
{
	public GameObject Pedometer;
	public Text PedCompletionText;
	public Text PedMilesText;
	private float transitionTime = 2f;
	private float GameManager.NumberOfSections;

	public GameObject MapIcon,
		GameIcon,
		MessageIcon,
		ContactsIcon;

	public GameObject MapNotification,
		GameNotification,
		MessageNotification,
		ContactsNotification;

	//ASHLYN: All the magic happens in these properties,
	//If it's being set to true then it enables the button component and makes the button fully opaque
	//Conversely if it's being set to false it disables the button component and makes the button partially transparent
	//In most cases you wont be setting these values directly but rather you'll be changing the correspoding static field in GameManager
	//The only time you'll need to set these properties directly is if you're changing them while in the frame
	public bool MapIconActive {
		get {
			return GameManager.MapIconActive;
		}
		set {
			if (_mapIconActive == !value) {
				if (value) {
					_mapImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
				} else {
					_mapImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, alphaValue);
				}
				_mapButton.enabled = value;
				_mapIconActive = value;
				_mapNotification.SetActive (value);
				GameManager.MapIconActive = value;
			}
		}
	}

	private bool _mapIconActive = true;

	//ASHLYN: All of these work the same way for each icon button
	public bool GameIconActive {
		get {
			return GameManager.GameIconActive;
		}
		set {
			if (_gameIconActive == !value) {
				if (value) {
					_gameImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
				} else {
					_gameImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, alphaValue);
				}
				_gameButton.enabled = value;
				_gameIconActive = value;
				_gameNotification.SetActive (value);
				GameManager.GameIconActive = value;
			}
		}
	}

	private bool _gameIconActive = true;

	public bool MessageIconActive {
		get {
			return GameManager.MessageIconActive;
		}
		set {
			if (_messageIconActive == !value) {
				if (value) {
					_messageImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
				} else {
					_messageImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, alphaValue);
				}
				_messageButton.enabled = value;
				_messageIconActive = value;
				_messageNotification.SetActive (value);
				GameManager.MessageIconActive = value;
			}
		}
	}

	private bool _messageIconActive = true;

	public bool ContactsIconActive {
		get {
			return GameManager.ContactsIconActive;
		}
		set {
			if (_contactsIconActive == !value) {
				if (value) {
					_contactsImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, 255);
				} else {
					_contactsImage.color = new Color (_mapImage.color.r, _mapImage.color.g, _mapImage.color.b, alphaValue);
				}
				_contactsButton.enabled = value;
				_contactsIconActive = value;
				_contactsNotification.SetActive (value);
				GameManager.ContactsIconActive = value;
			}
		}
	}

	private bool _contactsIconActive = true;
    
	private Button _mapButton,
		_gameButton,
		_messageButton,
		_contactsButton;
	private Image _mapImage,
		_gameImage,
		_messageImage,
		_contactsImage;
	private GameObject _mapNotification,
		_gameNotification,
		_messageNotification,
		_contactsNotification;

	//Should be between 0 and 1
	private float alphaValue = .50f;


	private DialogManager _theDialogManager;

	void Start ()
	{
		_theDialogManager = FindObjectOfType<DialogManager> ();
		_mapButton = MapIcon.GetComponent<Button> ();
		_gameButton = GameIcon.GetComponent<Button> ();
		_messageButton = MessageIcon.GetComponent<Button> ();
		_contactsButton = ContactsIcon.GetComponent<Button> ();

		_mapImage = MapIcon.GetComponent<Image> ();
		_gameImage = GameIcon.GetComponent<Image> ();
		_messageImage = MessageIcon.GetComponent<Image> ();
		_contactsImage = ContactsIcon.GetComponent<Image> ();

		_mapNotification = MapNotification;
		_gameNotification = GameNotification;
		_messageNotification = MessageNotification;
		_contactsNotification = ContactsNotification;




		ButtonSetup ();
	}

	public void Update ()
	{
		
	}

	IEnumerator UpdatePedometer (float percentageComplete)
	{
		//Pedometor.GetComponent<Image>().fillAmount +
		//yield return new WaitForSeconds (waitForSeconds);
		float time = 0f;
		while (time <= transitionTime) {
			Pedometer.GetComponent<Image> ().fillAmount = Mathf.Lerp (0, percentageComplete, time);
			PedMilesText.text = Mathf.Round (Mathf.Lerp (0, ((GameManager.NumberOfSections / 4.36f) * percentageComplete), time) * 100f) / 100f + " mi";
			PedCompletionText.text = Mathf.Round (Mathf.Lerp (0, percentageComplete * 100f, time) * 10f) / 10f + "% complete";
			time += Time.deltaTime * (1.0f / transitionTime);


			yield return null;
		}



	}



	private void ButtonSetup ()
	{
		//ASHLYN: This bit makes sure that our properties are in sync with the static values from the GameManager
		//This only happens on start so if you don't run start(basically if you're already in the frame scene)
		MapIconActive = GameManager.MapIconActive;
		GameIconActive = GameManager.GameIconActive;
		MessageIconActive = GameManager.MessageIconActive;
		ContactsIconActive = GameManager.ContactsIconActive;

		_mapNotification.SetActive (false);
		_gameNotification.SetActive (false);
		_messageNotification.SetActive (false);
		_contactsNotification.SetActive (false);
		Pedometer.GetComponent<Image> ().fillAmount = 0;


		//Here you'll be able to set which icons should be active on start based on character and current convo index.
		//Convo index increments at the end of the message convo if you're curious about that
		switch (GameManager.CurrentCharacterType) {
		case CharacterType.Marlon:
			
			if (GameManager.CurrentConvoIndex <= 1 || GameManager.CurrentConvoIndex >= 5) {

				GameManager.CurrentCharacterConvo = CharacterConvo.Mom;

				MapIconActive = false;
				GameIconActive = true;
				MessageIconActive = true;
				ContactsIconActive = false;

				_messageNotification.SetActive (true);
				_gameNotification.SetActive (false);

			
			}
			if (GameManager.CurrentConvoIndex <= 4) {

				if (GameManager.CurrentConvoIndex == 2)
					GameManager.CurrentCharacterConvo = CharacterConvo.Todd;
				else if (GameManager.CurrentConvoIndex == 3)
					GameManager.CurrentCharacterConvo = CharacterConvo.Kara;
				else
					GameManager.CurrentCharacterConvo = CharacterConvo.Mom;
				
				MapIconActive = false;
				GameIconActive = false;
				_messageNotification.SetActive (true);
				_gameNotification.SetActive (false);
			}
			break;
		case CharacterType.Isaac:
			{
				if (GameManager.CurrentConvoIndex <= 1) 
					GameManager.CurrentCharacterConvo = CharacterConvo.Lorna;
				else 
					GameManager.CurrentCharacterConvo = CharacterConvo.David;
				

				MapIconActive = false;
				GameIconActive = true;
				MessageIconActive = true;
				ContactsIconActive = false;

				_messageNotification.SetActive (true);
				_gameNotification.SetActive (false);
				
			}
			break;
		case CharacterType.Felix:{
				if (GameManager.CurrentConvoIndex >= 5 || GameManager.CurrentConvoIndex <= 6 )
					GameManager.CurrentCharacterConvo = CharacterConvo.Sadie;
				else
					GameManager.CurrentCharacterConvo = CharacterConvo.Glenn;


				MapIconActive = false;
				GameIconActive = true;
				MessageIconActive = true;
				ContactsIconActive = false;

				_messageNotification.SetActive (true);
				_gameNotification.SetActive (false);
			}
			break;
		default:
			break;
		}

		StartCoroutine (UpdatePedometer ((GameManager.CurrentConvoIndex + 1) / GameManager.NumberOfSections));


	}
}