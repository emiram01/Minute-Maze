using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Animator _anim;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
    }

    public void Easy()
    {
        StartCoroutine(LoadMaze(1));
    }

    public void Medium()
    {
        StartCoroutine(LoadMaze(2));
    }

    public void Hard()
    {
        StartCoroutine(LoadMaze(3));
    }

    public IEnumerator LoadMaze(int difficulty)
    {
        _anim.SetTrigger("Start");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(difficulty);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
