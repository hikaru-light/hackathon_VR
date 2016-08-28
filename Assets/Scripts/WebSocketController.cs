using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;
using WebSocketSharp.Net;

public class WebSocketController : SingletonBehaviour<WebSocketController> {
	WebSocket ws;
	public const string ipAddress = "taptappun.cloudapp.net";
	public const string port = "3001";
	private List<string> receivedMessageList = new List<string>();
	private Dictionary<GameObject, Action<string>> callbackList = new Dictionary<GameObject, Action<string>>();

	public bool IsConnected{
		get{
			return ws != null && ws.IsAlive;
		}
	}

	public void SendMessage(string message) {
		if(!IsConnected) return;
		ws.Send(message);
	}

	public void AddMessageReceiveCallback(GameObject obj, Action<string> callback){
		if (callbackList.ContainsKey (obj)) return;
		callbackList.Add(obj, callback);
	}

	public void RemoveMessageReceiveCallback(GameObject obj){
		if (!callbackList.ContainsKey (obj)) return;
		callbackList.Remove(obj);
	}

	void Update(){
		for (int i = 0; i < receivedMessageList.Count; ++i) {
			foreach (var callback in callbackList) {
				callback.Value(receivedMessageList[i]);
			}
		}
		receivedMessageList.Clear();
	}

	public void Connect () {
		ws = new WebSocket (string.Format("ws://{0}:{1}", ipAddress, port));

		ws.OnOpen += (sender, e) => {
			Debug.Log ("WebSocket Open");
		};

		ws.OnMessage += (sender, e) => {
			lock(receivedMessageList){
				receivedMessageList.Add(e.Data);
			}
		};

		ws.OnError += (sender, e) => {
			Debug.Log ("WebSocket Error Message: " + e.Message);
		};

		ws.OnClose += (sender, e) => {
			Debug.Log ("WebSocket Close");
		};

		ws.Connect ();
	}

	public void Disconnect () {
		if (ws != null) {
			ws.Close ();
			ws = null;
		}
	}

	void OnDestroy () {
		Disconnect ();
	}
}
