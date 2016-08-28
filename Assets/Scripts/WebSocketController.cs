﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;
using WebSocketSharp.Net;

public class WebSocketController : SingletonBehaviour<WebSocketController> {
	WebSocket ws;
	private const string WSAddress = "ws://192.168.220.49:3000";
	private List<string> receivedMessageList = new List<string>();
	private Dictionary<GameObject, Action<string>> callbackList = new Dictionary<GameObject, Action<string>>();

	public override void SingleAwake() {
		Connect();
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
		ws = new WebSocket (WSAddress);

		ws.OnOpen += (sender, e) => {
			Debug.Log ("WebSocket Open");
		};

		ws.OnMessage += (sender, e) => {
			Debug.Log ("WebSocket Message Type: " + e.Type + ", Data: " + e.Data);
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
		ws.Close ();
		ws = null;
	}

	void OnDestroy () {
		Disconnect ();
	}
}