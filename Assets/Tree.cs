using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TreeState
{
    private float m_growth = 0;
    public float growth
    {
        set { m_growth = value; }
        get { return m_growth; }
    }
    private float m_rTrunk = 1;
    public float rTrunk
    {
        set
        {
            m_rTrunk = value;
        }
        get { return m_rTrunk; }
    }
    [SerializeField]
    private float m_rBranches = 2;
    public float rBranches
    {
        set
        {
            m_rBranches = value;
        }
        get { return m_rBranches; }
    }
    private float m_rRoot = 3;
    public float rRoot
    {
        set
        {
            m_rRoot = value;
        }
        get { return m_rRoot; }
    }
    [SerializeField]
    private float m_rReproduction = 1;
    public float rReproduction
    {
        set { m_rReproduction = value; }
        get { return m_rReproduction; }
    }
    public float tall = 0;
    public float shadowDamage = 1.0f;
    //growing:
    [SerializeField]
    private float m_Age = 0;
    public float age
    {
        get { return m_Age; }
        set { m_Age = value;
        }
    }
    [SerializeField]
    private float m_branchGrow = 1;
    public float branchGrow
    {
        get { return m_branchGrow; }
        set { m_branchGrow = value; }
    }
    private float m_rootGrow = 1;
    public float rootGrow
    {
        get { return m_rootGrow; }
        set { m_rootGrow = value; }
    }
    public float def = 0;
    //otherPrametrs
    public static float reprodutionStandart = 2.0f;
    //public float m_DieChance = 0.01f; //from 0 up to 1;
    //private float m_Irigation = 0;
    //public float irigation
    //{
    //    get { return m_Irigation; }
    //    set { m_Irigation = value; }
    //}
   //public void Copy (TreeState state)
   //{
   //    age = state.age;
   //    growth = state.growth;
   //    rBranches = state.rBranches;
   //    branchGrow = state.branchGrow;
   //    rReproduction = state.rReproduction;
   //    shadowDamage = state.shadowDamage;
   //    tall = state.tall;
   //     
   //}
}
[System.Serializable]
public struct AgePeriod
{
    public float age1;
    public float age2;
    public float age3;
    public float age4;
    public float age5;
}

public class Tree : MonoBehaviour {
    public GameObject treePrefab;
    [SerializeField]
    public TreeState state;
    [SerializeField]
    SpriteRenderer m_truncModel;
    [SerializeField]
    SpriteRenderer m_branchModel;
    [SerializeField]
    SpriteRenderer m_rootModel;
    [SerializeField]
    SpriteRenderer m_shadowModel;
    [SerializeField]
    public AgePeriod ages;
    [SerializeField]
    private ForestManager forestHELL;
    public float startTall = 0.05f;
    public static int deadTrees = 0;
    // Use this for initialization
    void Start () {
        Forest.forest.Add(this);
        state = forestHELL.PineRandGen();
        UpdateModel();
    }
	// Update is called once per frame
	void Update () {
    }
    public void UpdateModel()
    {
        SetCoef();
        if (state.rRoot < 0)
            state.rRoot = 0;
        if (state.rBranches < 0)
            state.rBranches = 0;
        m_branchModel.transform.localScale = new Vector3(state.rBranches, state.rBranches, 0);
        state.age += 1;
        DoEffect();
        GrawBranch();
        ToReproduction();
        if(state.rBranches < 0.2f)
        {
            deadTrees += 1;
           //for (int i = 0; i < transform.childCount; i++)
           //{
           //    Destroy(transform.GetChild(i).gameObject);
           //}
            Forest.forest.Remove(this);
            Destroy(this);
        }
        m_branchModel.color = new Color (0, 1 - (state.age / 100), 0);
        state.def = Mathf.Exp(state.age / 25);
    }
    public void DoEffect()
    {
        for (int i = 0; i < Forest.forest.Count; i++)
        {
            DoEffect(Forest.forest[i]);
        }
    }
    private void DoEffect(Tree otherTree)
    {
        if (otherTree == this)
            return;
        float dist = Vector2.Distance(otherTree.transform.position, transform.position);
        float penalty = (dist - state.rBranches - otherTree.state.rBranches);
        if (penalty < 0)
        {
            state.rBranches -= state.shadowDamage / state.def;
        }
    }
    public void ToReproduction ()
    {
        int childCount = (int)(state.tall * temperatureCoef * precipCoef * state.rReproduction * GetRA(state.age) * TreeState.reprodutionStandart);
        for (int i = 0; i < childCount; i++)
        {
            Vector2 newPos = Random.insideUnitCircle * (100); //TODO:reproduction radius
            var newTree = Instantiate(treePrefab);
            newTree.transform.position = new Vector3(transform.position.x + newPos.x, transform.position.y + newPos.y);
            var treetemp = newTree.GetComponent<Tree>();
            treetemp.forestHELL = forestHELL;
        }
    }
    private float GetRA(float x)
    {
        float c = 3;
        float a = 34;
        if (x > 20 && x < 60)
            return (17 * (c / a) * Mathf.Pow(((0.7f * x) / a), c - 1) * (Mathf.Exp(-Mathf.Pow((0.7f * x) / a, c))));
        else if (x > 60)
            return Mathf.Exp(-0.018f * x);
        return 0;
    }
    private float GetTallCoef()
    {
        float ret = 0;
        if (state.age < ages.age1)
        {
            ret = -0.25f * state.age + 1.5f;
        }
        else if (state.age < ages.age2)
        {
            ret = (-0.15f * state.age + 1.5f) / 2.0f;
        }
        else if (state.age < ages.age3)
        {
            ret = (-0.053f * state.age + 1.5f) / 4.0f;
        }
        else if (state.age < ages.age4)
        {
            ret = 0.12f;
        }
        else if (state.age < ages.age5)
        {
            ret = Mathf.Exp(-0.045f * state.age);
        }
        return ret;
    }
    private void GrawRoot ()
    {

    }
    private void GrawBranch()
    {
        float tallCoef = GetTallCoef();
        if (tallCoef < 0.01)
            return;
        state.rBranches += state.branchGrow * energyCoef * temperatureCoef * precipCoef * tallCoef + startTall;
    }
    public virtual void GetEnvironment()
    {
        
    }
    public virtual void DieCalc()
    {
        //if(Random.value < state.m_DieChance)
        //{
        //    Forest.forest.Remove(this);
        //    Destroy(this);
        //}
    }
    public float temperatureCoef = 0;
    public float energyCoef = 0;
    public float precipCoef = 0;
    public virtual void SetCoef()
    {
        float s = 6;
        float m = 20;
        temperatureCoef = 15 / (s * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-(Mathf.Pow((Climate.temperature - m), 2) / (2 * Mathf.Pow(s, 2))));
        s = 8;
        m = 85;
        energyCoef = (20 / (s * Mathf.Sqrt(2 * Mathf.PI))) * Mathf.Exp(-(Mathf.Pow((Climate.energy - m), 2) / (2 * Mathf.Pow(s, 2))));
        s = 18;
        m = 100;
        precipCoef = 45 / (s * Mathf.Sqrt(2 * Mathf.PI)) * Mathf.Exp(-(Mathf.Pow((Climate.precipitation - m), 2) / (2 * Mathf.Pow(s, 2))));
    }
}
