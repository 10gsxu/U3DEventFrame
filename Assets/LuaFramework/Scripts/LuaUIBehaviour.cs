﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaInterface;

public class LuaUIBehaviour : MonoBehaviour {

	void Awake () {
		
	}

	public void AddButtonListener(LuaFunction action) {
		Button btn = transform.GetComponent<Button> ();
		if (btn != null) {
			btn.onClick.AddListener (delegate() {
				action.Call (gameObject);
			});
		}
	}
		
}
