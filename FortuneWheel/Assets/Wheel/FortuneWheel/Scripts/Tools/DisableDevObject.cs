using UnityEngine;
using System.Collections;

public class DisableDevObject : MonoBehaviour {

    void OnEnable()
    {
        if (MainController.Instance.environment != Environments.dev)
        {
            this.gameObject.SetActive(false);
        }
    }
}
