using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour {
  
  void OnGui() {
    GUI.backgroundColor = Color.black;
    var style = new GUIStyle(GUI.skin.box);
    style.active = style.normal;
    style.fontSize = 20;
    GUI.Box(new Rect(10, 10, 200, 30), "[Alpha2] - go to raycast point", style);
    GUI.Box(new Rect(10, 40, 200, 30), "[Alpha3] - go to \"Target\"-tagged object", style);
  }
  
}
