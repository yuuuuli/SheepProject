using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Sheeps : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform targetTrans;
    private AsyncOperation ao;
    public bool ifLoadGameScene;

    void Start()
    {
        ao = SceneManager.LoadSceneAsync(2);
        ao.allowSceneActivation = false;
        transform.DOLocalMove(targetTrans.localPosition, 2).SetEase(Ease.Linear).OnComplete
            (
                OnCompeleteEvent
            );
    }

    private void OnCompeleteEvent()
    {
        if (ifLoadGameScene)
        {
            ao.allowSceneActivation = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
