using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic : MonoBehaviour {

  public Vector3 startPoint;
  public Vector3 endPoint;
  public float speed = 16;

  public bool inversedDirection = false;

  void Start() {
    startPoint = transform.position;
  }

  void FixedUpdate() {
    var current = transform.position;
    var target = inversedDirection ? startPoint : endPoint;
    var maxDistanceDelta = speed * Time.fixedDeltaTime;
    transform.position = Vector3.MoveTowards(current, target, maxDistanceDelta);
    if (Vector3.Distance(transform.position, target) < 1e-4) {
      inversedDirection = !inversedDirection;
    }
  }

}
