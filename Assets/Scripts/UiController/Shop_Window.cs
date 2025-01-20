using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Window : WindowUi
{
    [SerializeField] private Slider m_SliderColorR, m_SliderColorG, m_SliderColorB;
    [SerializeField] private Toggle m_ToggleSpoiler;
    [SerializeField] private Text m_TextCoin;
    [SerializeField] private GameObject m_gmPanelBuy;
    [SerializeField] private Button m_ButtonBuy;
    private Skin_Loader m_CurrentCar;
    private bool m_bBought;
    private int _m_CurentId;
    private int m_CurentId
    {
        get => _m_CurentId;
        set
        {
            _m_CurentId = value;

            if (_m_CurentId > 1) _m_CurentId = 0;
            else if(_m_CurentId < 0) _m_CurentId = 1;
            if (m_CurentId == 1 && !m_bBought)
            {
                m_gmPanelBuy.SetActive(true);
                if (PlayerInfo.m_IntCoin < 10000) m_ButtonBuy.interactable = false;
                else m_ButtonBuy.interactable = true;
            }
            else
            {
                m_gmPanelBuy.SetActive(false);
            }



            m_CurrentCar = Main_Menu_Car_Show.m_sinThis.OnIdCh(_m_CurentId);
            m_ToggleSpoiler.isOn = m_CurrentCar.m_bSpoiler;
            Color cl = m_CurrentCar.GetColor();

            m_SliderColorR.value = cl.r;
            m_SliderColorG.value = cl.g;
            m_SliderColorB.value = cl.b;
        }
    }

    private void Start()
    {
        m_bBought = PlayerPrefs.GetInt("Second", 0) == 1;
        m_CurentId = PlayerInfo.m_IntCurrentId;
        m_TextCoin.text = $"{PlayerInfo.m_IntCoin}Kc";
        PlayerInfo.m_EventOnCoinCh += OnCoin;
        m_SliderColorR.onValueChanged.AddListener(SetColor);
        m_SliderColorG.onValueChanged.AddListener(SetColor);
        m_SliderColorB.onValueChanged.AddListener(SetColor);
        m_ToggleSpoiler.onValueChanged.AddListener((x) => m_CurrentCar.m_bSpoiler = x); 

    }
    public void SetColor(float x)
    {
        m_CurrentCar.SetColor(m_SliderColorR.value, m_SliderColorG.value, m_SliderColorB.value);
        m_CurrentCar.Save(m_SliderColorR.value, m_SliderColorG.value, m_SliderColorB.value);

    }
    public void OnExit()
    {
        if(m_CurentId == 1 && !m_bBought)
        {
            Main_Menu_Car_Show.m_sinThis.OnIdCh(0);
            return;
        }
        PlayerInfo.m_IntCurrentId = m_CurentId;
    }
    private void OnCoin(int coin)
    {
        m_TextCoin.text = $"{coin}Kc";
    }
    public void ChangeCarToNext()
    {
        m_CurentId++;
    }
    public void ChangeCarToPrevious()
    {
        m_CurentId--;
    }
    public void BuyCar()
    {
        PlayerPrefs.SetInt("Second", 1);
        m_bBought = true;
        PlayerInfo.m_IntCoin -= 10000;
        m_gmPanelBuy.SetActive(false);
    }
    public override void OnDestroy()
    {
        PlayerInfo.m_EventOnCoinCh -= OnCoin;
    }
}
