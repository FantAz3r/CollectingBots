using TMPro;
using UnityEngine;

public class ResourcePieceView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ResourceType _textType;

    public ResourceType TextType => _textType;

    public void SetText(string amount)
    {
        _text.text = amount;
    }
}
