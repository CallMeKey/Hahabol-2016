using UnityEngine;
using System.Collections;

[System.Serializable]

public class ForestManager : MonoBehaviour {
    [SerializeField]
    public TreeState pineNormal;
    [SerializeField]
    public TreeState pineRandRange;
    private bool isRedyToCalc = true;
    [HideInInspector]
    public bool isOnPlay = false;
	// Use this for initialization
	void Start () {
    }
	// Update is called once per frame
	void Update () {
        if(isOnPlay && isRedyToCalc)
        {
            StartCoroutine(StartPlay());
        }
        //Forest.UpdateForest();
	}
    IEnumerator StartPlay()
    {
        Debug.Log("DO!");
        isRedyToCalc = false;
        yield return new WaitForSeconds(0.1f);
        UpdateForest();
        isRedyToCalc = true;
    }
    public void UpdateForest()
    {
        Forest.UpdateForest();
    }
    public void TurnPlay()
    { isOnPlay = !isOnPlay; }
    public TreeState PineRandGen()
    {
        TreeState newstate = new TreeState();
        newstate.age = pineNormal.age;
        newstate.branchGrow = pineNormal.branchGrow + GetRand(pineRandRange.branchGrow);
        newstate.rBranches = pineNormal.rBranches + GetRand(pineRandRange.rBranches);
        newstate.rReproduction = pineNormal.rReproduction + GetRand(pineRandRange.rReproduction);
        newstate.shadowDamage = pineNormal.shadowDamage + GetRand(pineRandRange.shadowDamage);
        newstate.tall = pineNormal.tall + GetRand(pineRandRange.tall);
        newstate.def = pineNormal.def + GetRand(pineRandRange.def);
        return newstate;
    }
    private static float GetRand (float vary)
    {
        float ret = 0;
        for (int i = 0; i < 6; i++)
        {
            ret += Random.value;
        }
        return ret * vary + 1;
        
    }
}
