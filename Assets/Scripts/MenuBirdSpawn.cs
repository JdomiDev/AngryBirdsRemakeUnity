using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBirdSpawn : MonoBehaviour
{

    public GameObject[] birdPrefabs;

    [HideInInspector]
    public int birdIndex = 0;

    [HideInInspector]
    public List<GameObject> birds;

    private void Awake()
    {
        StartCoroutine(birdSpawner());
    }

    private void Update()
    {
        foreach (var bird in new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")))
        {
            if(bird.transform.position.y < -10)
            {
                Destroy(bird);
            }
        }
    }

    private IEnumerator birdSpawner()
    {
        yield return new WaitForSeconds(1f);
        float rand;
        while (0 == 0)
        {
            rand = Random.Range(0.15f, 0.45f);
            birds.Add(Instantiate(birdPrefabs[Random.Range(0, 6)], gameObject.transform.position, new Quaternion(0, 0, 0, 0)) as GameObject);
            birds[birdIndex].GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(3, 30), Random.Range(7, 15));
            birds[birdIndex].transform.localScale = new Vector3(rand, rand, rand);
            birdIndex++;

            yield return new WaitForSeconds(Random.Range(1f, 4f));
        }
    }
}
