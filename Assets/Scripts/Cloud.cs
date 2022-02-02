using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] float Live;
    [SerializeField] float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += transform.forward * Speed * Time.deltaTime;
        Live -= Time.deltaTime;
        if (Live <= 0) Destroy(gameObject);
    }
}
