using UnityEngine;
using UnityEngine.AI;

public class PlatformRotator : MonoBehaviour {

  private Vector3 rotationCenter;
  private Vector3 rotationStartPos;
  [SerializeField] float rotationSpeed = 1.0f;

  void Start() {
    rotationCenter = transform.position + 10 * Vector3.back;
    rotationStartPos = transform.position;
  }

  void Update() {
    transform.RotateAround(rotationCenter, Vector3.up, Time.deltaTime * rotationSpeed);
  }

}
