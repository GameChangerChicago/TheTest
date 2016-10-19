using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CollectablesManager : MonoBehaviour
{
	public Transform CoinCollectionSpot;
	public Animator FirstDigit,
		SecondDigit;

	protected int coinCount {
		get {
			return _coinCount;
		}
		set {
			StartCoroutine ("UpdateCoinCounter", value);

			_coinCount = value;
		}
	}

	private int _coinCount;

	private List<Transform> _coinsToMove = new List<Transform> ();
	private List<float> _initalDistances = new List<float> ();
	private GameManager _theGameManager;
	private Transform _bloom;
	private bool _gemCollected;

	void Start ()
	{
		_theGameManager = FindObjectOfType<GameManager> ();
	}

	void Update ()
	{
		if (_coinsToMove.Count > 0) {
			CoinMovementHandler ();
		}

		//UPDATE: Added to handle new LoadScene Method
		if (_gemCollected) {
			//_gemCollected = false; 
			GameEndHandler ();
		}
	}

	private void CoinMovementHandler ()
	{
		for (int i = 0; i < _coinsToMove.Count; i++) {
			//Debug.Log("Coins to move: " + _coinsToMove);
			//Debug.Log("Coin Collection Spot: " + CoinCollectionSpot);
			float distance = Vector2.Distance (_coinsToMove [i].position, CoinCollectionSpot.position),
			distanceDifference = _initalDistances [i] - distance;
            
			if (distanceDifference > 1)
				_coinsToMove [i].Translate ((CoinCollectionSpot.position.x - _coinsToMove [i].position.x) * distanceDifference * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove [i].position.y) * distanceDifference * Time.deltaTime, 0);
			else
				_coinsToMove [i].Translate ((CoinCollectionSpot.position.x - _coinsToMove [i].position.x) * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove [i].position.y) * Time.deltaTime, 0);

			if (distance < 0.5f) {
				GameObject objectToDestroy = _coinsToMove [i].gameObject;
				_coinsToMove.Remove (_coinsToMove [i].transform);
				_initalDistances.Remove (_initalDistances [i]);
				GameObject.Destroy (objectToDestroy);
			}
		}
	}

	//ASHLYN: This method handles scene change if you get the diamond in the submarine game.
	private void GameEndHandler ()
	{
		if (_bloom.localScale.x < 80)
			_bloom.localScale = new Vector3 (_bloom.localScale.x + ((25 + _bloom.localScale.x) * Time.deltaTime), _bloom.localScale.y + ((25 + _bloom.localScale.y) * Time.deltaTime), _bloom.localScale.z);
		else {
			_gemCollected = false;
			if (GameManager.CurrentCharacterType == CharacterType.Felix && GameManager.CurrentConvoIndex == 3) {
				_theGameManager.LoadScene ("MapScene", new Color (255, 255, 255, 0));//_theGameManager.FadeHandler(false, true, "MapScene");
			} else {
				_theGameManager.LoadScene (GameManager.CurrentCharacterType.ToString () + "Frame", new Color (255, 255, 255, 0));// _theGameManager.FadeHandler(false, true, GameManager.CurrentCharacterType.ToString() + "Frame");
			}
		}
	}

	private IEnumerator UpdateCoinCounter (int value)
	{
		yield return new WaitForSeconds (0.75f);

		int firstDigit = value % 10,
		secondDigit = value / 10;

		FirstDigit.SetInteger ("number", firstDigit);
		SecondDigit.SetInteger ("number", secondDigit);
	}

	public void CoinCollected (GameObject coin)
	{
		coinCount++;
		_coinsToMove.Add (coin.transform);
		_initalDistances.Add (Vector2.Distance (coin.transform.position, CoinCollectionSpot.position));
	}

	public void GemCollected (GameObject gem)
	{
		_gemCollected = true;
		_bloom = GameObject.Find ("Bloom").transform;

		GameManager.MessageIconActive = true;

		if (GameManager.CurrentCharacterType == CharacterType.Marlon) {
			GameManager.MapIconActive = true;
		}

		if (GameManager.CurrentCharacterType.ToString () == "Felix") {
			GameManager.CurrentConvoIndex = 3;
		}
	}
}