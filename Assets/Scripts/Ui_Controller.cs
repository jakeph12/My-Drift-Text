using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.ComponentModel;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Controller : MonoBehaviour
{
    public static Ui_Controller m_staticThis;
    public Text m_txMainScore, m_txMainTimer;
    public Text m_txTotalScore;
    public GameObject m_gmSpeed;
    private int _m_inCoin;
    public int m_inCoin
    {
        get => _m_inCoin;
        set
        {
            _m_inCoin = value;
            if(m_txTotalScore)
                m_txTotalScore.gameObject.SetActive(true);
            m_txTotalScore.text = $"Total score: {_m_inCoin}";
        }
    }



    private void Awake()
    {
        m_staticThis = this;
    }
    private bool Inited = false;
    public void Start()
    {
        TimeAndDriftCounter.m_staticThis.m_EventOnDrift += OnDrift;
        TimeAndDriftCounter.m_staticThis.m_EventOnDriftEnd += OnDriftEnd;
        TimeAndDriftCounter.m_staticThis.m_EventOnTimeChange += OnTimeChange;
        Main_Car_Controller.m_staticThis.m_EventOnSpeedCh += OnSpeedChange;
        Inited = true;


    }
    public void OnDrift(int Amount)
    {
        m_txMainScore.gameObject.SetActive(true);
        if ((int)(Amount / 120) % 2 == 0)
        {
            m_txMainScore.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.3f).OnComplete(() =>
            {
                m_txMainScore.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
            });
        }
        m_txMainScore.text = $"Your Score:{Amount}";
    }
    public void OnDriftEnd(int Amount)
    {
        m_inCoin += Amount;
        m_txMainScore.gameObject.SetActive(false);
    }
    public void OnSpeedChange(float speed)
    {
        m_gmSpeed.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -276 * speed));
        //m_txMainSpeed.text = $"{(int)(130 * _m_flSpeed)}";
    }
    public void OnTimeChange(Vector2Int time)
    {
        if (time.y >= 10)
            m_txMainTimer.text = $"Time: {time.x}:{time.y}";
        else
            m_txMainTimer.text = $"Time: {time.x}:0{time.y}";
    }
    public void OnDestroy()
    {
        if (!Inited) return;
        TimeAndDriftCounter.m_staticThis.m_EventOnDrift -= OnDrift;
        TimeAndDriftCounter.m_staticThis.m_EventOnDriftEnd -= OnDriftEnd;
        TimeAndDriftCounter.m_staticThis.m_EventOnTimeChange -= OnTimeChange;
        Main_Car_Controller.m_staticThis.m_EventOnSpeedCh -= OnSpeedChange;
    }

}
