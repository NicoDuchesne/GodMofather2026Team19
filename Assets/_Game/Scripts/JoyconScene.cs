using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoyconScene : MonoBehaviour
{
    public float waitTime;
    public string NextScene;
    private IEnumerator coroutine;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine((WaitForNextScene(waitTime)));
    }

    private IEnumerator WaitForNextScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(NextScene);
    }
}
