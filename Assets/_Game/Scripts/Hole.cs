using DG.Tweening;
using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Hole : MonoBehaviour
{
    int endScore = 88;
    [SerializeField] int scoreMult = 1;
    float marbleFadeTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Marble"))
        {

            Debug.Log("out");
            ScoreManager.instance.IncrementScore(endScore * scoreMult);

            collider.transform.DOScale(0f, marbleFadeTime).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                Destroy(collider.gameObject);
            });
        }
    }
}
