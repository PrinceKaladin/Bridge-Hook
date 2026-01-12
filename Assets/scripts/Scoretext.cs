using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoretext : MonoBehaviour
{
    private void Start()
    {
        Text text = this.GetComponent<Text>();
        text.text = "Score " + PlayerPrefs.GetInt("score", 0) + "\n" + "Best Score" + PlayerPrefs.GetInt("bestscore", 0); 
    }
}
