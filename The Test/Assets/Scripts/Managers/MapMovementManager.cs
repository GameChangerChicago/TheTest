﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MapMovementManager : MonoBehaviour
{
	public GameObject FelixMapPiece,
		IsaacMapPiece,
		MarlonMapPiece;
	public Transform[] FelixMapPaths,
		IsaacMapPaths,
		MarlonMapPaths;
	public Transform StartPos;

	protected bool facingRight {
		get {
			return _facingRight;
		}
		set {
			if (_facingRight != value) {
				currentPlayer.transform.localScale = new Vector3 (currentPlayer.transform.localScale.x * -1, currentPlayer.transform.localScale.y, currentPlayer.transform.localScale.z);

				_facingRight = value;
			}
		}
	}

	private bool _facingRight;

	protected bool facingDown {
		get {
			return _facingDown;
		}
		set {
			if (_facingDown != value) {
				_charactorAnimator.SetBool ("FaceDown", value);

				_facingDown = value;
			}
		}
	}

	private bool _facingDown;

	protected GameObject currentPlayer {
		get {
			switch (GameManager.CurrentCharacterType) {
			case CharacterType.Felix:
				_currentMapPaths = FelixMapPaths;
				break;
			case CharacterType.Isaac:
				_currentMapPaths = IsaacMapPaths;
				break;
			case CharacterType.Marlon:
				_currentMapPaths = MarlonMapPaths;
				break;
			default:
				Debug.LogError ("Do we have more than three character types? Check that.");
				break;
			}

			return _currentPlayer;
		}

		set {
			_currentPlayer = value;
		}
	}

	private GameObject _currentPlayer;
	private Transform[] _currentMapPaths;

	private Animator _charactorAnimator;
	private GameManager _theGameManager;
	private CameraManager _theCameraManager;
	private FrameController _theFrameController;
	private int _currentMapIndex = 0,
		_currentMapTarget;
	private bool _characterMoving;

	void Start ()
	{
		_theGameManager = FindObjectOfType<GameManager> ();
		_theCameraManager = FindObjectOfType<CameraManager> ();
		_theFrameController = FindObjectOfType<FrameController> ();
		currentPlayer = new GameObject ();
		currentPlayer.transform.position = new Vector3 (_currentMapPaths [GameManager.CurrentMapPositionIndex].position.x, _currentMapPaths [GameManager.CurrentMapPositionIndex].position.y + 0.7f, 0);

		PlayerMapPieceSetup ();
	}

	void Update ()
	{
		if (_characterMoving) {
			MapPieceMovementHandler ();
		}
	}

	private bool MapPieceMovementHandler ()
	{
		if (Vector3.Distance (new Vector3 (currentPlayer.transform.position.x, currentPlayer.transform.position.y, 0),
			    new Vector3 (_currentMapPaths [_currentMapIndex + 1].position.x, _currentMapPaths [_currentMapIndex + 1].position.y + 0.223f, 0)) > 0.1f) {
			float xDistance = Vector2.Distance (new Vector2 (_currentMapPaths [_currentMapIndex].position.x, 0), new Vector2 (_currentMapPaths [_currentMapIndex + 1].position.x, 0)),
			yDistance = Vector2.Distance (new Vector2 (0, _currentMapPaths [_currentMapIndex].position.y), new Vector2 (0, _currentMapPaths [_currentMapIndex + 1].position.y));
                  
			if (xDistance > yDistance) {
				yDistance /= xDistance;
				xDistance /= xDistance;
			} else {
				xDistance /= yDistance;
				yDistance /= yDistance;
			}

			if (_currentMapPaths [_currentMapIndex].position.x > _currentMapPaths [_currentMapIndex + 1].position.x) {
				facingRight = false;
			} else {
				facingRight = true;
			}
			xDistance *= -1;

			if (_currentMapPaths [_currentMapIndex].position.y > _currentMapPaths [_currentMapIndex + 1].position.y) {
				yDistance *= -1;
				facingDown = true;
			} else {
				facingDown = false;
			}

			currentPlayer.transform.Translate (new Vector3 (xDistance * Time.deltaTime * 2f,
				yDistance * Time.deltaTime * 2f, 0));

			return false;
		} else {
			if (_currentMapIndex != _currentMapTarget - 1)
				_currentMapIndex++;
			else {
				_characterMoving = false;
				GameManager.CurrentMapPositionIndex = _currentMapIndex;
				_charactorAnimator.SetBool ("GetMessage", true);
			
				Invoke ("FinishWalk", 1.25f);
			}

			return true;
		}
	}

	public IEnumerator StartMoving (float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		_theCameraManager.TargetToFollow = currentPlayer.transform;
		_charactorAnimator.SetBool ("StartWalking", true);
		_characterMoving = true;
	}

	private void FinishWalk ()
	{
		_theGameManager.LoadScene (GameManager.CurrentCharacterType.ToString () + "Frame");

		//SceneManager.LoadScene (GameManager.CurrentCharacterType.ToString () + "Frame");
		//if (GameManager.CurrentCharacterType == CharacterType.Felix)
		//{
		//    if (GameManager.CurrentConvoIndex == 4)
		//    {
		//        SceneManager.LoadScene("Irritation");
		//    }
		//    else
		//    {
		//        SceneManager.LoadScene("TempFrame");
		//    }
		//}

		//if (GameManager.CurrentCharacterType == CharacterType.Isaac)
		//{
		//    if (GameManager.CurrentConvoIndex == 0)
		//    {
		//        SceneManager.LoadScene("RoomEscape");
		//    }
		//    if (GameManager.CurrentConvoIndex == 1)
		//    {
		//        SceneManager.LoadScene("FindingFriends");
		//    }
		//    if (GameManager.CurrentConvoIndex == 2)
		//    {
		//        SceneManager.LoadScene("Irritation");
		//    }
		//}

		//if (GameManager.CurrentCharacterType == CharacterType.Marlon)
		//{
		//    if (GameManager.CurrentConvoIndex == 0)
		//    {
		//        SceneManager.LoadScene("RoomEscape");
		//    }
		//    if (GameManager.CurrentConvoIndex >= 1)
		//    {
		//        SceneManager.LoadScene("TempFrame");
		//    }
		//}
	}

	private void PlayerMapPieceSetup ()
	{
		if (GameManager.CurrentFrame == FrameType.MAPFOLLOW) {
			_theCameraManager.TargetToFollow = currentPlayer.transform;
		}
        
		if (GameManager.CurrentCharacterType == CharacterType.Felix) {
			if (GameManager.CurrentConvoIndex == 0) {
				StartPos.position = FelixMapPaths [0].position;
				_currentMapTarget = 1;
			}
			if (GameManager.CurrentConvoIndex == 3) {
				StartPos.position = FelixMapPaths [1].position;
				_currentMapTarget = 2;
			}
			if (GameManager.CurrentConvoIndex == 5) {
				StartPos.position = FelixMapPaths [2].position;
				_currentMapTarget = 4;
			}
			if (GameManager.CurrentConvoIndex == 7) {
				StartPos.position = FelixMapPaths [4].position;
				_currentMapTarget = 5;
			}

			_currentPlayer = (GameObject)Instantiate (FelixMapPiece, new Vector3 (StartPos.position.x, StartPos.position.y + 0.223f, StartPos.position.z), Quaternion.identity);
		}

		if (GameManager.CurrentCharacterType == CharacterType.Isaac) {
			if (GameManager.CurrentConvoIndex == 0) {
				StartPos.position = IsaacMapPaths [0].position;
				_currentMapTarget = 2;
			}
			if (GameManager.CurrentConvoIndex == 1) {
				StartPos.position = IsaacMapPaths [2].position;
				_currentMapTarget = 4;
			}
			if (GameManager.CurrentConvoIndex == 2) {
				StartPos.position = IsaacMapPaths [4].position;
				_currentMapTarget = 6;
			}
			if (GameManager.CurrentConvoIndex == 4) {
				StartPos.position = IsaacMapPaths [6].position;
				_currentMapTarget = 7;
			}

			_currentPlayer = (GameObject)Instantiate (IsaacMapPiece, new Vector3 (StartPos.position.x, StartPos.position.y + 0.223f, StartPos.position.z), Quaternion.identity);
		}

		if (GameManager.CurrentCharacterType == CharacterType.Marlon) {
			if (GameManager.CurrentConvoIndex == 0) {
				StartPos.position = MarlonMapPaths [0].position;
				_currentMapTarget = 2;
			}
			if (GameManager.CurrentConvoIndex == 2) {
				StartPos.position = MarlonMapPaths [2].position;
				_currentMapTarget = 3;
			}
			if (GameManager.CurrentConvoIndex == 4) {
				StartPos.position = MarlonMapPaths [3].position;
				_currentMapTarget = 4;
			}
			if (GameManager.CurrentConvoIndex == 5) {
				StartPos.position = MarlonMapPaths [4].position;
				_currentMapTarget = 5;
			}

			_currentPlayer = (GameObject)Instantiate (MarlonMapPiece, new Vector3 (StartPos.position.x, StartPos.position.y + 0.223f, StartPos.position.z), Quaternion.identity);
		}
		_charactorAnimator = _currentPlayer.GetComponent<Animator> ();

		foreach (BuildingFadeManager bfm in FindObjectsOfType<BuildingFadeManager>()) {
			if (_currentPlayer) {
				bfm.Player = _currentPlayer.GetComponent<SpriteRenderer> ();
			} else {
				Debug.Log ("The player wasn't set for some reason...");
				break;
			}
		}
		StartCoroutine (StartMoving (1));
	}
}