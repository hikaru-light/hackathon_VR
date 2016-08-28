using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

public abstract class SubGroup : MonoBehaviour
{
    public abstract void FadeCallback();
}

public class GameManager : MonoBehaviour
{
    public enum SubGroupIndex
    {
        Title = 0,
        Result = 1,
    }

    [SerializeField]
    VRTeleport vrTeleport;

    [SerializeField]
    float duration = 1f;

    [SerializeField]
    CanvasGroup[] subGroupCanvas;

    [SerializeField]
    SubGroup[] subGroup;

    [SerializeField]
    Transform canvasShowPoint;

    int currentSubGroupIndex;

    bool isFading;


    public void ShowGroup(SubGroupIndex index)
    {
        this.transform.position = this.canvasShowPoint.position;
        this.transform.rotation = this.canvasShowPoint.rotation;

        this.isFading = false;
        this.currentSubGroupIndex = (int)index;
        this.subGroupCanvas[this.currentSubGroupIndex].alpha = 1f;
        this.vrTeleport.IsTeleportLock = true;
        this.subGroupCanvas[this.currentSubGroupIndex].gameObject.SetActive(true);
    }

    void Awake()
    {
        this.ShowGroup(SubGroupIndex.Title);
    }

    void Update()
    {
        if (this.isFading) { return; }
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(this.Fade());
        }
    }

    IEnumerator Fade()
    {
        this.isFading = true;
        float timer = 0f;
        while (timer <= duration)
        {
            this.subGroupCanvas[this.currentSubGroupIndex].alpha = Mathf.Lerp(1f, 0f, timer / this.duration);
            timer += Time.deltaTime;
            yield return null;
        }
        this.subGroup[this.currentSubGroupIndex].FadeCallback();
        this.subGroupCanvas[this.currentSubGroupIndex].gameObject.SetActive(false);
        yield break;
    }
}
