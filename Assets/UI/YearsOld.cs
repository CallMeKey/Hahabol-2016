using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class YearsOld : MonoBehaviour {
    // Use this for initialization
    private Text text;
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "Living: " + Forest.forest.Count + "\n" +
                        "Dead: " + Tree.deadTrees;

    }
}
