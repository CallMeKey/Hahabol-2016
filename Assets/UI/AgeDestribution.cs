using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AgeDestribution : MonoBehaviour {
    private Text text;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "Child: " + Forest.child + "\n" +
                "Teen: " + Forest.teen + "\n" +
                "Mature: " + Forest.mature + "\n" +
                "Old: " + Forest.old;
	}
}
