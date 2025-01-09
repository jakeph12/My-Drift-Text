using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public delegate void OnIntChange(int am);


    private static int _m_inCoin;
    private static int _m_inId;
    public static event OnIntChange m_evOnCoinCh;
    public static event OnIntChange m_evOnIdCh;
    public static int m_inCoin
    {
        get 
        {
            Init();
            return _m_inCoin;
        }
        set
        {
            _m_inCoin = value;
            PlayerPrefs.SetInt("PlayerCoin", _m_inCoin);
            m_evOnCoinCh?.Invoke(_m_inCoin);
        }
    }
    public static int m_inId
    {
        get
        {
            Init();
            return _m_inId;
        }
        set
        {
            _m_inId = value;
            PlayerPrefs.SetInt("PlayerCar", _m_inId);
            m_evOnIdCh?.Invoke(_m_inId);
        }
    }

    private static bool m_bInit;
    public static void Init()
    {
        if (m_bInit) return;
        m_bInit = true;
        _m_inId = PlayerPrefs.GetInt("PlayerCar", 0);
        _m_inCoin = PlayerPrefs.GetInt("PlayerCoin", 0);

    }
}
