using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MazeEnding : MonoBehaviour
{
    public bool mazeComplete;
    [SerializeField] private Animator _winAnim;
    [SerializeField] private GameObject _winText;
    [SerializeField] private AudioManager _audio;
    [SerializeField] private PlayerManager _player;

    private void Update()
    {
        this.transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        StartCoroutine(nameof(WinScreen));
        mazeComplete = true;
    }

    private IEnumerator WinScreen()
    {
        _audio.Play("victory");
        _winAnim.SetTrigger("Win");
        _player.LockPlayer(false);
        yield return new WaitForSeconds(1f);
        _winText.SetActive(true);
        yield return new WaitForSeconds(2.75f);
        EndGame();
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
