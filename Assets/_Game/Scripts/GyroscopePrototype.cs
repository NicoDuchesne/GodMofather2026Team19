using UnityEngine;
using System.Collections.Generic;

public class GyroscopePrototype : MonoBehaviour
{
    private List<Joycon> joyconList;
	private Quaternion orientation;
    
    public int joyconIndex = 0;

    void Start ()
    {
	    //We get the joyconList and checks if t
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
			orientation = j.GetRollVector();
            gameObject.transform.rotation = orientation;
        }
    }
}
