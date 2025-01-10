using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

public class Iap_Shop_Wi : WindowUi, IStoreListener
{
    private IStoreController m_inteController;
    private IExtensionProvider m_inteExtensions;
    [SerializeField] private Card_ShopIap[] m_CrMain;
    private Action m_acOnComplete,m_acUnCom;

    protected async override void Awake()
    {
        base.Awake();
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
       await UnityServices.InitializeAsync(options);
        ResourceRequest request = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        request.completed += HandleIAPCatalogLoaded;
    }
    void HandleIAPCatalogLoaded(AsyncOperation asy)
    {
        ResourceRequest req = asy as ResourceRequest;
        Debug.Log($"Assset {req.asset}");


        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((req.asset as TextAsset).text);
        Debug.Log($"Loaded {catalog.allProducts.Count}");
#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else 
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif

        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            List<PayoutDefinition> payouts = new List<PayoutDefinition>();
            foreach (var payout in item.Payouts)
            {
                payouts.Add(new PayoutDefinition(
                    payout.type.ToString(),
                    payout.subtype,
                    payout.quantity
                ));
            }

            builder.AddProduct(item.id, item.type, new IDs(), payouts.ToArray());
        }

        UnityPurchasing.Initialize(this, builder);

        List<Product> sorted = m_inteController.products.all.OrderBy(item => item.metadata.localizedPrice).ToList();
        for(int i = 0; i < sorted.Count; i++)
        {
            int t = i;
            m_CrMain[t].Init(sorted[t]);
            m_CrMain[t].m_evOnPurchase += OnPurchase;
            
        }
    }
    private void OnPurchase(Product product, Action callback, Action UnCom)
    {
        m_inteController.InitiatePurchase(product);
        m_acOnComplete = callback;
        m_acUnCom = UnCom;
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_inteController = controller;
        m_inteExtensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        throw new System.NotImplementedException();
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        m_acUnCom?.Invoke();
        m_acUnCom = null;
        m_acOnComplete = null;
        Debug.Log("Canceled");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        m_acOnComplete?.Invoke();
        m_acOnComplete = null;
        m_acUnCom = null;
        return PurchaseProcessingResult.Complete;
    }
}
