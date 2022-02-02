using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] PlayFabManager playFabManager;
    // Start is called before the first frame update
    void Start()
    {
        playFabManager.GetSoftCurrency();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void PayGame()
    {
        if (PlayFabLog.Lives > 0)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(4);
        }
    }
    public void ShopMenu()
    {
        SceneManager.LoadScene(4);
    }
    public void LeaderScene()
    {
        SceneManager.LoadScene(3);
    }
    public void InfoScene()
    {
        SceneManager.LoadScene(5);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(1);
    }
}
