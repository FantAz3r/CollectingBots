using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    public void SetColor(Color color)
    {
        _renderer.material.color = color;
    }
}
