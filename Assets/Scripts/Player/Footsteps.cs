using UnityEngine;

public class Footsteps : MonoBehaviour
{
    private AudioManager _audio;

    private void Start()
    {
        _audio = FindObjectOfType<AudioManager>();
    }

    public void Left()
    {
        _audio.Play("fs1");
    }

    public void Right()
    {
        _audio.Play("fs2");
    }
    
    public void Jump()
    {
        _audio.Play("jump");
    }
}
