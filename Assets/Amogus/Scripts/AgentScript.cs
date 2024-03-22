using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour {

  public NavMeshAgent agent;

  void Update() {
    if (Input.GetKeyDown(KeyCode.Alpha2)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hit)) {
        agent.SetDestination(hit.point);
      }
    }
    if (Input.GetKeyDown(KeyCode.Alpha3)) {
      var target = GameObject.FindWithTag("Target");
      if (target != null) {
        agent.SetDestination(target.transform.position);
      }
    }
  }

}
