using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[Serializable]
public struct ZoneBox {
  public int destZoneIdx;
  public NavMeshLink link;
  public bool useEndPosition;

  public Vector3 From() {
    var delta = useEndPosition ? link.endPoint : link.startPoint;
    return link.transform.position + delta;
  }
  public Vector3 To() {
    var delta = useEndPosition ? link.startPoint : link.endPoint;
    return link.transform.position + delta;
  }
}

public class ZoneScript : MonoBehaviour {

  public static List<ZoneScript> zones = new();
  public static int maxIndex = 0;
  
  public int zoneIdx;

  public ZoneBox[] neigs = {};

  void Awake() {
    if (zoneIdx > maxIndex) maxIndex = zoneIdx;
    zones.Add(this);
  }

  static public List<Vector3> MakePath(int startZone, int endZone) {
    int[,] adjacencyMatrix = new int[maxIndex + 1, maxIndex + 1];
    foreach (var zone in zones) {
      foreach (var neig in zone.neigs) {
        adjacencyMatrix[zone.zoneIdx, neig.destZoneIdx] = 1;
      }
    }
    var graph = new Graph(adjacencyMatrix);
    var zonesIdx = graph.BreadthFirstSearch(startZone, endZone);
    var path = new List<Vector3>();
    for (int i = 0; i < zonesIdx.Count - 1; i++) {
      var zoneIdx = zonesIdx[i];
      var zone = zones.Find(zone=>zone.zoneIdx==zoneIdx);
      var neig = Array.Find(zone.neigs, neig=>neig.destZoneIdx==zonesIdx[i+1]);
      path.Add(neig.From());
      path.Add(neig.To());
    }
    return path;
  }

}

class Graph {

  private readonly int[,] adjacencyMatrix;
  private readonly int verticesCount;

  public Graph(int[,] matrix) {
    adjacencyMatrix = matrix;
    verticesCount = matrix.GetLength(0);
  }

  public List<int> BreadthFirstSearch(int start, int target) {
    bool[] visited = new bool[verticesCount];
    int[] parent = new int[verticesCount];
    Queue<int> queue = new();
    queue.Enqueue(start);
    visited[start] = true;
    parent[start] = -1;
    while (queue.Count > 0) {
      int current = queue.Dequeue();
      if (current == target) {
        return ReconstructPath(parent, current);
      }
      for (int i = 0; i < verticesCount; i++) {
        if (adjacencyMatrix[current, i] == 1 && !visited[i]) {
          visited[i] = true;
          parent[i] = current;
          queue.Enqueue(i);
        }
      }
    }
    return null;
  }

  private List<int> ReconstructPath(int[] parent, int current) {
    List<int> path = new();
    while (current != -1) {
      path.Add(current);
      current = parent[current];
    }
    path.Reverse();
    return path;
  }
}