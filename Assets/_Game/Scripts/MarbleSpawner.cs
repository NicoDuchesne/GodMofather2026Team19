using UnityEngine;

public class MarbleSpawner : MonoBehaviour
{

    [SerializeField] bool origin = false;
    Rigidbody marbleBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        marbleBody = gameObject.GetComponent<Rigidbody>();

        if (!origin)
        {
            marbleBody.linearVelocity = new Vector3(Random.Range(-10, 10), 10, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
