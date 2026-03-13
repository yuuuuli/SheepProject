using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SheepMove : MonoBehaviour
{
    public RectTransform rt;
    public Transform[] movePointsTrans;
    private Vector3[] movePoints;
    public bool loadScene;
    private AsyncOperation ao;

    // Start is called before the first frame update
    void Start()
    {
        movePoints = new Vector3[movePointsTrans.Length];
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePoints[i] = movePointsTrans[i].localPosition;
        }
        if (loadScene)
        {
            ao = SceneManager.LoadSceneAsync(1);
            ao.allowSceneActivation = false;
        }
        transform.DOLocalPath(movePoints, 3).SetEase(Ease.Linear).OnComplete
            (
            () =>
            {
                if (loadScene)
                {
                    ao.allowSceneActivation = true;
                }

            }
            );
    }

    // Update is called once per frame
    void Update()
    {
        if (rt.anchoredPosition.y >= 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
