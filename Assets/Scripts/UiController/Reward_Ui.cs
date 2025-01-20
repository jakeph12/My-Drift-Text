using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward_Ui : WindowUi
{
    [SerializeField] private Button m_ButtonAccept,m_ButtonDouble;
    [SerializeField] private Text m_TextAmountReward;
    private int m_inAmount;

    public void Init(int Amount,Action Callback = null)
    {
        m_inAmount = Amount;
        m_TextAmountReward.text = $"You get: {Amount}";
        m_ButtonAccept.onClick.AddListener(() =>
        {
            PlayerInfo.m_IntCoin += Amount;
            Callback?.Invoke();
            Screen_Loader.Load_Async_Scene(0);
        });
        m_ButtonDouble.onClick.AddListener(() =>
        {
            Callback?.Invoke();
            if (!Show_inzerce.m_sinThis.ShowRew(() => PlayerInfo.m_IntCoin += Amount * 2)) m_ButtonDouble.interactable = false;
            else Screen_Loader.Load_Async_Scene(0);
        });
    }
}
