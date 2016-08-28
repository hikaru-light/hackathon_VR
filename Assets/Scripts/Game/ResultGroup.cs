using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultGroup : SubGroup
{
    [SerializeField]
    UnityEvent showCallback;

    void OnEnable()
    {
        Timer.StopCount();
    }

    public override void FadeCallback()
    {
        showCallback.Invoke();
        SceneManager.LoadSceneAsync( "Main" );
    }
}
