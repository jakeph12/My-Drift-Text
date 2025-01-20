using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public delegate void OnIntChange(int am);


    private static int _m_IntCoin;
    private static int _m_IntCurrentId;
    public static event OnIntChange m_EventOnCoinCh;
    public static event OnIntChange m_EventOnIdCh;
    public static int m_IntCoin
    {
        get 
        {
            Init();
            return _m_IntCoin;
        }
        set
        {
            _m_IntCoin = value;
            PlayerPrefs.SetInt("PlayerCoin", _m_IntCoin);
            m_EventOnCoinCh?.Invoke(_m_IntCoin);
        }
    }
    public static int m_IntCurrentId
    {
        get
        {
            Init();
            return _m_IntCurrentId;
        }
        set
        {
            _m_IntCurrentId = value;
            PlayerPrefs.SetInt("PlayerCar", _m_IntCurrentId);
            m_EventOnIdCh?.Invoke(_m_IntCurrentId);
        }
    }

    private static bool m_bInit;
    public static void Init()
    {
        if (m_bInit) return;
        m_bInit = true;
        _m_IntCurrentId = PlayerPrefs.GetInt("PlayerCar", 0);
        _m_IntCoin = PlayerPrefs.GetInt("PlayerCoin", 0);

    }
}
