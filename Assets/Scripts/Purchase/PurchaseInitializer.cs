using System.Collections.Generic;
using OnePF;

public class PurchaseInitializer
{
    private static PurchaseInitializer _instance;
    private const string public_key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmTFGKYq5TXsik57CDmtRj8ayQ466PKKXlEgjFbL6iK5rb+uB8ezAXm4sFspc9TjE8PXd7Y72XIdab4mSgtvU8KovUatA99OhoQ1oEXtb8SPB6YKczDlS6xvs0xwiTlOkttAYfQpIC2T+H35i2OTZu2M9sjCSnECXiixyKKJsqMz4H1i+J6PPCcrQSy1VAeym/oyR4YtHQwefc6m27P2uWdrFCfVYfPt1qSzD4m9THkRWJSLnMGFsAYTGj4KjQJXKVb4C/HXZes5/1XfK3b0ryO6xE0rvhz0TnrsnwR+mfY3vIAzzUYyEscklbppABrGZSJ2cT58zV9Vl9NE20CcURwIDAQAB";

    private PurchaseInitializer() {}


    public static PurchaseInitializer Init()
    {
        if (_instance == null)
        {
            _instance = new PurchaseInitializer();
            PurchaseInit();
        }
        return _instance;
    }

    private static void PurchaseInit()
    {
        OpenIAB.mapSku("sku", OpenIAB_Android.STORE_GOOGLE, "google-play.sku");

        Options options = new Options();
        options.verifyMode = OptionsVerifyMode.VERIFY_EVERYTHING;
        options.storeKeys = new Dictionary<string, string> {
            {OpenIAB_Android.STORE_GOOGLE, public_key}
        };

        OpenIAB.init(options);
    }
}
