using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightBlinking : MonoBehaviour
{
    [SerializeField] private Light light;
    // Start is called before the first frame update
    float Delay = 0.5f;
    float timeDelay = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeDelay += Time.deltaTime;
        if (timeDelay > Delay)
        {
            timeDelay = 0;
            Delay = Random.Range(3.0f, 5.0f);
            light.enabled = !light.enabled;
            Invoke("setActiv_", Random.Range(0.01f, 0.05f));
        }
    }

    void setActiv_()
    {
        light.enabled = true;
    }
}
