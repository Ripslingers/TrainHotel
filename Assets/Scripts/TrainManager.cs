using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public GameObject wagonPrefab;
    public Transform lastWagon;
    public float wagonSpacing = 5f;

    public void AddWagon()
    {
        Vector3 newWagonPosition = lastWagon.position - new Vector3(0, 0, wagonSpacing);
        GameObject newWagon = Instantiate(wagonPrefab, newWagonPosition, Quaternion.identity);
        lastWagon = newWagon.transform;
    }
}

