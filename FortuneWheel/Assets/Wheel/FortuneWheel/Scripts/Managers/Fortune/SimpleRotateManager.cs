using UnityEngine;
using System.Collections;

public class SimpleRotateManager : MonoBehaviour {
    public bool allowRotation;
    public float rotationSpeed;
    public int rotationDirection;

    private Transform _tr;

	
	void Start () {
        this._tr = this.transform;
	}
	
	void Update () {
	    if (this.allowRotation&& this._tr != null)
        {
            this._tr.Rotate(0, 0, this.rotationDirection * this.rotationSpeed * Time.deltaTime);
        }
	} // Update

} // SimpleRotateManager