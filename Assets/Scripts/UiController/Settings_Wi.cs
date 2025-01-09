using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings_Wi : WindowUi
{
    [SerializeField] private Slider m_slFlex;
    [SerializeField] private Toggle m_tgMusic,m_tgFxs;
    public void Start()
    {
        m_slFlex.value = PlayerPrefs.GetFloat("FlexP", 0);
        m_tgMusic.isOn = PlayerPrefs.GetInt("MusicP", 1) == 1;
        m_tgFxs.isOn = PlayerPrefs.GetInt("FxsP", 1) == 1;

        m_slFlex.onValueChanged.AddListener((x) => PlayerPrefs.SetFloat("FlexP", x));
        m_tgMusic.onValueChanged.AddListener((x) => PlayerPrefs.SetInt("MusicP",x? 1 : 0));
        m_tgFxs.onValueChanged.AddListener((x) => PlayerPrefs.SetInt("FxsP", x ? 1 : 0));
    }
}
