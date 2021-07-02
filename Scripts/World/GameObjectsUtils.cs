using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameObjectsUtils
{
    public static List<GameObject> SortByDistance(this List<GameObject> objects, Vector3 mesureFrom)
    {
        return objects.OrderBy(x => Vector3.Distance(x.transform.position, mesureFrom)).ToList();
    }
}
