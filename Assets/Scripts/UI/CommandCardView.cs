using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class CommandCardView : MonoBehaviour, IBindable<CommandData>
{
    [Header("Header Section")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _name;

    [Header("Stats Section")]
    [SerializeField] private TextMeshProUGUI _clock;
    [SerializeField] private TextMeshProUGUI _range;
    private bool _atkToggle = true;
    [SerializeField] private CanvasGroup _atkGroup;
    [SerializeField] private LayoutElement _atkLayout;
    [SerializeField] private TextMeshProUGUI _penetrationBonus;
    [SerializeField] private TextMeshProUGUI _payload;
    [SerializeField] private TextMeshProUGUI _criticalMultiplier;

    [Header("Effects Section")]
    private bool _effectToggle = true;
    [SerializeField] private CanvasGroup _effectGroup;
    [SerializeField] private LayoutElement _effectLayout;
    [SerializeField] private Transform _effectParent;
    private List<EffectItemView> _effectList = new ();

    public void Bind(CommandData data)
    {
        if (data == null)
        {
            Debug.LogError("Tried to bind but data is null");
            return;
        }

        // TODO: icon 
        _name.text = data.GetName();
        _clock.text = data.GetClock();
        _range.text = data.GetRange();

        if (data.type == CommandType.Attack)
        {
            SetAttackStatsVisible(true);

            _penetrationBonus.text = data.GetPenetrationBonus();
            _payload.text = data.GetPayload();
            _criticalMultiplier.text = data.GetCriticalMultiplier();
        }
        else
        {
            SetAttackStatsVisible(false);
        }

        ClearEffectList();
        if (data.GetEffects() is { } effectArray  && effectArray.Length > 0)
        {
            SetEffectSectionVisible(true);
            for(int i = 0, iMax = effectArray.Length; i<iMax; i++)
            {
                AddEffectItem(effectArray[i]);
            }
        }
        else
        {
            SetEffectSectionVisible(false);
        }
    }

    private void SetAttackStatsVisible(bool visible)
    {
        if (_atkToggle == visible)
            return;

        _atkToggle = visible;
        _atkGroup.alpha = visible ? 1f : 0f;
        _atkLayout.ignoreLayout = !visible;
    }

    private void SetEffectSectionVisible(bool visible)
    {
        if (_effectToggle == visible)
            return;

        _effectToggle = visible;
        _effectGroup.alpha = visible ? 1f : 0f;
        _effectLayout.ignoreLayout = !visible;
    }

    private void ClearEffectList()
    {
        for (int i = 0, iMax = _effectList.Count; i < iMax; i++)
        {
            PoolManager.Instance.ReleaseEffectItem(_effectList[i]);
        }
        _effectList.Clear();
    }

    private void AddEffectItem(string effect)
    {
        var effectItem = PoolManager.Instance.GetEffectItem();
        effectItem.transform.SetParent(_effectParent, false);
        effectItem.SetDesc(effect);
        _effectList.Add(effectItem);
    }
}
