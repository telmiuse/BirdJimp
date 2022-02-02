using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayFabManager : MonoBehaviour
{
    public Text output;
    [SerializeField] LeaderTableManager lm;
    [SerializeField] ShopManager sm;
    [SerializeField] GameObject Panel;
    // Start is called before the first frame update
    void Start()
    {
        //Login();
    }

    // Update is called once per frame

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    void OnSuccess(LoginResult result)
    {
        Debug.Log("ok");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("KO");
        Debug.Log(error.GenerateErrorReport());
    }
    public void SendLeaderboard(int score)
    {
        Panel.SetActive(true);
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate
                {
                    StatisticName = "BirdTable",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    public void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        SceneManager.LoadScene(2);
        Debug.Log("OK Scoretable");
    }
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "BirdTable",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    public void OnLeaderboardGet(GetLeaderboardResult result)
    {
        lm.SetLeaderBoardText(result);
    }
    public void GetSoftCurrency()
    {
        if( Panel != null)Panel.SetActive(true);
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }
    public void AddSoftCurrency(int coins)
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "SC",
            Amount = coins
        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, OnModifyVirtualCurrencySuccess, OnError);
    }
    public void SubstractSoftCurrency(int coins)
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            VirtualCurrency = "SC",
            Amount = coins
        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnModifyVirtualCurrencySuccess, OnError);
    }
    public void OnModifyVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {

    }
    public void OnGetUserInventorySuccess(GetUserInventoryResult result) {
        CheckLives(result);
        CheckJumps(result);
        if (Panel != null) Panel.SetActive(false);
        PlayFabLog.Inventory = result.Inventory;
        PlayFabLog.Coins = result.VirtualCurrency["SC"];
        if (sm != null) sm.SetUIShop();
    }
    public void CheckLives(GetUserInventoryResult result)
    {
        PlayFabLog.Lives = 0;
        foreach (ItemInstance item in result.Inventory)
        {
            if (item.ItemId == "Live") PlayFabLog.Lives += (int)item.RemainingUses;
        }
    }
    public void CheckJumps(GetUserInventoryResult result)
    {
        PlayFabLog.Jumps = 0;
        foreach (ItemInstance item in result.Inventory)
        {
            if (item.ItemId == "Jump") PlayFabLog.Jumps += (int)item.RemainingUses;
        }
    }
    public void SaveUserDara(int fuelLevel, int liveLevel, int jumpLevel)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"FuelLevel", fuelLevel.ToString() },
                {"LiveLevel", liveLevel.ToString() },
                {"JumpLevel", jumpLevel.ToString() }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
    public void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("OK saved");
    }
    public void GetCatalog()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnCatalogRecived, OnError);

    }
    public void OnCatalogRecived(GetCatalogItemsResult result)
    {
        PlayFabLog.Items = result.Catalog;
        sm.SetUIShop();
        Debug.Log("ok Catalogo");

    }
    public void PurchaseCatalogItem(CatalogItem item ,string Currency)
    {
        Panel.SetActive(true);
        PurchaseItemRequest request = new PurchaseItemRequest();
        request.ItemId = item.ItemId;
        request.Price = (int)item.VirtualCurrencyPrices[Currency];
        request.VirtualCurrency = Currency;
        request.StoreId = "BirdStore";
        PlayFabClientAPI.PurchaseItem(request, OnPurchaseSucces, OnPurchaseError);
    }
    public void OnPurchaseSucces(PurchaseItemResult result)
    {
        if (output != null) output.text = "";
        GetSoftCurrency();

        Debug.Log("Purchase Completed");
    }
    public void OnPurchaseError(PlayFabError error)
    {
        if (Panel != null) Panel.SetActive(false);
        if (output != null) output.text = "Insuficient Coins";
        Debug.Log("KO");
        Debug.Log(error.GenerateErrorReport());
    }

    public void ConsumeItem(ItemInstance item, int n)
    {
        Panel.SetActive(true);
        ConsumeItemRequest request = new ConsumeItemRequest();
        request.ItemInstanceId = item.ItemInstanceId;
        request.ConsumeCount = n;
        PlayFabClientAPI.ConsumeItem(request, OnConsumeSucces, OnError);
    }
    public void OnConsumeSucces(ConsumeItemResult result)
    {
        GetSoftCurrency();
    }

}
