using UnityEngine;
using System.Collections;

public class VRTeleportTarget : MonoBehaviour
{
    public enum GazeType
    {
        // 注視解除
        Out = 0,
        // 注視
        Over = 1,
        // テレポート可能
        Possible = 2,
    }
    // 現在の注視状態
    public GazeType NowGazeType
    {
        get; set;
    }

    [SerializeField]
    ParticleSystem particle;
    [SerializeField]
    Color particleColorOver;
    [SerializeField]
    Color particleColorOut;

    /// <summary>ターゲット注視</summary>
    public void HandleOver()
    {
        if (!this.particle.gameObject.activeSelf) { return; }
        this.particle.startColor = this.particleColorOver;
        this.NowGazeType = GazeType.Over;
    }
    /// <summary>ターゲット注視解除</summary>
    public void HandleOut()
    {
        if (!this.particle.gameObject.activeSelf) { return; }
        this.particle.startColor = this.particleColorOut;
        this.NowGazeType = GazeType.Out;
    }

    /// <summary>
    /// プレイヤーの非表示圏内に入ったら非表示
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "TargetHideTrigger")
        {
            this.particle.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// プレイヤーの非表示圏内から出たら再表示
    /// </summary>
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "TargetHideTrigger")
        {
            this.particle.gameObject.SetActive(true);
        }
    }
}