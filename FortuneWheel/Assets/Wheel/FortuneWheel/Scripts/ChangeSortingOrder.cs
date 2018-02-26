using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSortingOrder : MonoBehaviour {

    void Start()
    {
        GetComponent<MeshRenderer>().sortingOrder = 8;
    }
}
