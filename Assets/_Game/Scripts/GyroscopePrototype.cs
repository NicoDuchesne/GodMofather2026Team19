using UnityEngine;
using System.Collections.Generic;

public class GyroscopePrototype : MonoBehaviour
{
    private List<Joycon> joyconList;
	private Quaternion orientation;
	private Rigidbody rb;
    
    public int joyconIndex = 0;
    public int rotationSpeed = 60;
    
    

    void Start ()
    {
	    rb = GetComponent<Rigidbody>();
	    
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
        }
    }
    
    void FixedUpdate()
    {
	    //rotate avec transform.rotation, mais ça traversait les murs
	    // transform.rotation = Quaternion.RotateTowards(
		   //  transform.rotation,
		   //  orientation,
		   //  rotationSpeed
	    // );
	    
	    //rotate instantané avec un rigidbody, ça ne traverse plus mais la balle prend beaucoup de vitesse
	    //rb.MoveRotation(orientation.normalized);
	    
	    //tentative de mélange, on utilise RotateWowards et rb.MoveRotation
	    Quaternion target = orientation.normalized;
	    
	    Quaternion newRotation = Quaternion.RotateTowards(
		    rb.rotation,
		    target,
		    rotationSpeed * Time.fixedDeltaTime
	    );
	    
	    rb.MoveRotation(newRotation);
    }
}
