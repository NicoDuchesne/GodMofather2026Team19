using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class Collectible : MonoBehaviour
{
    private MeshRenderer collectibleMesh;
    private Transform collectibleTransform;
    [SerializeField] int collectibleScore = 8;
    [SerializeField] GameObject marblePrefab;
    float fadeTime = 0.5f;
    GameObject marble;

    bool canCollect = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collectibleMesh = gameObject.GetComponent<MeshRenderer>();
        collectibleTransform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Marble") && canCollect)
        {
            canCollect = false;
            Debug.Log("collect");
            ScoreManager.instance.IncrementScore(collectibleScore);

            for (int i = 0; i < 2; i++)
            {
                marble = Instantiate(marblePrefab, transform.position, transform.rotation);
            }

            collectibleTransform.DOScale(1.2f, fadeTime).SetEase(Ease.OutQuart);
            collectibleMesh.material.DOFade(0f, fadeTime).OnComplete(() =>
            {
                Destroy(gameObject);
            });        
        }
    }
}
