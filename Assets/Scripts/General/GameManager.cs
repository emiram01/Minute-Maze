using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Maze Generation")]
    [SerializeField] private Vector3 _end;
    [SerializeField] private MazeGenerator _mazePrefab;
    [SerializeField] private GameObject _gem;
    private MazeGenerator _mazeInstance;

    [Header("Player Settings")]
    [SerializeField] private Vector3 _spawn;
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameObject _arms;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 _camPosition;
    [SerializeField] private CameraManager _cam;

    [Header("Timer Settings")]
    [SerializeField] private int _countDownTime;
    [SerializeField] private TextMeshProUGUI _timeUI;
    [SerializeField] private float _currentTime;
    private bool _countdown;
    
    //other
    [SerializeField] private AudioManager _audio;
    [SerializeField] private Animator _anim;
    [SerializeField] private Animator _gameOveranim;
    [SerializeField] private GameObject _gameOverText;
    [SerializeField] private GameObject[] _introText;
    [SerializeField] private InputManager _input;
    [SerializeField] private GameObject _pauseMenu;
    private bool _canPause;
    private bool _paused;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _anim.SetTrigger("End");
        StartCoroutine(nameof(Begin));
    }

    private void Start()
    {
        _currentTime = _countDownTime;
        _timeUI.text = FormatTime(_currentTime);
        _gem.transform.position = _end;
        _canPause = false;
    }

    private void Update()
    {
        if(_canPause && _input.pause)
        {
            if(_paused)
                Resume();
            else
                Pause();
        }

        if(_countdown && _gem && !_gem.GetComponent<MazeEnding>().mazeComplete)
            Timer();
    }

    private void Generation()
    {
        _mazeInstance = Instantiate(_mazePrefab) as MazeGenerator;
        StartCoroutine(_mazeInstance.Generate());
    }

    private IEnumerator Begin()
    {
        _cam.transform.position = _camPosition;
        yield return new WaitForSeconds(2.75f);
        _anim.gameObject.SetActive(false);
        Generation();
        yield return new WaitUntil(() => _mazeInstance.mazeComplete);
        foreach(GameObject go in _introText)
            go.SetActive(false);

        _cam.ChangeProjection();
        _cam.mazeView = false;
        _canPause = true;
        _cam.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        _arms.SetActive(true);
        _player.transform.position = _spawn;
        _player.gameObject.SetActive(true);
        _countdown = true;
    }

    #region Time
    private string FormatTime(float time)
    {
        if(time <= 0)
            time = 0;

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
    
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    private void Timer()
    {
        _currentTime -= 1 * Time.deltaTime;
        _timeUI.text = FormatTime(_currentTime);

        if(_currentTime <= 0)
            StartCoroutine(nameof(GameOver));
    }
    #endregion

    #region States
    private IEnumerator GameOver()
    {
        _countdown = false;
        _audio.Play("gameOver");
        _playerManager.LockPlayer(false);
        _gameOveranim.SetTrigger("GameOver");
        yield return new WaitForSeconds(1f);
        _gameOverText.SetActive(true);
        yield return new WaitForSeconds(2.75f);
        _gem.GetComponent<MazeEnding>().EndGame();
    }

    private void Pause()
    {
        _audio.Play("pause");
        _paused = true;
        Time.timeScale = 0f;
        _cam.canMoveCam = false;
        _pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        _audio.Play("resume");
        _paused = false;
        Time.timeScale = 1f;
        _cam.canMoveCam = true;
        _pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Menu()
    {
        _paused = false;
        _canPause = false;
        StopAllCoroutines();
        _gem.GetComponent<MazeEnding>().EndGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}