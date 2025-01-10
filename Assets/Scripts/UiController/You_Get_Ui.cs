using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class You_Get_Ui : WindowUi
{
    [SerializeField] private Button m_btAccept,m_bDouble;
    [SerializeField] private Text m_xtMain;
    private int m_inAmount;

    public void Init(int Amount,Action Callback = null)
    {
        m_inAmount = Amount;
        m_xtMain.text = $"You get: {Amount}";
        m_btAccept.onClick.AddListener(() =>
        {
            PlayerInfo.m_inCoin += Amount;
            Callback?.Invoke();
            SceneManager.LoadScene(0);
        });
        m_bDouble.onClick.AddListener(() =>
        {
            Callback?.Invoke();
            if (!Show_inzerce.m_sinThis.ShowRew(() => PlayerInfo.m_inCoin += Amount * 2)) m_bDouble.interactable = false;
            else SceneManager.LoadScene(0);
        });
    }
}
