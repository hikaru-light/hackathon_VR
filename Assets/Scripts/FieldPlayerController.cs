using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

using Newtonsoft.Json;

public class FieldPlayerController : SingletonBehaviour<FieldPlayerController> {
	[SerializeField] Prefab rivalPlayerPrefab;

	private Dictionary<string, RivalPlayer> rivalPlayers = new Dictionary<string, RivalPlayer>();

	// 同じものを通信したくないので、直前のものとの差分だけ送るようにしておく
	Vector3 prevPosition = Vector3.zero;

	string myId = null;

	public override void SingleAwake() {
		WebSocketController.Instance.Connect();
		WebSocketController.Instance.AddMessageReceiveCallback(this.gameObject, OnMessageReceieved);
		StartCoroutine (GetUserId());
	}

	IEnumerator GetUserId() {
		UnityWebRequest request = UnityWebRequest.Get(string.Format("http://{0}:{1}/user_token", WebSocketController.ipAddress, WebSocketController.port));
		yield return request.Send();

		if(request.isError) {
			Debug.Log(request.error);
		}else {
			myId = request.downloadHandler.text;
		}
	}

	void OnDestroy(){
		myId = null;
		WebSocketController.Instance.RemoveMessageReceiveCallback(this.gameObject);
		WebSocketController.Instance.Disconnect();
	}

	void Update(){
		if (WebSocketController.Instance.IsConnected && myId != null) {
			Vector3 pos = Camera.main.transform.position;
			if (pos != prevPosition) {
				prevPosition = pos;
				Player player = new Player();
				player.id = myId;
				player.position = Axis3.Convert (Camera.main.transform.position);
				string json = JsonConvert.SerializeObject(player);
				WebSocketController.Instance.SendMessage (json);
			}
		}
	}

	private void OnMessageReceieved(string json){
		Player player = JsonConvert.DeserializeObject<Player>(json);
		if (!rivalPlayers.ContainsKey (player.id)) {
			rivalPlayers.Add (player.id, rivalPlayerPrefab.InstantiateTo<RivalPlayer> (this.transform));
		}
		RivalPlayer rivalPlayer = rivalPlayers[player.id];
		rivalPlayer.transform.position = new Vector3(player.position.x, player.position.y, player.position.z);
	}
}
