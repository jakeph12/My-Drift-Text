using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class WindowUi : MonoBehaviour
{
    public Action m_acOnDestroy,m_acOnInit;
    public Action m_acOnSDestroy, m_acOnInitEnd;
    [HideInInspector]
    public Vector2 m_vcStartPos;
    private CanvasGroup m_cvGroop;
    private TweenerCore<float, float, FloatOptions> m_anMain;

    protected virtual void Awake()
    {
        m_cvGroop = GetComponent<CanvasGroup>();
    }

    public virtual void DellObj(float time = 1)
    {
        if (m_anMain != null)
        {
            m_acOnInitEnd?.Invoke();
            m_anMain.Kill();
        }
        m_acOnSDestroy?.Invoke();
        m_cvGroop.DOFade(0, time).OnComplete(() =>
        {
            m_acOnDestroy?.Invoke();
            Destroy(gameObject);

        });

    }
    public virtual void Open(float time = 1)
    {
        m_acOnInit?.Invoke();
        m_cvGroop.alpha = 0;
        m_anMain = m_cvGroop.DOFade(1, time).OnComplete(() =>
        {
            m_acOnInitEnd?.Invoke();
            m_anMain = null;
        });

    }
    public virtual void Init()
    {
        
    }
    public virtual void OpenOtherM(WindowUi wi) => Main_Menu_Controller.m_sinThis.OpenWindow(wi, TypeWindow.Main);
    public virtual void OpenOtherS(WindowUi wi) => Main_Menu_Controller.m_sinThis.OpenWindow(wi, TypeWindow.Additional);
    public virtual void OpenOtherN(WindowUi wi) => Main_Menu_Controller.m_sinThis.OpenWindow(wi, TypeWindow.NoClosed);

}
