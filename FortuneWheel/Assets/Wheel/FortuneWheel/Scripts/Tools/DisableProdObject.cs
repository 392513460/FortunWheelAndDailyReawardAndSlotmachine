using UnityEngine;
using System.Collections;

public class DisableProdObject : MonoBehaviour {

	void OnEnable()
    {
        if (MainController.Instance.startPoint.environment == Environments.prod)
        {
            gameObject.SetActive(false);
        }
	}
}