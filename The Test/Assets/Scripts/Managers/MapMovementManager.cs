using UnityEngine;
using System.Collections;

public class MapMovementManager : MonoBehaviour
{
    public GameObject RedPlayerPeg,
                      GreenPlayerPeg,
                      PurplePlayerPeg;
    public Transform[] RedMapPaths,
                       GreenMapPaths,
                       PurpleMapPaths;

    protected GameObject currentPlayer
    {
        get
        {
            switch(_theGameManager.CurrentCharacterType)
            {
                case CharacterType.RED:
                    _currentPlayer = RedPlayerPeg;
                    _currentMapPaths = RedMapPaths;
                    break;
                case CharacterType.GREEN:
                    _currentPlayer = GreenPlayerPeg;
                    _currentMapPaths = GreenMapPaths;
                    break;
                case CharacterType.PURPLE:
                    _currentPlayer = PurplePlayerPeg;
                    _currentMapPaths = PurpleMapPaths;
                    break;
                default:
                    Debug.LogError("Do we have more than three character types? Check that.");
                    break;
            }    

            return _currentPlayer;
        }
    }
    private GameObject _currentPlayer;
    private Transform[] _currentMapPaths;

    private GameManager _theGameManager;
    private int _currentMapIndex = 1,
                _currentMapTarget = 3;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
    }
    
    void Update()
    {
        //Debug.Log(PegMovementHandler());
        PegMovementHandler();

        if(Input.GetKeyDown(KeyCode.O))
        {
            ProceedToMapPoint(5);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            ProceedToMapPoint(7);
        }
    }

    private bool PegMovementHandler()
    {
        if (Vector3.Distance(new Vector3(currentPlayer.transform.position.x, currentPlayer.transform.position.y, 0),
            new Vector3(_currentMapPaths[_currentMapIndex].position.x, _currentMapPaths[_currentMapIndex].position.y + 0.7f, 0)) > 0.1f)
        {
            currentPlayer.transform.Translate(new Vector3((_currentMapPaths[_currentMapIndex].position.x - _currentMapPaths[_currentMapIndex - 1].position.x) * Time.deltaTime,
                                                         ((_currentMapPaths[_currentMapIndex].position.y + 0.7f) - (_currentMapPaths[_currentMapIndex - 1].position.y + 0.7f)) * Time.deltaTime, 0));
            
            return false;
        }
        else
        {
            if (_currentMapIndex < _currentMapTarget)
                _currentMapIndex++;

            return true;
        }
    }

    public void ProceedToMapPoint(int newMapPointTarget)
    {
        _currentMapTarget = newMapPointTarget;
    }
}