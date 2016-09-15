using UnityEngine;
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

    protected GameObject currentPlayer
    {
        get
        {
            switch(GameManager.CurrentCharacterType)
            {
                case CharacterType.Felix:
                    _currentPlayer = FelixMapPiece;
                    _currentMapPaths = FelixMapPaths;
                    break;
                case CharacterType.Isaac:
                    _currentPlayer = IsaacMapPiece;
                    _currentMapPaths = IsaacMapPaths;
                    break;
                case CharacterType.Marlon:
                    _currentPlayer = MarlonMapPiece;
                    _currentMapPaths = MarlonMapPaths;
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
    private CameraManager _theCameraManager;
    private FrameController _theFrameController;
    private int _currentMapIndex = 1,
                _currentMapTarget = 3;
    private bool _characterMoving;

    void Start()
    {
        _theGameManager = FindObjectOfType<GameManager>();
        _theCameraManager = FindObjectOfType<CameraManager>();
        _theFrameController = FindObjectOfType<FrameController>();
        currentPlayer.transform.position = new Vector3(_currentMapPaths[GameManager.CurrentMapPositionIndex].position.x, _currentMapPaths[GameManager.CurrentMapPositionIndex].position.y + 0.7f, 0);

        PlayerMapPieceSetup();
    }
    
    void Update()
    {
        if(_characterMoving)
        {
            MapPieceMovementHandler();
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            ProceedToMapPoint(5);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            ProceedToMapPoint(7);
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("Prioritizing");
        }
    }

    private bool MapPieceMovementHandler()
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
            else
            {
                _characterMoving = false;
                GameManager.CurrentMapPositionIndex = _currentMapIndex;
                _theFrameController.ChangeFrame(3);
            }

            return true;
        }
    }

    public void ProceedToMapPoint(int newMapPointTarget)
    {
        _currentMapTarget = newMapPointTarget;
    }

    public IEnumerator StartMoving(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _theCameraManager.TargetToFollow = currentPlayer.transform;
        _characterMoving = true;

    }

    private void PlayerMapPieceSetup()
    {
        if (GameManager.CurrentFrame == FrameType.MAPFOLLOW)
        {
            _theCameraManager.TargetToFollow = currentPlayer.transform;
        }

        GameObject player = null;
        if (GameManager.CurrentCharacterType == CharacterType.Felix)
        {
            //if(GameManager.CurrentConvoIndex < )
            player = (GameObject)Instantiate(FelixMapPiece, StartPos.position, Quaternion.identity);
        }

        if (GameManager.CurrentCharacterType == CharacterType.Isaac)
        {
            player = (GameObject)Instantiate(IsaacMapPiece, StartPos.position, Quaternion.identity);
        }

        if (GameManager.CurrentCharacterType == CharacterType.Marlon)
        {
            player = (GameObject)Instantiate(MarlonMapPiece, StartPos.position, Quaternion.identity);
        }
        _currentPlayer = player;

        foreach (BuildingFadeManager bfm in FindObjectsOfType<BuildingFadeManager>())
        {
            if (player)
            {
                bfm.Player = player.GetComponent<SpriteRenderer>();
            }
            else
            {
                Debug.Log("The player wasn't set for some reason...");
                break;
            }
        }
    }
}