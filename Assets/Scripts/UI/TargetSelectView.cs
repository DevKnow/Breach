using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 타겟 선택 UI - Func 활용 예시
/// </summary>
public class TargetSelectView : MonoBehaviour
{
    [SerializeField] private GameObject _targetButtonPrefab;
    [SerializeField] private Transform _targetButtonContainer;

    private List<Button> _targetButtons = new();

    /// <summary>
    /// 타겟 선택 완료 시 발생하는 이벤트
    /// </summary>
    public event Action<ProgramModel> OnTargetSelected;

    /// <summary>
    /// 타겟 선택 UI 표시
    /// </summary>
    /// <param name="candidates">모든 후보</param>
    /// <param name="filter">필터 조건 (Func 사용)</param>
    public void ShowTargets(List<ProgramModel> candidates, Func<ProgramModel, bool> filter)
    {
        // 기존 버튼 정리
        ClearButtons();

        // Func<ProgramModel, bool> 설명:
        // - ProgramModel을 받아서 bool을 반환하는 함수
        // - true 반환하면 유효한 타겟, false면 제외

        for (int i = 0, iMax = candidates.Count; i < iMax; i++)
        {
            var candidate = candidates[i];

            // filter 함수 호출 - 조건에 맞는지 확인
            if (!filter(candidate))
                continue;

            CreateTargetButton(candidate);
        }

        gameObject.SetActive(true);
    }

    /// <summary>
    /// 사용 예시들
    /// </summary>
    public void ExampleUsages(List<ProgramModel> all, ProgramModel actor, CommandModel command)
    {
        // 예시 1: 적만 선택 가능
        ShowTargets(all, (target) => target.Team == Team.ENEMY);

        // 예시 2: 아군만 선택 가능 (힐 스킬 등)
        ShowTargets(all, (target) => target.Team == Team.PLAYER);

        // 예시 3: 자기 자신 제외
        ShowTargets(all, (target) => target != actor);

        // 예시 4: 복합 조건 - 적이면서 사거리 내
        ShowTargets(all, (target) =>
        {
            if (target.Team != Team.ENEMY)
                return false;

            // 거리 계산은 나중에 구현
            // return command.IsInRange(distance);
            return true;
        });

        // 예시 5: 메서드 참조
        ShowTargets(all, IsValidTarget);
    }

    private bool IsValidTarget(ProgramModel target)
    {
        // Func<ProgramModel, bool> 시그니처와 일치
        return target.Team == Team.ENEMY && target.CurrentIntegrity > 0;
    }

    private void CreateTargetButton(ProgramModel target)
    {
        var buttonObj = Instantiate(_targetButtonPrefab, _targetButtonContainer);
        var button = buttonObj.GetComponent<Button>();

        // 버튼 텍스트 설정
        var text = buttonObj.GetComponentInChildren<Text>();
        if (text != null)
        {
            text.text = target.GetID();
        }

        // 클릭 이벤트 등록
        button.onClick.AddListener(() =>
        {
            // 이벤트 발생 - 구독자에게 선택된 타겟 전달
            OnTargetSelected?.Invoke(target);
            Hide();
        });

        _targetButtons.Add(button);
    }

    private void ClearButtons()
    {
        for (int i = 0, iMax = _targetButtons.Count; i < iMax; i++)
        {
            Destroy(_targetButtons[i].gameObject);
        }
        _targetButtons.Clear();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
