using UnityEngine;

public class LavaWall : MonoBehaviour
{
    [SerializeField] private Renderer _lava;
    [SerializeField] private float _speed;

    private void Update()
    {
        if(_lava)
        {
            float offset = Time.time * _speed;
            _lava.material.SetTextureOffset("_BaseMap", new Vector2(offset, 0));
        }
    }
}
    