using System.Collections.Generic;
using OnePF;
using UnityEngine;
using System.Collections;

public class PurchaseScript : MonoBehaviour
{
    private const string HEALTH = "health_pack";
    private const string public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmTFGKYq5TXsik57CDmtRj8ayQ466PKKXlEgjFbL6iK5rb+uB8ezAXm4sFspc9TjE8PXd7Y72XIdab4mSgtvU8KovUatA99OhoQ1oEXtb8SPB6YKczDlS6xvs0xwiTlOkttAYfQpIC2T+H35i2OTZu2M9sjCSnECXiixyKKJsqMz4H1i+J6PPCcrQSy1VAeym/oyR4YtHQwefc6m27P2uWdrFCfVYfPt1qSzD4m9THkRWJSLnMGFsAYTGj4KjQJXKVb4C/HXZes5/1XfK3b0ryO6xE0rvhz0TnrsnwR+mfY3vIAzzUYyEscklbppABrGZSJ2cT58zV9Vl9NE20CcURwIDAQAB";
   
    [SerializeField]
    private GameEntity _player;
    public GameEntity Player
    {
        get { return _player; }
        set { _player = value; }
    }

    void Start()
    {
        OpenIAB.mapSku("sku", OpenIAB_Android.STORE_GOOGLE, "google-play.sku");

        Options options = new Options();
        options.verifyMode = OptionsVerifyMode.VERIFY_EVERYTHING;
        options.storeKeys = new Dictionary<string, string> {
            {OpenIAB_Android.STORE_GOOGLE, public_key}
        };

        OpenIAB.init(options);
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
        _player.GetComponent<HeroControll>().PlayerRevive();
        OpenIAB.consumeProduct(purchase);
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
