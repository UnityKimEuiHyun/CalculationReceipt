using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Scrollbar))]
public class ScrollDown : MonoBehaviour
{
    Scrollbar bar;
    [SerializeField]
    float beforeSize = 1;
    // 중간에 콘텐츠가 생성되는 경우 false
    public static bool isLastContent = true;

    private void Start()
    {
        bar = GetComponent<Scrollbar>();
    }

    private void OnEnable()
    {
        StartCoroutine(wait());
    }

    private void Update()
    {
            
        if (bar.size != beforeSize)
        {
            if (bar.size < beforeSize && gameObject.activeSelf)
            {
                if (isLastContent)
                    StartCoroutine(wait());
                else
                    isLastContent = true;
            }
            beforeSize = bar.size;
        }
    }

    public IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        bar.value = 0;
        yield return null;
    }

}
