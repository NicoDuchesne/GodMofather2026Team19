using UnityEngine;
using System.Collections.Generic;

public class GyroscopePrototype : MonoBehaviour
{
    private List<Joycon> joyconList;
	private Quaternion orientation;
	private Rigidbody rb;
    
    public int joyconIndex = 0;
    public int rotationSpeed = 60;
    public Transform cameraTarget;
    
    

    void Start ()
    {
	    rb = this.GetComponent<Rigidbody>();
	    
        joyconList = JoyconManager.Instance.j;
		if (joyconList.Count < joyconIndex+1 || joyconIndex < 0){
			Debug.Log("The joyconIndex given is not connected");
		}
    }
    
    void FixedUpdate () {
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

			
			// rb.MovePosition(new Vector3(0, cameraTarget.position.y, 0));
			// rb2.MovePosition(new Vector3(0, -cameraTarget.position.y, 0));
			
			// this.transform.position = new Vector3(0, cameraTarget.position.y, 0);	
			// this.transform.GetChild(0).position = new Vector3(0, -cameraTarget.position.y, 0);	
        }
    }

    
}
