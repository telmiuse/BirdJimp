using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject Prefab;
    [SerializeField] GameObject CurrentColumn;
    [SerializeField] Transform  CameraTransform;
    [SerializeField] float  CameraSpeed;
    [SerializeField] Text ScoreText;
    [SerializeField] Text LiveText;
    [SerializeField] Text JumpText;
    [SerializeField] GameObject JumpImg;
    [SerializeField] PlayFabManager PM;

    private int PlayerScore;
    private float lerp;
    private Vector3 newCameraPosition;
    private bool IsMoving;
    private bool FirstColumn = true;
    // Start is called before the first frame update
    void Start()
    {
        JumpText.text = PlayFabLog.Jumps.ToString();
        LiveText.text = PlayFabLog.Lives.ToString();
        PlayerScore = 0;
        ScoreText.text = PlayerScore.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsMoving) MoveCamera();
        if (PlayFabLog.Lives <= 0) GoToMenu();
        JumpText.text = PlayFabLog.Jumps.ToString();
        LiveText.text = PlayFabLog.Lives.ToString();
    }
    void IncrementScore()
    {
        PlayerScore++;
        ScoreText.text = PlayerScore.ToString();
    }
    void UpdateScoreTable()
    {
        PM.SendLeaderboard(PlayerScore);
    }
    void ConsumeLive()
    {
        foreach (ItemInstance item in PlayFabLog.Inventory)
        {
            if (item.ItemId == "Live")
            {
                PlayFabLog.Lives--;
                Debug.Log("CONSUMIR VIDA");
                PM.ConsumeItem(item, 1);
                return;
            }
        }
    }
    public void ConsumeJump(int n)
    {
        foreach (ItemInstance item in PlayFabLog.Inventory)
        {
            if (item.ItemId == "Jump")
            {
                PlayFabLog.Jumps--;
                PM.ConsumeItem(item, n);
                return;
            }
        }
        JumpText.text = PlayFabLog.Jumps.ToString();
        JumpText.enabled = false;
        JumpImg.SetActive(false);
    }
    public GameObject NextColumn()
    {
        IncrementScore();
        IsMoving = true;
        FirstColumn = false;
        Vector3 nextPosition = CurrentColumn.transform.position;
        nextPosition.z += Random.Range(3, 8);
        Instantiate(Prefab, nextPosition, gameObject.transform.rotation);
        newCameraPosition = new Vector3(CurrentColumn.transform.position.x + 5, CurrentColumn.transform.position.y + 5, CurrentColumn.transform.position.z - 1);
        return CurrentColumn;
    }
    public void MoveCamera()
    {
        if (lerp <= 1f)
        {
            lerp += CameraSpeed * Time.deltaTime;
            if (CameraTransform.position == newCameraPosition) IsMoving = false;
            CameraTransform.position = Vector3.Lerp(CameraTransform.position, newCameraPosition, lerp);
        }
        else
        {
            lerp = 0;
        }
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void GameOver()
    {
        PM.AddSoftCurrency(PlayerScore);
        ConsumeLive();
        UpdateScoreTable();
       // SceneManager.LoadScene(2);

    }
    public void SetCurrentColumn(GameObject C)
    {
        CurrentColumn = C;
    }

}
