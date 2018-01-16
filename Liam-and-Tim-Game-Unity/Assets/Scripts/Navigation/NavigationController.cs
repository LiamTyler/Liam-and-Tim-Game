using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NavigationController : MonoBehaviour {
  public Vector3 m_navGridCenter;
  public string m_mapName;
  public NavGrid m_navGrid;
  public Vector3 m_Start;
  public Vector3 m_Finish;

  public static Stack<NavCell> CreatePath(NavCell node) {
    Stack<NavCell> path = new Stack<NavCell>();
    path.Push(node);
    node = node.Parent();
    while (node.Parent() != null) {
      path.Push(node);
      node = node.Parent();
    }
    return path;
  }

  public Vector2 WorldToGridCoord(Vector2 p) { return m_navGrid.WorldToGridCoord(p); }
  public Vector3 GridCoordToWorld(Vector2 p) { return m_navGrid.GridCoordToWorld(p); }

	void Start () {
		// GenerateMap();
	}

  public void GenerateMap() {
	  StreamReader reader = new StreamReader(m_mapName);
    string line;
    line = reader.ReadLine();
    string[] subs = line.Split(' ');
    int width = int.Parse(subs[0]);
    int height = int.Parse(subs[1]);
    int dx = 1;
    int dy = 1;
    m_navGrid = new NavGrid(width, height, dx, dy, m_navGridCenter);
    for (int r = 0; r < height; r++) {
      line = reader.ReadLine();
      char[] chars = line.ToCharArray();
      for (int c = 0; c < width; c++) {
        float cost = 0;
        bool isWall = false;
        char ch = chars[c];
        if (ch == 'S') {
          // m_Start = Instantiate(m_startSquare, m_navGrid.GridCoordToWorld(r, c), Quaternion.identity);
          m_Start = GridCoordToWorld(new Vector2(r, c));
          cost = 1;
        } else if (ch == 'F') {
          // m_Finish = Instantiate(m_finishSquare,  m_navGrid.GridCoordToWorld(r, c), Quaternion.identity);
          m_Finish = GridCoordToWorld(new Vector2(r, c));
          cost = 1;
        } else if (ch == 'W') {
          isWall = true;
          // Instantiate(m_wallSquare,  m_navGrid.GridCoordToWorld(r, c), Quaternion.identity);
        } else {
          cost = int.Parse(ch.ToString());
        }
        m_navGrid.SetGridCell(r, c, cost, isWall);
      }
    }
    reader.Close();
  }

  bool NavCellInList(List<NavCell> list, NavCell node, out int index) {
    index = -1;
    for (int i = 0; i < list.Count; i++) {
      if (list[i].Pos() == node.Pos()) {
        index = i;
        return true;
      }
    }   
    return false;
  }

  NavCell GetLowestCost(List<NavCell> list) {
    int LowestIndex = 0;
    for (int i = 1; i < list.Count; i++) {
      if (list[i].F() < list[LowestIndex].F()) {
        LowestIndex = i;
      }
    }
    NavCell lowest = list[LowestIndex];
    list.RemoveAt(LowestIndex);
    return lowest;
  }

  float Heuristic(NavCell current, NavCell goal) {
    Vector2 d = current.Pos() - goal.Pos();
    return Mathf.Abs(d.x) + Mathf.Abs(d.y);
  }

  public Stack<NavCell> FindPath(NavCell start, NavCell goal) {
    List<NavCell> open_list = new List<NavCell>();
    List<NavCell> closed_list = new List<NavCell>();
    start.G(0);
    start.F(start.G() + Heuristic(start, goal));
    open_list.Add(start);
    
    while (open_list.Count != 0) {
      NavCell current = GetLowestCost(open_list);
      if (current.Pos() == goal.Pos()) {
        return CreatePath(current);
      }
      closed_list.Add(current);
      List<NavCell> neighbors = m_navGrid.GetNeighbors(current);
      for(int i = 0; i < neighbors.Count; i++) {
        NavCell neighbor = neighbors[i];
        int index;
        if (!NavCellInList(closed_list, neighbor, out index)) {
          if (!NavCellInList(open_list, neighbor, out index)) {
            neighbor.F(neighbor.G() + Heuristic(neighbor, current));
            open_list.Add(neighbor);
          } else {
            NavCell open_neighbor = open_list[index];
            if (neighbor.G() < open_neighbor.G()) {
              open_neighbor.G(neighbor.G());
              open_neighbor.Parent(current);
            }
          }
        }
      }
    }
    return null;
  }	
}
