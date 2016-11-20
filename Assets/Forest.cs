using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Forest {
    public static List<Tree> forest = new List<Tree>();
    public static List<NonlivingNature> nonlNature = new List<NonlivingNature>();
    public static int child, teen, mature, old;
    public static void UpdateForest()
    {
        child = 0; teen = 0; mature = 0; old = 0;
        for (int i = 0; i < forest.Count-1; i++)
        {
            forest[i].UpdateModel();
            if (forest[i] == null)
                continue;
            if (forest[i].state.age < 20)
                child++;
            else if (forest[i].state.age < 50)
                teen++;
            else if (forest[i].state.age < 150)
                mature++;
            else
                old++;
        }
    }
}
