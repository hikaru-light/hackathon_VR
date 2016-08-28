using UnityEngine;
using System.Collections;

using Newtonsoft.Json;

public class FieldPlayerController : SingletonBehaviour<FieldPlayerController> {
	[SerializeField] Prefab rivalPlayerPrefab;

	Vector3 prevPosition = Vector3.zero;

	public override void SingleAwake() {
		WebSocketController.Instance.Connect();
		WebSocketController.Instance.AddMessageReceiveCallback(this.gameObject, OnMessageReceieved);
	}

	void OnDestroy(){
		WebSocketController.Instance.RemoveMessageReceiveCallback(this.gameObject);
		WebSocketController.Instance.Disconnect();
	}

	void Update(){
		if (WebSocketController.Instance.IsConnected) {
			Vector3 pos = Camera.main.transform.position;
			if (pos != prevPosition) {
				prevPosition = pos;
				string json = JsonConvert.SerializeObject (Axis3.Convert (Camera.main.transform.position));
				WebSocketController.Instance.SendMessage (json);
			}
		}
	}

	private void OnMessageReceieved(string json){
		Debug.Log(json);
	}
}
