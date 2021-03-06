﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
	public GameObject DialogContainer,
		TypingAnimObj;
	public Transform BottomPoint,
		LeftPoint,
		RightPoint;
	public float TypingDelay;

	public bool NewNotification {
		get {
			return _notification;
		}
	}

	protected int currentConvoIndex {
		get {
			return GameManager.CurrentConvoIndex;
		}
		set {
			GameManager.CurrentConvoIndex = value;
		}
	}

	private MenuController _theMenuController;
	private FrameButtonManager _frameButtMan;
	private GameManager _theGameManager;
	private Vector3 _initialDialogPos = Vector3.zero,
		_initialTopPointPos;
	//This field will only be needed for this SAT dialog test thing
	private float _dialogOffset,
		_delayModifier;
	private int _currentDialogPieceIndex;
	private bool _dialogFinished,
		_dialogActive,
		_typing,
		_notification;

	void Start ()
	{
		_initialTopPointPos = DialogContainer.transform.position;
		_theMenuController = FindObjectOfType<MenuController> ();
		_frameButtMan = FindObjectOfType<FrameButtonManager> ();
		_theGameManager = FindObjectOfType <GameManager> ();
		_notification = true;
	}

	//	void Update ()
	//	{
	//		if (Input.GetKeyDown (KeyCode.Mouse0) && _dialogActive && _theMenuController.LornaDialogueScreen.activeInHierarchy && !_typing) {
	//			if (!_dialogFinished)
	//				LoadPieceOfDialog ();
	//			else {
	//				_dialogActive = false;
	//				_dialogFinished = false;
	//				_theMenuController.ToHomeScreen ();
	//				//LoadMinigame();
	//			}
	//		}
	//	}

	public void ContinueDialog ()
	{
		if (_dialogActive && !_typing)
		if (!_dialogFinished)
			LoadPieceOfDialog ();
		else {
			_dialogActive = false;
			_dialogFinished = false;
			_theMenuController.ToHomeScreen ();
			if (GameManager.FinishedGame)
				StartCoroutine (_theGameManager.EndGame ());
		}


	}

	private IEnumerator DelayedLoadPieceOfDialog (GameObject dialogPieceRenderer, GameObject typingObj)
	{
		yield return new WaitForSeconds (TypingDelay + _delayModifier);
		dialogPieceRenderer.SetActive (true);
		GameObject.Destroy (typingObj);
		_typing = false;
	}

	public void LoadPieceOfDialog ()
	{
		if (_initialDialogPos == Vector3.zero) {
			_initialDialogPos = DialogContainer.transform.position;
			_dialogActive = true;
		}

		//Creates a gameobject by pulling the correct dialog prefab from the resources folder
		GameObject pieceOfDialogToLoad = Resources.Load<GameObject> ("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString () + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex),
		nextPieceOfDialogToLoad = Resources.Load<GameObject> ("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString () + "/" + currentConvoIndex + "/" + (_currentDialogPieceIndex + 1));
		bool lastPieceOfDialog = false;
		if (!pieceOfDialogToLoad) { //The final piece of dialog will be marked with an 'f' this bit lets us know when we're dealing with the last dialog piece

			pieceOfDialogToLoad = Resources.Load<GameObject> ("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString () + "/" + currentConvoIndex + "/" + _currentDialogPieceIndex + "f");
			nextPieceOfDialogToLoad = Resources.Load<GameObject> ("Prefabs/DialogPieces/" + GameManager.CurrentCharacterType.ToString () + "/" + currentConvoIndex + "/" + (_currentDialogPieceIndex + 1) + "f");
			lastPieceOfDialog = true;
		}


		//Instatiates the gameobject at the initial position plus the current offset and puts it inside of the DialogContainer
		if (pieceOfDialogToLoad)
			_dialogOffset += (pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.yMin + 300f);
		else {
			return;
		}


		//Debug.Log("Dialog Offset: " + _dialogOffset);
		if (pieceOfDialogToLoad.tag == "LeftSideDialog") {
			//float typingAnimOffset = _dialogOffset - ((pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.yMax / 2f) / 0.7f) + ((TypingAnimObj.GetComponent<SpriteRenderer> ().sprite.bounds.max.y / 2f) / 0.7f);
			pieceOfDialogToLoad = (GameObject)Instantiate (pieceOfDialogToLoad, new Vector3 (LeftPoint.GetComponent<RectTransform> ().position.x + ((pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.width / 2f) / 0.7f), _initialDialogPos.y - _dialogOffset, _initialDialogPos.z), Quaternion.identity);
			//	GameObject typingObject = (GameObject)Instantiate (TypingAnimObj, new Vector3 (LeftPoint.position.x + ((TypingAnimObj.GetComponent<RectTransform> ().rect.xMax / 2) / 0.7f), _initialDialogPos.y - typingAnimOffset, _initialDialogPos.z), Quaternion.identity);
			//RectTransform renderer = pieceOfDialogToLoad.GetComponent<RectTransform> ();
			//renderer.gameObject.SetActive (true);
			//_delayModifier = ((renderer.rect.xMin - renderer.rect.xMax) + (((renderer.rect.yMax - renderer.rect.yMin) - (TypingAnimObj.GetComponent<SpriteRenderer> ().bounds.max.y - TypingAnimObj.GetComponent<SpriteRenderer> ().bounds.min.y)) * 30)) * Time.deltaTime * 5;
			//	_typing = true;
			//StartCoroutine (DelayedLoadPieceOfDialog (renderer.gameObject, typingObject));
			pieceOfDialogToLoad.transform.SetParent (DialogContainer.transform, true);

			//Disabling auto dialog for now
			//if (nextPieceOfDialogToLoad)
			//{
			//    if (nextPieceOfDialogToLoad.tag == "LeftSideDialog")
			//        Invoke("LoadPieceOfDialog", TypingDelay + _delayModifier);
			//}
		} else if (pieceOfDialogToLoad.tag == "RightSideDialog") {
			pieceOfDialogToLoad = (GameObject)Instantiate (pieceOfDialogToLoad, new Vector3 (RightPoint.GetComponent<RectTransform> ().position.x - (pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.width / 2f / 0.7f), _initialDialogPos.y - _dialogOffset, _initialDialogPos.z), Quaternion.identity);

			//Debug.Log("Rect Rect Position X: " + pieceOfDialogToLoad.GetComponent<RectTransform>().rect.position.x);
			//Vector3 TempLocalPosition = pieceOfDialogToLoad.transform.position;

			pieceOfDialogToLoad.transform.SetParent (DialogContainer.transform, true);
            
			//	pieceOfDialogToLoad.transform.localPosition = new Vector3 (TempLocalPosition.x, pieceOfDialogToLoad.transform.localPosition.y);
			//Disabling auto dialog for now
			//if (nextPieceOfDialogToLoad)
			//{
			//    if (nextPieceOfDialogToLoad.tag == "LeftSideDialog")
			//        Invoke("LoadPieceOfDialog", TypingDelay);
			//}
		} else
			Debug.LogWarning ("You need to tag all dialog pieces with either LeftSideDialog or RightSideDialog.");



		if (pieceOfDialogToLoad.transform.position.y - ((pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.yMax)) < BottomPoint.position.y) {
			float containerOffset = Vector2.Distance (new Vector2 (0, pieceOfDialogToLoad.GetComponent<RectTransform> ().position.y), new Vector2 (0, BottomPoint.GetComponent<RectTransform> ().position.y));
			DialogContainer.transform.position = new Vector3 (DialogContainer.transform.position.x, DialogContainer.transform.position.y + containerOffset, pieceOfDialogToLoad.transform.position.z);
			_dialogOffset -= containerOffset;
		}

		//Calculates how much the next dialog box should be offset
		float ammountToIncreaseOffset = (pieceOfDialogToLoad.GetComponent<RectTransform> ().rect.height / 2f);//Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMax)), Camera.main.ScreenToWorldPoint(new Vector3(0, pieceOfDialogToLoad.GetComponent<SpriteRenderer>().sprite.rect.yMin))) / 2;

		//This ticks forward the index of currenct convo unless it's the last dialog piece in which case...
		if (!lastPieceOfDialog) {
			_currentDialogPieceIndex++;
			_dialogOffset += ammountToIncreaseOffset;// + 0.1f;
	

			_frameButtMan.GameNotification.SetActive (true);

		} else { //We let the dialog manager know the dialog is finished and tick forward the index of which convo you're on.
			_dialogFinished = true;
			_notification = false;
			currentConvoIndex++;
			_frameButtMan.MessageNotification.SetActive (false);
			_frameButtMan.GameIconActive = true;


		}
	}

	//This method is only needed for this SAT dialog test thing.
	private void ClearDialogContainer ()
	{
		_currentDialogPieceIndex++;
		for (int i = 0; i < _currentDialogPieceIndex; i++) {
			GameObject.Destroy (DialogContainer.transform.GetChild (i + 2).gameObject);
		}
		_currentDialogPieceIndex = 0;
		DialogContainer.transform.position = _initialTopPointPos;
		_dialogOffset = 0;
	}
}