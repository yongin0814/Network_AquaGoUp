using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Title_YJ : MonoBehaviour
{
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= 5.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
