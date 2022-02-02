using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float Force;
    [SerializeField] float ForceOffset;
    [SerializeField] Rigidbody Rb;
    [SerializeField] GameManager GM;
    [SerializeField] GameObject CurrentTower;
    [SerializeField] ParticleSystem PickUp;
    [SerializeField] GameObject HoldToJump;
    private bool isJumping;
    private bool isStart;
    private bool HasReborn;
    // Start is called before the first frame update
    void Start()
    {
        HasReborn = false;
    }
    // Update is called once per frame
    void Update()
    {
        CheckTouch();
        CheckPosition();
    }
    public void CheckTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
        if (Input.GetMouseButton(0))
        {
            Force += Time.deltaTime * ForceOffset;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Jump();
        }
    }
    public void CheckPosition()
    {
        if (gameObject.transform.position.y <= -10)
        {
            if (PlayFabLog.Jumps > 0 && !HasReborn) {
                HasReborn = true;
                isJumping = false;
                Vector3 SafePosition = new Vector3(CurrentTower.transform.position.x, CurrentTower.transform.position.y + 0.65f, CurrentTower.transform.position.z);
                gameObject.transform.position = SafePosition;
                gameObject.transform.rotation = Quaternion.identity;
                PlayFabLog.Jumps--;
                GM.ConsumeJump(1);
            }
            else {
                GM.GameOver();
                Destroy(gameObject);
            }
        }
    }
    public void Jump()
    {
        if (!isJumping)
        {
            HoldToJump.SetActive(false);
            Rb.AddForce((gameObject.transform.forward * Force) + (7.0f * Vector3.up), ForceMode.Impulse);
            isJumping = true;
        }
        Force = 0;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground" && collision.gameObject != CurrentTower)
        {
            PickUp.Play();
            GM.SetCurrentColumn(collision.gameObject);
            GM.MoveCamera();
            CurrentTower = GM.NextColumn();
            isJumping = false;
        }
        if (collision.gameObject.tag == "Ground" && !isStart)
        {
            //GM.SetCurrentColumn(collision.gameObject);
            isStart = true;
            GM.NextColumn();
            isJumping = false;
        }
    }

}
