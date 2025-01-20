using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Screen_Loader : MonoBehaviour
{
    private static Screen_Loader m_staticThis;
    public static bool m_bActiveScreenOnLoad = false;
    [SerializeField] private Image m_Spinner;
    [SerializeField] private CanvasGroup m_CanvasCurrent;

    private void Awake()
    {
        m_staticThis = this;
        if (!m_bActiveScreenOnLoad)
        {
            m_CanvasCurrent.gameObject.SetActive(false);
            m_CanvasCurrent.alpha = 0;
        }
        else
        {
            m_CanvasCurrent.gameObject.SetActive(true);
            m_CanvasCurrent.DOFade(0, 1).OnComplete(() =>
            {
                m_CanvasCurrent.gameObject.SetActive(false);
            });
        }
    }

    public async static void Load_Async_Scene(int id)
    {
        m_staticThis.m_CanvasCurrent.gameObject.SetActive(true);
        m_bActiveScreenOnLoad = true;
        var t = SceneManager.LoadSceneAsync(id);
        t.allowSceneActivation = false;

        await m_staticThis.m_CanvasCurrent.DOFade(1, 1).AsyncWaitForCompletion();

        while (!t.isDone)
        {
            await UniTask.Delay(1000);
            m_staticThis.m_Spinner.fillAmount = t.progress +0.1f;
            if ((t.progress + 0.11f) >= 1f)
            {
                t.allowSceneActivation = true;
                Debug.Log("Done");
                break;
            }
            Debug.Log(t.progress + 0.1f);
        }
    }

}
