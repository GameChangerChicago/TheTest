﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CollectablesManager : MonoBehaviour
{
    public Transform CoinCollectionSpot;
    public Image CoinCounter;

    protected int coinCount
    {
        get
        {
            return _cointCount;
        }
        set
        {
            _cointCount = value;
        }
    }
    private int _cointCount;

    private List<Transform> _coinsToMove = new List<Transform>();
    private List<float> _initalDistances = new List<float>();
    private GameManager _theGameManager;
    private Transform _bloom;
    private bool _gemCollected;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(_coinsToMove.Count > 0)
        {
            CoinMovementHandler();
        }

        if (_gemCollected)
        {
            GameEndHandler();
        }
    }

    private void CoinMovementHandler()
    {
        for (int i = 0; i < _coinsToMove.Count; i++)
        {
            //Debug.Log("Coins to move: " + _coinsToMove);
            //Debug.Log("Coin Collection Spot: " + CoinCollectionSpot);
            float distance = Vector2.Distance(_coinsToMove[i].position, CoinCollectionSpot.position),
                  distanceDifference = _initalDistances[i] - distance;
            
            if (distanceDifference > 1)
                _coinsToMove[i].Translate((CoinCollectionSpot.position.x - _coinsToMove[i].position.x) * distanceDifference * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove[i].position.y) * distanceDifference * Time.deltaTime, 0);
            else
                _coinsToMove[i].Translate((CoinCollectionSpot.position.x - _coinsToMove[i].position.x) * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove[i].position.y) * Time.deltaTime, 0);

            if (distance < 0.5f)
            {
                GameObject objectToDestroy = _coinsToMove[i].gameObject;
                _coinsToMove.Remove(_coinsToMove[i].transform);
                _initalDistances.Remove(_initalDistances[i]);
                GameObject.Destroy(objectToDestroy);
            }
        }
    }

    //ASHLYN: This method handles scene change if you get the diamond in the submarine game.
    private void GameEndHandler()
    {
        if (_bloom.localScale.x < 80)
            _bloom.localScale = new Vector3(_bloom.localScale.x + ((25 + _bloom.localScale.x) * Time.deltaTime), _bloom.localScale.y + ((25 + _bloom.localScale.y) * Time.deltaTime), _bloom.localScale.z);
        else
        {
            //This bit is kinda tricky. Basically in this one situation we want to reset the temp frame. If ever you want to reset the frame (basically, make it so the player can select a character).
            if (GameManager.CurrentCharacterType == CharacterType.Marlon && GameManager.CurrentConvoIndex == 6)
            {
                GameManager.CharacterSelected = false;
            }

            if (GameManager.CurrentCharacterType == CharacterType.Felix && GameManager.CurrentConvoIndex == 3)
            {
                _theGameManager.FadeHandler(false, true, "MapScene");
            }
            else
            {
                _theGameManager.FadeHandler(false, true, "TempFrame");
            }
        }
    }

    public void CoinCollected(GameObject coin)
    {
        coinCount++;
        _coinsToMove.Add(coin.transform);
        _initalDistances.Add(Vector2.Distance(coin.transform.position, CoinCollectionSpot.position));
    }

    public void GemCollected(GameObject gem)
    {
        _gemCollected = true;
        _bloom = GameObject.Find("Bloom").transform;

        if(GameManager.CurrentCharacterType.ToString() == "Felix")
        {
            GameManager.CurrentConvoIndex = 3;
        }
    }
}