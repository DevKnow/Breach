using UnityEngine;
using TMPro;

public class PatchItemView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _desc;

    public void SetDesc(string desc)
    {
        _desc.text = desc;
    }
}