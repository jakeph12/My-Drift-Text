using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_wi : WindowUi
{
    [SerializeField] private Slider m_slR, m_slG, m_slB;
    [SerializeField] private Toggle m_tgSpoiler;
    [SerializeField] private Text m_txCoin;
    [SerializeField] private GameObject m_gmBuy;
    [SerializeField] private Button m_btBuy;
    private Skin_Loader m_scCur;
    private bool m_bBought;
    private int _m_inCuId;
    private int m_inCuId
    {
        get => _m_inCuId;
        set
        {
            _m_inCuId = value;

            if (_m_inCuId > 1) _m_inCuId = 0;
            else if(_m_inCuId < 0) _m_inCuId = 1;
            if (m_inCuId == 1 && !m_bBought)
            {
                m_gmBuy.SetActive(true);
                if (PlayerInfo.m_inCoin < 10000) m_btBuy.interactable = false;
                else m_btBuy.interactable = true;
            }
            else
            {
                m_gmBuy.SetActive(false);
            }



            m_scCur = Main_Menu_Car_Show.m_sinThis.OnIdCh(_m_inCuId);
            m_tgSpoiler.isOn = m_scCur.m_bSpoiler;
            Color cl = m_scCur.GetColor();

            m_slR.value = cl.r;
            m_slG.value = cl.g;
            m_slB.value = cl.b;
        }
    }

    private void Start()
    {
        m_bBought = PlayerPrefs.GetInt("Second", 0) == 1;
        m_inCuId = PlayerInfo.m_inId;
        m_txCoin.text = $"{PlayerInfo.m_inCoin}Kc";
        PlayerInfo.m_evOnCoinCh += OnCoin;
        m_acOnDestroy += () => PlayerInfo.m_evOnCoinCh -= OnCoin;
        m_acOnDestroy += OnEnd;

        m_slR.onValueChanged.AddListener(SetColor);
        m_slG.onValueChanged.AddListener(SetColor);
        m_slB.onValueChanged.AddListener(SetColor);
        m_tgSpoiler.onValueChanged.AddListener((x) => m_scCur.m_bSpoiler = x); 

    }
    public void SetColor(float x)
    {
        m_scCur.SetColor(m_slR.value, m_slG.value, m_slB.value);
        m_scCur.Save(m_slR.value, m_slG.value, m_slB.value);

    }
    public void OnEnd()
    {
        if(m_inCuId == 1 && !m_bBought)
        {
            Main_Menu_Car_Show.m_sinThis.OnIdCh(0);
            return;
        }
        PlayerInfo.m_inId = m_inCuId;
    }
    private void OnCoin(int coin)
    {
        m_txCoin.text = $"{coin}Kc";
    }
    public void Next()
    {
        m_inCuId++;
    }
    public void Priv()
    {
        m_inCuId--;
    }
    public void Buy()
    {
        PlayerPrefs.SetInt("Second", 1);
        m_bBought = true;
        PlayerInfo.m_inCoin -= 10000;
        m_gmBuy.SetActive(false);
    }
    public void GoToMain() => Main_Menu_Controller.OpenMainMenu(1);
}
