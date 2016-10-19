using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static PhoneTypes CurrentPhoneType = PhoneTypes.UNIDENTIFIED;
	public static CharacterType CurrentCharacterType;
	public static CharacterConvo CurrentCharacterConvo;
	public static FrameType CurrentFrame;
	public static int CurrentMapPositionIndex,
		CurrentConvoIndex;
	public static bool MapIconActive,
		GameIconActive,
		MessageIconActive,
		ContactsIconActive;

	public GameObject CollectDiamond,
		GetBone,
		ProtectCookie,
		FindFF;
	public SpriteRenderer BlackFadeMask,
		WhiteFadeMask;

	public GameObject WinText, TransitionScreen;
	public float MiniGameTimer;
	public bool GameOn;

	private MeshRenderer _fireflyLightRenderer;
	private LightController _playerLightController;
	private SpriteRenderer _currentInstructions;
	private bool _removeInstructions;
	public static float NumberOfSections;
	private CanvasGroup _transitionScreenCanvasGroup;
	public static bool FinishedGame;


	void Awake ()
	{

		if (CurrentCharacterType == CharacterType.Marlon)
			NumberOfSections = 7.0f;
		else if (CurrentCharacterType == CharacterType.Isaac)
			NumberOfSections = 5.0f;
		else if (CurrentCharacterType == CharacterType.Felix)
			NumberOfSections = 9.0f;

		#region DeviceResolution
		if (CurrentPhoneType == PhoneTypes.UNIDENTIFIED && Camera.main != null) {
			float currentAspect = Camera.main.aspect;

			//Galaxy S7
			if (currentAspect == 1440f / 2560f) {
				CurrentPhoneType = PhoneTypes.GALAXYS7;
			}
			//iPhone6
			if (currentAspect == 750 / 1334) {
				CurrentPhoneType = PhoneTypes.IPHONE6;
			}
			//iPhone5
			if (currentAspect == 640 / 1136) {
				CurrentPhoneType = PhoneTypes.IPHONE5;
			}
			//iPhone4
			if (currentAspect == 640 / 960) {
				CurrentPhoneType = PhoneTypes.IPHONE4;
			}
		}
		#endregion



		//Mason: Temp solution to test transition between phone and game scenes without character select. If you are seeing this, please feel free to remove.
		//Ashlyn: This is ok, just comment stuff like this out before making commits.
		//CurrentCharacterType = CharacterType.Marlon;
		//CurrentConvoIndex = 0;
		//CurrentCharacterConvo = CharacterConvo.Mom;

		/*TransitionScreen.SetActive (true);
		TransitionScreen.GetComponent<CanvasGroup> ().alpha = 1f;*/

		if (SceneManager.GetActiveScene ().name == "Prioritizing") {
			GameObject instructions = Instantiate (CollectDiamond, new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
			_currentInstructions = instructions.GetComponent<SpriteRenderer> ();

			if (CurrentCharacterType == CharacterType.Felix) {
				if (CurrentConvoIndex == 1) {
					FindObjectOfType<BackgroundManager> ().AddNewSegment ("Level 1-1");
				}
				if (CurrentConvoIndex == 2) {
					FindObjectOfType<BackgroundManager> ().AddNewSegment ("Level 2-1");
				}
				if (CurrentConvoIndex == 3) {
					FindObjectOfType<BackgroundManager> ().AddNewSegment ("Level 3-1");
				}
			}
			if (CurrentCharacterType == CharacterType.Marlon) {
				if (CurrentConvoIndex == 5) {
					FindObjectOfType<BackgroundManager> ().AddNewSegment ("Level 1-1");
				} else if (CurrentConvoIndex == 6) {
					FindObjectOfType<BackgroundManager> ().AddNewSegment ("Level 3-1");
				}
			}
		}
		if (SceneManager.GetActiveScene ().name == "RoomEscape") {
			GameObject instructions = Instantiate (GetBone, new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
			_currentInstructions = instructions.GetComponent<SpriteRenderer> ();
			GameObject currentRELevel = null;

			if (CurrentCharacterType == CharacterType.Marlon) {
				if (CurrentConvoIndex == 0)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 1);
				else if (CurrentConvoIndex == 1)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 3);
				else if (CurrentConvoIndex == 2)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 4);
			}
			if (CurrentCharacterType == CharacterType.Isaac) {
				if (CurrentConvoIndex == 0)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 3);
				else if (CurrentConvoIndex == 1)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 4);
				else if (CurrentConvoIndex == 2)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 1);
			}
			if (CurrentCharacterType == CharacterType.Felix) {
				if (CurrentConvoIndex == 4)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 5);
				else if (CurrentConvoIndex == 5)
					currentRELevel = Resources.Load<GameObject> ("Prefabs/Minigames/RoomEscape/Level " + 1);
			}

			Instantiate (currentRELevel, currentRELevel.transform.position, Quaternion.identity);
		}
		if (SceneManager.GetActiveScene ().name == "Irritation") {
			GameObject instructions = Instantiate (ProtectCookie, new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
			_currentInstructions = instructions.GetComponent<SpriteRenderer> ();
		}
		if (SceneManager.GetActiveScene ().name == "FindingFriends") {
			GameObject fFLevel = new GameObject ();
			if (CurrentCharacterType == CharacterType.Marlon) {
				if (CurrentConvoIndex == 3)
					fFLevel = Resources.Load<GameObject> ("Prefabs/Minigames/FindingFriends/Level 1 Hard");
				if (CurrentConvoIndex == 4)
					fFLevel = Resources.Load<GameObject> ("Prefabs/Minigames/FindingFriends/Level 1 Easy");
			} else if (CurrentCharacterType == CharacterType.Isaac) {
				if (CurrentConvoIndex == 2)
					fFLevel = Resources.Load<GameObject> ("Prefabs/Minigames/FindingFriends/Level 1 Hard");
				if (CurrentConvoIndex == 3)
					fFLevel = Resources.Load<GameObject> ("Prefabs/Minigames/FindingFriends/Level 1 Easy");
			}
			fFLevel = (GameObject)Instantiate (fFLevel, fFLevel.transform.position, Quaternion.identity);

			_fireflyLightRenderer = GameObject.Find ("Player").GetComponentInChildren<MeshRenderer> ();
			_playerLightController = _fireflyLightRenderer.gameObject.GetComponent<LightController> ();
			_fireflyLightRenderer.enabled = false;
			_playerLightController.enabled = false;
			GameObject instructions = Instantiate (FindFF, new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, 1), Quaternion.identity) as GameObject;
			_currentInstructions = instructions.GetComponent<SpriteRenderer> ();
		}
	}



	void Update ()
	{
		if (CurrentConvoIndex / NumberOfSections == 1)
			FinishedGame = true;
		if (GameOn) {
			MiniGameTimer += Time.deltaTime;

			if (CurrentCharacterType == CharacterType.Marlon && SceneManager.GetActiveScene ().name == "Prioritizing") {
				if (MiniGameTimer > 299) {
					GameOn = false;

					LoadScene (CurrentCharacterType.ToString () + "Frame");
				}
			} else {
				if (MiniGameTimer > 29) {
					if (CurrentCharacterType != CharacterType.Marlon && SceneManager.GetActiveScene ().name == "FindingFriends" && CurrentConvoIndex == 4) {
						MapIconActive = true;
					}

					MessageIconActive = true;
					GameOn = false;
					LoadScene (CurrentCharacterType.ToString () + "Frame");
				}
			}

			//ASHLYN: This switch is where all of the transitions out of the minigames happen. They are all watching MiniGameTimer and will call FadeHandler to bring up the next scene. Check FadeHandler for more.
			//switch (currentSceneName)
			//{
			//    case "Prioritizing":
			//        if (MiniGameTimer > 29 && CurrentCharacterType != CharacterType.Marlon)
			//        {
			//            FadeHandler(false, false, CurrentCharacterType.ToString() + "Frame");
			//        }
			//        else if (MiniGameTimer > 299 && CurrentCharacterType == CharacterType.Marlon)
			//        {
			//        }
			//        break;
			//    case "RoomEscape":
			//        if (MiniGameTimer > 29)
			//        {
			//            //ASHLYN: Certain situations require for a new minigame to be loaded rather than the frame. This will be based on which character you've selected and the conversation you had last.
			//            if (GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 2)
			//            {
			//                FadeHandler(false, false, "FindingFriends");
			//            }
			//            else if (GameManager.CurrentCharacterType == CharacterType.Felix && GameManager.CurrentConvoIndex == 4)
			//            {
			//                FadeHandler(false, false, "Irritation");
			//            }
			//            else
			//            {
			//                FadeHandler(false, false, CurrentCharacterType.ToString() + "Frame");
			//            }
			//        }
			//        break;
			//    case "Irritation":
			//        if (MiniGameTimer > 29)
			//        {
			//            FadeHandler(false, true, CurrentCharacterType.ToString() + "Frame");
			//        }
			//        break;
			//    case "FindingFriends":
			//        if (MiniGameTimer > 29)
			//        {
			//            if (GameManager.CurrentCharacterType == CharacterType.Isaac && GameManager.CurrentConvoIndex == 3)
			//            {
			//                FadeHandler(false, false, "Irritation");
			//            }
			//            FadeHandler(false, false, CurrentCharacterType.ToString() + "Frame");
			//        }
			//        break;
			//    case "PhoneScene":

			//        if (MiniGameTimer > 29)
			//        {
			//            FadeHandler(false, false, "PhoneScreen");
			//        }
			//        break;
			//    default:
			//        Debug.Log("This scene doesn't have a timer.");
			//        break;
			//}
		} else if (Input.GetKeyDown (KeyCode.Mouse0) && _currentInstructions) {
			_removeInstructions = true;
		}

		if (_removeInstructions) {
			FadeHandler (true, false, "");
		}
	}


	//NOTE: I'd like to stream line this...
	//Ash Note: Moved this to Game Manager since it should handle transitions between dialogue and minigames. The DialogManager should only manage dialog
	//ASHLYN: This method is the one used for loading minigames. It looks at the current character and which convorsation you're on to figure out which scene to load next.
	public void LoadMinigame ()
	{
		switch (CurrentCharacterType) {
		case CharacterType.Felix:
			if (CurrentConvoIndex <= 3) {
				SceneManager.LoadScene ("Prioritizing");
			} else if (CurrentConvoIndex <= 5) {
				SceneManager.LoadScene ("RoomEscape");
			} else if (CurrentConvoIndex == 6) {
				SceneManager.LoadScene ("Irritation");
			} else if (CurrentConvoIndex == 7) {
				GameManager.CurrentConvoIndex = 0;
				StartCoroutine (EndGame ());
			}
			break;
		case CharacterType.Isaac:
			if (CurrentConvoIndex <= 2) {
				CurrentCharacterConvo = CharacterConvo.Lorna;
				LoadScene ("RoomEscape");
			} else if (CurrentConvoIndex == 3) {
				LoadScene ("FindingFriends");
			} else if (CurrentConvoIndex == 4) {
				LoadScene ("Irritation");
			} else if (CurrentConvoIndex == 5) {
				GameManager.CurrentConvoIndex = 0;
				StartCoroutine (EndGame ());
			}
			break;
		case CharacterType.Marlon:
			if (CurrentConvoIndex <= 2) {
				CurrentCharacterConvo = CharacterConvo.Mom;
				LoadScene ("RoomEscape");
			} else if (CurrentConvoIndex <= 4) {
				LoadScene ("FindingFriends");
			} else if (CurrentConvoIndex <= 6) {
				LoadScene ("Prioritizing");
			} else if (CurrentConvoIndex == 7) {
				GameManager.CurrentConvoIndex = 0;
				StartCoroutine (EndGame ());

			}
			break;
		default:
			Debug.LogWarning ("Did you add a new character type? You'll have to update a lot of switch statements, you know...");
			break;
		}
	}




	//Handles the endgame state, thanking the player and transitioning to the Character Selection Scene
	public  IEnumerator EndGame ()
	{
		GameManager.CurrentConvoIndex = 0;

		WinText.gameObject.SetActive (true);
		StartCoroutine (UIFadeHandler (0f, 1.0f, 1f, WinText.GetComponent<CanvasGroup> ()));
		yield return new WaitForSeconds (1);
		LoadScene ("CharacterSelection");

	}

	//Handles fade transition for UI elements. This will be moved to a UIManager at a later time.
	public IEnumerator UIFadeHandler (float aValue, float toValue, float aTime, CanvasGroup alphaObject)
	{
		//float alpha = alphaObject.alpha;
		if (aValue < toValue) {
			for (float t = aValue; t < toValue; t += Time.deltaTime / aTime) {
				alphaObject.alpha += t;
				yield return null;
			}
		} 

		if (aValue > toValue) {
			for (float t = aValue; t > toValue; t -= Time.deltaTime / aTime) {
				alphaObject.alpha -= t;
				yield return null;
			}
		}
	}



	//ASHLYN: This method takes two paramaters: fadingIn determins whether we're fading in on a scene or out of one. Basically use false for changing scenes. You basically wont need to use this method when fadingIn == true.
	//SceneToLoad is a just a string of the scene you're to load. Long story short: call the method as FadeHandler(false, "NameOfScene");
	public void FadeHandler (bool fadingIn, bool win, string SceneToLoad)
	{
		if (fadingIn) {
			if (_currentInstructions != null) {
				_currentInstructions.color = new Color (_currentInstructions.color.r, _currentInstructions.color.g, _currentInstructions.color.b, _currentInstructions.color.a - (2 * Time.deltaTime));
				if (_currentInstructions.color.a < 0.1f) {
					_currentInstructions.color = new Color (_currentInstructions.color.r, _currentInstructions.color.g, _currentInstructions.color.b, 0);
					_currentInstructions = null;
					_removeInstructions = false;
					GameOn = true;

					if (SceneManager.GetActiveScene ().name == "FindingFriends") {
						_fireflyLightRenderer.enabled = true;
						_playerLightController.enabled = true;
					}
				}
			}
//		} else {
//			if (!win && BlackFadeMask != null) {
//				//BlackFadeMask.color = new Color (BlackFadeMask.color.r, BlackFadeMask.color.g, BlackFadeMask.color.b, BlackFadeMask.color.a + (2 * Time.deltaTime));
//				//if (BlackFadeMask.color.a > 0.9f)
//				LoadScene (SceneToLoad);//SceneManager.LoadScene (SceneToLoad);
//			} else if (win && WhiteFadeMask != null) {
//				/*WhiteFadeMask.color = new Color (WhiteFadeMask.color.r, WhiteFadeMask.color.g, WhiteFadeMask.color.b, WhiteFadeMask.color.a + (2 * Time.deltaTime));
//				if (WhiteFadeMask.color.a > 0.9f) {
//					WhiteFadeMask.color = new Color (WhiteFadeMask.color.r, WhiteFadeMask.color.g, WhiteFadeMask.color.b, 1f);*/
//				LoadScene (SceneToLoad, new Color (255, 255, 255, 0));//SceneManager.LoadScene (SceneToLoad);
//				//}
//			} else
//				LoadScene (SceneToLoad);//SceneManager.LoadScene (SceneToLoad);
		}
	}


	//Use this method to transition between scenes
	public void LoadScene (string SceneToLoad)
	{
		if (NiceSceneTransition.instance != null) {
			NiceSceneTransition.instance.LoadScene (SceneToLoad);
		} else {
			SceneManager.LoadScene (SceneToLoad, LoadSceneMode.Single);
		}
	}

	public void LoadScene (string SceneToLoad, Color color)
	{

		if (NiceSceneTransition.instance != null) {
			NiceSceneTransition.instance.LoadScene (SceneToLoad, color);
		} else {
			SceneManager.LoadScene (SceneToLoad, LoadSceneMode.Single);
		}
	}

	public void UpdateCharacter ()
	{
		
	}




}