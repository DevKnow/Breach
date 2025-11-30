using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _desc;

    public void SetDesc(string text)
    {
        _desc.text = $"- {text}";
    }
}
