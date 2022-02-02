using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LeaderTableManager : MonoBehaviour
{
    [SerializeField] PlayFabManager playFabManager;
    [SerializeField] Text LeaderBoardText;

    [SerializeField] GameObject rowPrefab;
    [SerializeField] Transform rowsParent;
    // Start is called before the first frame update
    void Start()
    {
        playFabManager.GetLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetLeaderBoardText(GetLeaderboardResult result)
    {
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGo.GetComponentsInChildren<Text>();
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName.ToString();
            texts[2].text = item.StatValue.ToString();
            Debug.Log(string.Format("Place: {0} | ID: {1} | VALUE: {2} ",
                    item.Position, item.DisplayName, item.StatValue));
        }
    }

    public void GetLeaderBoard()
    {
        playFabManager.GetLeaderboard();
    }

    public void ReturnToGame()
    {
        SceneManager.LoadScene(1);
    }
}
