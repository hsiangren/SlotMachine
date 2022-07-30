using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIEffects;

public class LogoCtrl : MonoBehaviour
{
    [SerializeField] GameObject logo;
    public float redirectDelay;
    // Start is called before the first frame update
    float procTime;
    bool endFlag;
    void Start()
    {
        endFlag = false;
        procTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (endFlag)
            return;

        procTime += Time.deltaTime;
        logo.GetComponent<UIEffect>().effectFactor = 1.0f - EasingFunction.EaseInExpo(0, 1, procTime / redirectDelay);

        if (procTime > redirectDelay)
        {
            endFlag = true;
            GameManager.instance.LoadScene( (int)SceneIndexes.Main );
        }
    }
}
