using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingManager : Singleton<SettingManager>
{
    public LANGUAGE Language => _currentLanguage;
    private LANGUAGE _currentLanguage = LANGUAGE.Ko;
}
