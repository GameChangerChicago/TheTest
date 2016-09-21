using UnityEngine;
using System.Collections;

public class CookieManager : MonoBehaviour
{
    public GameObject[] CookieSprites;
    public float PercentEaten;

    protected int currentCookieIndex
    {
        get
        {
            return _currentCookieIndex;
        }
        set
        {
            if(_currentCookieIndex != value)
            {
                if (value == -1)
                {
                    CookieSprites[5].SetActive(false);

                    foreach(AntController ant in FindObjectsOfType<AntController>())
                    {
                        ant.enabled = false;
                    }

                    _fading = true;
                }
                else
                {
                    for (int i = 0; i < CookieSprites.Length; i++)
                    {
                        if (i != value)
                        {
                            CookieSprites[i].SetActive(false);
                        }
                        else
                        {
                            CookieSprites[i].SetActive(true);
                        }
                    }
                }

                _currentCookieIndex = value;
            }
        }
    }
    private int _currentCookieIndex;

    private GameManager _theGameManager;
    private bool _fading;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(_fading)
            _theGameManager.FadeHandler(false, false, "TempFrame");

        if (PercentEaten > 99.5)
        {
            currentCookieIndex = -1;
        }
        else if(PercentEaten > 90)
        {
            currentCookieIndex = 5;
        }
        else if(PercentEaten > 60)
        {
            currentCookieIndex = 4;
        }
        else if(PercentEaten > 40)
        {
            currentCookieIndex = 3;
        }
        else if (PercentEaten > 25)
        {
            currentCookieIndex = 2;
        }
        else if (PercentEaten > 10)
        {
            currentCookieIndex = 1;
        }
    }
}