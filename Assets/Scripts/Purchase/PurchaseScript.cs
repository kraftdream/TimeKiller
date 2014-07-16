using OnePF;
using UnityEngine;

public class PurchaseScript : MonoBehaviour
{
    private const string HEALTH = "health_pack";

    [SerializeField]
    private GameEntity _player;
    public GameEntity Player
    {
        get { return _player; }
        set { _player = value; }
    }

    void Start()
    {
        PurchaseInitializer.Init();
    }

    private void OnEnable()
    {
        OpenIABEventManager.billingSupportedEvent += billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }

    private void OnDisable()
    {
        OpenIABEventManager.billingSupportedEvent -= billingSupportedEvent;
        OpenIABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        OpenIABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        OpenIABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        OpenIABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        OpenIABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        OpenIABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        OpenIABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }

    public void BuyItem()
    {
        OpenIAB.purchaseProduct(HEALTH, "");
    }

    private void billingSupportedEvent()
    {
    }
    private void billingNotSupportedEvent(string error)
    {
    }
    private void queryInventorySucceededEvent(Inventory inventory)
    {
    }
    private void queryInventoryFailedEvent(string error)
    {
    }
    private void purchaseSucceededEvent(Purchase purchase)
    {
        if (_player != null)
        {
            _player.GetComponent<HeroControll>().PlayerRevive();
             OpenIAB.consumeProduct(purchase);
        }
        else
        {
            _player = GameObject.FindWithTag("Player").GetComponent<GameEntity>();
             _player.GetComponent<HeroControll>().PlayerRevive();
            OpenIAB.consumeProduct(purchase);
        }

        Mixpanel.SendEvent(AnalyticsEvents.PURCHASE_HEALTH);
    }
    private void purchaseFailedEvent(int errorCode, string errorMessage)
    {
    }
    private void consumePurchaseSucceededEvent(Purchase purchase)
    {
    }
    private void consumePurchaseFailedEvent(string error)
    {
    }
}
