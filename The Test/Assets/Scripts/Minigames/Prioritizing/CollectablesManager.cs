using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class CollectablesManager : MonoBehaviour
{
    public Transform CoinCollectionSpot,
                     Bloom;
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
    private bool _gemCollected;

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
            float distance = Vector2.Distance(_coinsToMove[i].position, CoinCollectionSpot.position),
                  distanceDifference = _initalDistances[i] - distance;
            
            if (distanceDifference > 1)
                _coinsToMove[i].Translate((CoinCollectionSpot.position.x - _coinsToMove[i].position.x) * distanceDifference * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove[i].position.y) * distanceDifference * Time.deltaTime, 0);
            else
                _coinsToMove[i].Translate((CoinCollectionSpot.position.x - _coinsToMove[i].position.x) * Time.deltaTime, (CoinCollectionSpot.position.y - _coinsToMove[i].position.y) * Time.deltaTime, 0);

            if (distance < 0.3f)
            {
                GameObject objectToDestroy = _coinsToMove[i].gameObject;
                _coinsToMove.Remove(_coinsToMove[i].transform);
                _initalDistances.Remove(_initalDistances[i]);
                GameObject.Destroy(objectToDestroy);
            }
        }
    }

    private void GameEndHandler()
    {
        if (Bloom.localScale.x < 75)
            Bloom.localScale = new Vector3(Bloom.localScale.x + ((25 + Bloom.localScale.x) * Time.deltaTime), Bloom.localScale.y + ((25 + Bloom.localScale.y) * Time.deltaTime), Bloom.localScale.z);
        else
            SceneManager.LoadScene("Frame");

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
    }
}