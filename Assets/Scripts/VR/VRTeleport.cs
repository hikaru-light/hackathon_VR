using UnityEngine;
using System.Collections;

public class VRTeleport : MonoBehaviour
{
    /// <summary>
    /// trueなら確認用Rayを表示
    /// </summary>
    [SerializeField]
    bool isShowDebugRay = false;

    /// <summary>
    /// 無視するレイヤー
    /// </summary>
    [SerializeField]
    LayerMask exclusionLayer;

    /// <summary>カメラフェード管理クラス</summary>
    [SerializeField]
    VRCameraFade vrCameraFade;

    /// <summary>
    /// 移動用レーザー
    /// </summary>
    [SerializeField]
    LineRenderer LaserRenderer;
    /// <summary>
    /// 移動用レーザーの先端部
    /// </summary>
    [SerializeField]
    Transform laserPoint;
    /// <summary>
    /// レーザー射程距離
    /// </summary>
    [SerializeField]
    float lazerDistance = 500f;
    /// <summary>
    /// レーザーの範囲外に出た際のデフォルトの長さ
    /// </summary>
    [SerializeField]
    Vector3 laserDefaultPosition = new Vector3(0f, 0f, 20f);

    [SerializeField]
    Transform laserStartPos = null;

    /// <summary>
    /// 自身のカメラのTransformの参照
    /// </summary>
    Transform myCameraTrs;

    /// <summary>
    /// レーザー先端部のデフォルトのサイズ
    /// </summary>
    Vector3 originalPointScale;
    /// <summary>
    /// 現在選択中の移動先
    /// </summary>
    VRTeleportTarget currentTarget;
    /// <summary>
    /// 前に選択していた移動先
    /// </summary>
    VRTeleportTarget preTarget;

    public bool IsTeleportLock { get; set; }


    /// <summary>
    /// 初期化
    /// </summary>
    void Awake()
    {
        this.originalPointScale = this.laserPoint.localScale;
        this.myCameraTrs = Camera.main.transform;
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        if (this.IsTeleportLock) { return; }

        // Fire1を話している時にはレーザー非表示
        if (!Input.GetButton("Fire1"))
        {
            // 離したタイミングで選択していた移動先があったらテレポート
            if (this.currentTarget != null)
            {
                //this.transform.parent.position = this.currentTarget.transform.position;
                this.FadeStart(this.currentTarget.transform.position);
            }
            if (this.preTarget != null)
            {
                this.preTarget.HandleOut();
            }
            this.currentTarget = null;
            this.preTarget = null;
            this.LaserRenderer.gameObject.SetActive(false);
            return;
        }

#if UNITY_EDITOR
        // デバッグ用表示
        if (this.isShowDebugRay)
        {
            Debug.DrawRay(this.myCameraTrs.position, this.myCameraTrs.forward * 5, Color.blue, 0.1f);
        }
#endif

        Ray ray = new Ray(this.myCameraTrs.position, this.myCameraTrs.forward);
        RaycastHit hit;
        // レーザー(Ray)を500m分照射
        if (Physics.Raycast(ray, out hit, this.lazerDistance, ~this.exclusionLayer))
        {
            this.currentTarget = hit.collider.GetComponent<VRTeleportTarget>();
            if (this.currentTarget != null && this.currentTarget != this.preTarget)
            {
                this.currentTarget.HandleOver();
            }
            if (this.preTarget != null && this.preTarget != this.currentTarget)
            {
                this.preTarget.HandleOut();
            }
            this.preTarget = this.currentTarget;

            this.laserPoint.position = hit.point;
            this.laserPoint.localScale = this.originalPointScale * hit.distance;
        }
        else
        {
            // 当たらなかった場合
            this.currentTarget = null;
            if (this.preTarget != null)
            {
                this.preTarget.HandleOut();
            }
            this.preTarget = null;
            this.laserPoint.localPosition = laserDefaultPosition;
            this.laserPoint.localScale = Vector3.one;
        }

        // レーザー表示
        this.LaserRenderer.gameObject.SetActive(true);

        if (this.laserStartPos != null)
        {
            this.LaserRenderer.SetPosition(0, this.laserStartPos.position);
        }
        else
        {
            this.LaserRenderer.SetPosition(0, new Vector3(
                    this.myCameraTrs.position.x,
                    this.myCameraTrs.position.y - 0.3f,
                    this.myCameraTrs.position.z));
        }

        this.LaserRenderer.SetPosition(1, this.laserPoint.position);
    }

    void FadeStart(Vector3 movePos)
    {
        var fadeDuration = 0.2f;
        var fadeColor = Color.black;
        this.vrCameraFade.FadeOut(fadeDuration, fadeColor,
            () =>
            {
                this.transform.parent.position = movePos;
                this.vrCameraFade.FadeIn(fadeDuration, fadeColor);
            });
    }
}
