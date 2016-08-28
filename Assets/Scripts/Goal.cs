using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    [SerializeField]
    GameManager manager;

    /// <summary>
    /// プレイヤーの非表示圏内に入ったら非表示
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "TargetHideTrigger")
        {
            this.manager.ShowGroup(GameManager.SubGroupIndex.Result);
            this.gameObject.SetActive(false);
        }
    }
}
