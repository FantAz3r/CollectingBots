using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceViewer : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private List<ResourcePieceView> _resourceAmount;
    [SerializeField] private TMP_Text _totalAmountText;

    private void OnEnable()
    {
        _storage.ResourceChanged += OnResourceChanged;
        _storage.TotalAmountChanged += OnTotalAmountChanged;
    }

    private void OnDisable()
    {
        _storage.ResourceChanged -= OnResourceChanged;
        _storage.TotalAmountChanged -= OnTotalAmountChanged;
    }

    private void OnResourceChanged(Dictionary<ResourceType, int> resourses)
    {
        foreach (var text in _resourceAmount)
        {
            if (resourses.ContainsKey(text.TextType))
            {
                text.SetText(resourses[text.TextType].ToString());
            }
        }
    }

    private void OnTotalAmountChanged(int total)
    {
        if (_totalAmountText != null)
        {
            _totalAmountText.text = total.ToString();
        }
    }
}
