using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] GameObject Cloud;
    [SerializeField] float MaxTimeToSpawn;
    [SerializeField] float MinTimeToSpawn;
    [SerializeField] float RestTimeToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RestTimeToSpawn -= Time.deltaTime;
        if (RestTimeToSpawn <= 0) Spawn();
    }

    void Spawn()
    {
        Instantiate(Cloud, gameObject.transform.position, gameObject.transform.rotation);
        RestTimeToSpawn = Random.Range(MinTimeToSpawn, MaxTimeToSpawn);
    }
}
