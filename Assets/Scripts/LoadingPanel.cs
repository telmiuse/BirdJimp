using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField] LoadingPanel Text;
    [SerializeField] float TimeToReset;
    private const string loading = "LOADING";
    int p;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeToReset <= 0) SetUI();
    }

    void SetUI()
    {
        string text = loading;
        for(int i = 0; i <= p; i++)
        {
            text += ".";
        }
        p++;
        if (p == 4) p = 0;
    }
}
