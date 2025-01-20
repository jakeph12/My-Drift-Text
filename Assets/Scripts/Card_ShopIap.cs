using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Card_ShopIap : MonoBehaviour
{
    public Button m_btMain;
    public delegate void PurchaseEvent(Product prod, Action OnComlete,Action UnCom);
    public event PurchaseEvent m_evOnPurchase;
    [SerializeField] private Text m_txPrice,m_txTitle;
    private Product m_prProd;

    public void Purchase()
    {
        m_btMain.interactable = false;
        m_evOnPurchase?.Invoke(m_prProd, OnComlete, UCom);
    }
    public void Init(Product Prod)
    {
        m_prProd = Prod;
        m_txPrice.text = m_prProd.metadata.localizedPrice.ToString();
        m_txTitle.text = m_prProd.metadata.localizedTitle;
    }
    public void OnComlete()
    {
        PlayerInfo.m_IntCoin += (int)m_prProd.definition.payouts.FirstOrDefault().quantity;
        Debug.Log(PlayerInfo.m_IntCoin);
        m_btMain.interactable = true;
    }
    public void UCom()
    {
        m_btMain.interactable = true;
    }
}
