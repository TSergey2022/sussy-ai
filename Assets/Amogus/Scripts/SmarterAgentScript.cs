using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmarterAgent : MonoBehaviour {

  public NavMeshAgent agent;

  private Coroutine walkCoroutine;

  void Update() {
    if (Input.GetKeyDown(KeyCode.Alpha2)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out RaycastHit hit)) {
        TryGoToDestination(hit.point);
      }
    }
    if (Input.GetKeyDown(KeyCode.Alpha3)) {
      var target = GameObject.FindWithTag("Target");
      if (target != null) {
        TryGoToDestination(target.transform.position);
      }
    }
  }

  IEnumerator Walk(List<Vector3> path) {
    agent.ResetPath();
    while (path.Count > 0) {
      var target = path[0];
      var navMeshPath = new NavMeshPath();
      while (!(agent.CalculatePath(target, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)) {
        yield return new WaitForFixedUpdate();
      }
      agent.SetPath(navMeshPath);
      while (agent.remainingDistance > 5.0f) {
        yield return new WaitForFixedUpdate();
      }
      path.RemoveAt(0);
    }
    walkCoroutine = null;
  }

  void TryGoToDestination(Vector3 target) {
    (agent.navMeshOwner as Component).TryGetComponent(out ZoneScript ownerZone);
    var agentZoneIdx = ownerZone.zoneIdx;
    var path = new NavMeshPath();
    if (agent.CalculatePath(target, path) && path.status == NavMeshPathStatus.PathComplete) {
      agent.SetPath(path);
    } else {
      foreach (var zone in ZoneScript.zones) {
        var zonePos = zone.neigs[0].From();
        NavMesh.CalculatePath(zonePos, target, NavMesh.AllAreas, path);
        if (path.status != NavMeshPathStatus.PathInvalid) {
          var globalPath = ZoneScript.MakePath(agentZoneIdx, zone.zoneIdx);
          if (globalPath.Count < 1) continue;
          var completePath = new List<Vector3>();
          completePath.AddRange(globalPath);
          completePath.Add(target);
          if (walkCoroutine != null) {
            StopCoroutine(walkCoroutine);
            walkCoroutine = null;
          }
          StartCoroutine(Walk(completePath));
          return;
        }
      }
    }
  }

}
