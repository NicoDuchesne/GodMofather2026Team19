using UnityEngine;
using System.Collections.Generic;
public class CameraTarget : MonoBehaviour
{
    public GameObject ballList;
    public int maxDistance = 100;

    //private List<float> positions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float total = 0;
        float count = 0f;
        foreach (Transform ballTransform in ballList.transform)
        {
            if (Mathf.Abs(transform.position.y - ballTransform.position.y) < maxDistance)
            {
                total += ballTransform.position.y;
                count+=1f;
            }
        }
        
        if (count > 0f)
        {
            this.transform.position = new Vector3(0, total/count, 0);
        } else {
            if (ballList.transform.GetChild(0) != null) this.transform.position = new Vector3(0, ballList.transform.GetChild(0).position.y, 0);
        }
        
    }
}
