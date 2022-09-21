using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    public GameObject[] pathTemplates;

    public static PathPoints instance;

    public List<GameObject> lastPoints;

    public float timeInterval;

    int lastIndex = 0;

    private void Start()
    {
        instance = this;
        lastPoints = new List<GameObject>();
    }

    public void CreateCurrentPathPoint(Vector3 position, float multiply)
    {
        GameObject point = Instantiate(pathTemplates[lastIndex], position, Quaternion.identity, transform);
        point.transform.localScale = point.transform.localScale * multiply;
        point.SetActive(true);
        lastPoints.Add(point);

        lastIndex++;

        if (lastIndex == pathTemplates.Length) 
            lastIndex = 0;
    }

    public void Clear()
    {
        lastPoints.ForEach((obj) => Destroy(obj));
        lastPoints.Clear();
        lastIndex = 0;
    }
}
