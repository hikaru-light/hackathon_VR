using UnityEngine;
using System.Collections;

public class Creator : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;

    [SerializeField]
    Transform setParent;

    [SerializeField]
    Transform posParent;

    void Start()
    {
        for (int i = 0; i < this.posParent.childCount; ++i)
        {
            var child = this.posParent.GetChild(i);

            var obj = Instantiate<GameObject>(this.prefab);
            var trs = obj.transform;
            trs.SetParent(this.setParent, false);
            trs.localPosition = child.localPosition;
            trs.localScale = child.localScale;
            trs.GetChild(0).transform.localPosition = Vector3.zero;

            obj.name = child.name;
        }
    }
}
