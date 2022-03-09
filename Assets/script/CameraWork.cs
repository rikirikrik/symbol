using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour
{
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        // 変数にPlayerオブジェクトのtransformコンポーネントを代入
		target = GameObject.Find("player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // カメラのx座標をPlayerオブジェクトのx座標から取得y座標とz座標は現在の状態を維持
		//transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
        transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
    }
}
