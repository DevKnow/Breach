using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyView : MonoBehaviour
{
    [Header("Program Select")]
    [SerializeField] private GameObject _programSelectPanel;
    [SerializeField] private Transform _programButtonContainer;
    [SerializeField] private GameObject _programButtonPrefab;

    [Header("Module Select")]
    [SerializeField] private GameObject _moduleSelectPanel;
    [SerializeField] private Transform _moduleButtonContainer;
    [SerializeField] private GameObject _moduleButtonPrefab;
    [SerializeField] private TMP_Text _moduleSelectCountText;

    // 선택된 프로그램
    private string _selectedProgramId;

    // 선택된 모듈들
    private List<string> _selectedModuleIds = new();
    private const int MAX_MODULE_SELECT = 2;

    // 이벤트: 선택 완료 시 발생
    public event Action<string, List<string>> OnSelectionComplete;

    private SaveManager _saveManager;

    public void Initialize(SaveManager saveManager)
    {
        _saveManager = saveManager;
        ShowProgramSelect();
    }

    #region Program Select

    private void ShowProgramSelect()
    {
        _programSelectPanel.SetActive(true);
        _moduleSelectPanel.SetActive(false);

        // TODO: 기존 버튼들 제거
        // TODO: 해금된 프로그램 목록 가져오기
        // TODO: 각 프로그램마다 버튼 생성
        // TODO: 잠긴 프로그램은 비활성화 표시
    }

    private void OnProgramButtonClicked(string programId)
    {
        // TODO: 선택한 프로그램 저장
        // TODO: 모듈 선택 화면으로 이동
    }

    #endregion

    #region Module Select

    private void ShowModuleSelect()
    {
        _programSelectPanel.SetActive(false);
        _moduleSelectPanel.SetActive(true);

        _selectedModuleIds.Clear();
        UpdateModuleCountText();

        // TODO: 기존 버튼들 제거
        // TODO: BasicPool에서 4개 랜덤 모듈 가져오기
        // TODO: 각 모듈마다 버튼 생성
    }

    private void OnModuleButtonClicked(string moduleId, Button button)
    {
        // TODO: 이미 선택된 모듈이면 선택 해제
        // TODO: 아니면 선택 추가 (최대 2개)
        // TODO: 버튼 비주얼 업데이트
        // TODO: 2개 선택 완료되면 자동으로 다음 단계? 또는 확인 버튼?
    }

    private void UpdateModuleCountText()
    {
        // TODO: "모듈 선택 (0/2)" 형식으로 텍스트 업데이트
    }

    private void OnConfirmButtonClicked()
    {
        // TODO: 2개 선택 안 됐으면 리턴
        // TODO: OnSelectionComplete 이벤트 발생
    }

    #endregion
}
