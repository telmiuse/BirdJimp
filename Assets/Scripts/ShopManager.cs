using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Text LivePriceText;
    [SerializeField] Text JumpPriceText;
    [SerializeField] Text LiveRealPriceText;
    [SerializeField] Text JumpRealPriceText;
    [SerializeField] Text CoinText;
    [SerializeField] Text NLivesText;
    [SerializeField] Text NJumpsText;
    [SerializeField] Text LiveTimeText;
    [SerializeField] Text JumpTimeText;
    [SerializeField] Animator animation;
    [SerializeField] PlayFabManager playFabManager;
    [SerializeField] GameObject ClaimButton;
    [SerializeField] GameObject ClaimButtonLive;


    DateTime nextTime;
    DateTime nextTimeLive;
    // Start is called before the first frame update
    void Start()
    {
        GetShopItems();

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("IsWaiting") == 1) RestJumpTime();
        if (PlayerPrefs.GetInt("IsWaitingLive") == 1) RestLiveTime();
    }

    public void RestJumpTime()
    {
        DateTime currentDate = System.DateTime.Now;
        long temp = Convert.ToInt64(PlayerPrefs.GetString("next"));
        DateTime nextDate = DateTime.FromBinary(temp);
        TimeSpan difference = currentDate.Subtract(nextDate);
        if (difference.TotalSeconds > 0)
        {
            ClaimButton.SetActive(true);
            JumpTimeText.text = "00:00";
        }
        else
        {
            JumpTimeText.text = (-difference.Minutes).ToString() + ":" + (-difference.Seconds).ToString();
        }

    }
    public void RestLiveTime()
    {
        DateTime currentDate = System.DateTime.Now;
        long temp = Convert.ToInt64(PlayerPrefs.GetString("nextLive"));
        DateTime nextDateLive = DateTime.FromBinary(temp);
        TimeSpan difference = currentDate.Subtract(nextDateLive);
        if (difference.TotalSeconds > 0)
        {
            ClaimButtonLive.SetActive(true);
            LiveTimeText.text = "00:00";
        }
        else
        {
            LiveTimeText.text = (-difference.Minutes).ToString() + ":" + (-difference.Seconds).ToString();
        }


    }
    public void SetUIShop()
    {
        CoinText.text = PlayFabLog.Coins.ToString();
        foreach (CatalogItem item in PlayFabLog.Items)
        {
            if (item.ItemId == "Live") LivePriceText.text = item.VirtualCurrencyPrices["SC"].ToString();
            if (item.ItemId == "Live") LiveRealPriceText.text = item.VirtualCurrencyPrices["RM"].ToString();
            if (item.ItemId == "Jump") JumpPriceText.text = item.VirtualCurrencyPrices["SC"].ToString();
            if (item.ItemId == "Jump") JumpRealPriceText.text = item.VirtualCurrencyPrices["RM"].ToString();
        }
        NLivesText.text = PlayFabLog.Lives.ToString();
        if (PlayFabLog.Lives <= 0) {
            NLivesText.color = Color.red;
            animation.SetTrigger("EmptyLives");
        } else {
            animation.SetTrigger("HasLives");
            NLivesText.color = Color.black; 
        }
        NJumpsText.text = PlayFabLog.Jumps.ToString();
    }
    public void GetShopItems()
    {
        playFabManager.GetCatalog();

    }
    public void BuyLives()
    {
        if (PlayerPrefs.GetInt("IsWaitingLive") == 0)
        {
            nextTimeLive = DateTime.Now.AddMinutes(10);
            PlayerPrefs.SetString("nextLive", nextTimeLive.ToBinary().ToString());
            PlayerPrefs.SetInt("IsWaitingLive", 1);
        }
    }
    public void BuyJump()
    {
        if (PlayerPrefs.GetInt("IsWaiting") == 0)
        {
            nextTime = DateTime.Now.AddMinutes(5);
            PlayerPrefs.SetString("next", nextTime.ToBinary().ToString());
            PlayerPrefs.SetInt("IsWaiting", 1);
        }
    }
    public void GetLive()
    {
        ClaimButtonLive.SetActive(false);
        PlayerPrefs.SetInt("IsWaitingLive", 0);
        LiveTimeText.text = "";
        playFabManager.PurchaseCatalogItem(PlayFabLog.Items[0],"SC");
    }
    public void GetJump()
    {
        ClaimButton.SetActive(false);
        PlayerPrefs.SetInt("IsWaiting", 0);
        JumpTimeText.text = "";
        playFabManager.PurchaseCatalogItem(PlayFabLog.Items[1], "SC");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
    }
   
    public void BuyRealLive()
    {
        PlayerPrefs.SetInt("IsWaitingLive", 0);
        ClaimButtonLive.SetActive(false);
        LiveTimeText.text = "";
        playFabManager.PurchaseCatalogItem(PlayFabLog.Items[0], "SC");
    }

    public void BuyRealjUMP()
    {
        PlayerPrefs.SetInt("IsWaiting", 0);
        ClaimButton.SetActive(false);
        JumpTimeText.text = "";
        playFabManager.PurchaseCatalogItem(PlayFabLog.Items[1], "SC");
    }


}
