using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class PlayFabLog
{
    public static int Coins = 0;
    public static int Lives = 0;
    public static int Jumps = 0;
    public static List<CatalogItem> Items;
    public static List<ItemInstance> Inventory;
    public static void Login(string email, string password)
    {
        var request = new LoginWithEmailAddressRequest
        {

            Email = email,
            Password = password
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, OnSuccess, OnError);
    }
    public static void Register(string name, string email, string password)
    {
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = name,
            Username = name,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);

    }
    static void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        SceneManager.LoadScene(1);
        Debug.Log("ok");
    }
    static void OnSuccess(LoginResult result)
    {
        SceneManager.LoadScene(1);
        Debug.Log("ok");
    }
    static void OnError(PlayFabError error)
    {
        Debug.Log("KO");
        Debug.Log("report " + error.GenerateErrorReport());

    }
}
