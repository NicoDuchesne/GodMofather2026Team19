using UnityEngine;
using System.Collections.Generic;

public class GyroscopePrototype : MonoBehaviour
{
    private List<Joycon> joyconList;
	private Quaternion orientation;
	private Quaternion orientation2;
	private Rigidbody rb;
    
    public int joyconIndex = 0;
    public int rotationSpeed = 60;
    
    

    void Start ()
    {
	    rb = transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
	    
        joyconList = JoyconManager.Instance.j;
		if (joyconList.Count < joyconIndex+1 || joyconIndex < 0){
			Destroy(gameObject);
			Debug.Log("The joyconIndex given is not connected");
		}
    }
    
    void Update () {
		if (joyconList.Count > 0)
        {
	        //We get the joycon asked
			Joycon j = joyconList[joyconIndex];

			//We change the gameObject rotation based on the Z rotation from joycon
			orientation = j.GetRollVector().normalized;
			
			Quaternion target = orientation.normalized;
	    
			Quaternion newRotation = Quaternion.RotateTowards(
				rb.rotation,
				target,
				rotationSpeed * Time.fixedDeltaTime
			);

			rb.MoveRotation(newRotation);
        }
    }
    
}
