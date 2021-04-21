using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField]
    Text timeText = null;

    // Update is called once per frame
    void Update()
    {
        timeText.text = Mathf.Round(Time.timeSinceLevelLoad).ToString();
    }
}
