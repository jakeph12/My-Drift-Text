using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public enum TypeWindow
{
    Main,
    Additional,
    NoClosed,
}

public class Main_Menu_Controller : MonoBehaviour
{
    public static Main_Menu_Controller m_sinThis; 


    [SerializeField]
    private GameObject m_gmMainPanel,m_gmAddintionPanel,m_gmNoClosedPanel;
    private GameObject m_gmCurMain,m_gmCurAdditional;
    [SerializeField]
    private WindowUi m_gmMainMenu;
    [SerializeField]
    private GameObject m_gmBlock;


    public void Awake()
    {
        m_sinThis = this;
        m_gmBlock.SetActive(false);
    }

    public void Start()
    {
        if(m_gmMainMenu)
            OpenWindow(m_gmMainMenu,TypeWindow.Main,0f);
    }

    public GameObject OpenWindow(WindowUi gm,TypeWindow typeW, float time = 1f)
    {
        GameObject NewObj;

        if(time > 0)
            SetBlock(time*0.8f).Forget();

        switch (typeW)
        {
            case TypeWindow.Main:
                if(m_gmCurMain)
                    m_gmCurMain.GetComponent<WindowUi>().DellObj();
                NewObj = Instantiate(gm.gameObject, m_gmMainPanel.transform);
                m_gmCurMain = NewObj;
                break;

            case TypeWindow.Additional:
                if (m_gmCurAdditional)
                    m_gmCurAdditional.GetComponent<WindowUi>().DellObj();
                NewObj = Instantiate(gm.gameObject, m_gmAddintionPanel.transform);
                m_gmCurAdditional = NewObj;
                break;

            case TypeWindow.NoClosed:
                NewObj = Instantiate(gm.gameObject, m_gmNoClosedPanel.transform);
                break;
            default:
                NewObj = Instantiate(gm.gameObject, m_gmCurMain.transform);
                break;

        }

        NewObj.GetComponent<WindowUi>().m_vcStartPos = NewObj.transform.localPosition;
        NewObj.GetComponent<WindowUi>().Open(time);

        return NewObj;
    }
    public void CloseAll(TypeWindow typeW)
    {

        switch (typeW)
        {
            case TypeWindow.Main:
                if (m_gmCurMain)
                    m_gmCurMain.GetComponent<WindowUi>().DellObj();
                break;

            case TypeWindow.Additional:
                if (m_gmCurAdditional)
                    m_gmCurAdditional.GetComponent<WindowUi>().DellObj();
                break;

            case TypeWindow.NoClosed:
                var c = m_gmNoClosedPanel.transform.childCount;

                if (c > 0)
                    for(int i = 0;i < c; i++)
                        m_gmNoClosedPanel.GetComponentInChildren<WindowUi>().DellObj();

                break;
            default:
                break;

        }
    }

    public static void OpenMainMenu(float time) => m_sinThis.OpenWindow(m_sinThis.m_gmMainMenu,TypeWindow.Main,time);

    private CancellationTokenSource m_caToken;

    public static async UniTask SetBlock(float Time)
    {
        if (m_sinThis.m_caToken != null) m_sinThis.m_caToken.Cancel();
        m_sinThis.m_caToken = new CancellationTokenSource();
        m_sinThis.m_gmBlock.SetActive(true);

        try
        {
            await UniTask.Delay((int)(Time * 1000),cancellationToken: m_sinThis.m_caToken.Token);
        }
        catch(Exception ex)
        {
            Debug.Log(ex);
        }
        m_sinThis.m_caToken.Dispose();
        m_sinThis.m_caToken = null;
        m_sinThis.m_gmBlock.SetActive(false);
    }

}
