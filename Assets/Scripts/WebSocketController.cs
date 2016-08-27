using UnityEngine;
using System.Collections;

using WebSocketSharp;
using WebSocketSharp.Net;

public class WebSocketController : SingletonBehaviour<WebSocketController> {
	WebSocket ws;
	private const string WSAddress = "ws://192.168.220.49:3000";

	void Start(){
		Connect ();
	}

	void Connect () {
		ws = new WebSocket (WSAddress);

		ws.OnOpen += (sender, e) => {
			Debug.Log ("WebSocket Open");
		};

		ws.OnMessage += (sender, e) => {
			Debug.Log ("WebSocket Message Type: " + e.Type + ", Data: " + e.Data);
		};

		ws.OnError += (sender, e) => {
			Debug.Log ("WebSocket Error Message: " + e.Message);
		};

		ws.OnClose += (sender, e) => {
			Debug.Log ("WebSocket Close");
		};

		ws.Connect ();
	}

	void Disconnect () {
		ws.Close ();
		ws = null;
	}

	void OnDestroy () {
		Disconnect ();
	}
}
