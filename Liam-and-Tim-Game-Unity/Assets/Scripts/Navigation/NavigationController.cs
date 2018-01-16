using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NavigationController : MonoBehaviour {
  public Vector3 m_navGridCenter;
  public string m_mapName;
  public NavGrid m_navGrid;

  public GameObject m_wallSquare;
  public GameObject m_Selected;

  public Vector2 WorldToGridCoord(Vector2 p) { return m_navGrid.WorldToGridCoord(p); }
  public Vector3 GridCoordToWorld(Vector2 p) { return m_navGrid.GridCoordToWorld(p); }

	void Start () {
    m_Selected = null;
    GenerateMap();
    for (int r = -1; r < m_navGrid.m_height + 1; r++) {
      for (int c = -1; c < m_navGrid.m_width + 1; c++) {
        if (r < 0 || r == m_navGrid.m_height ||
            c < 0 || c == m_navGrid.m_width) {
          Instantiate(m_wallSquare, GridCoordToWorld(new Vector2(r, c)), Quaternion.identity);
        } else {
          GridCell cell = m_navGrid.GetGridCell(r, c);
          if (cell.IsWall()) {
            Instantiate(m_wallSquare, GridCoordToWorld(new Vector2(r, c)), Quaternion.identity);
          }
        }
      }
    }
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

          cost = 1;
        } else if (ch == 'F') {

          cost = 1;
        } else if (ch == 'W') {
          isWall = true;
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

  public static List<NavCell> CreatePath(NavCell end) {
    Stack<NavCell> path = new Stack<NavCell>();
    List<NavCell> p = new List<NavCell>();
    path.Push(end);
    end = end.Parent();
    while (end != null) {
      path.Push(end);
      end = end.Parent();
    }
    while (path.Count != 0) {
      p.Add(path.Pop());
    }
    return p;
  }

  public List<NavCell> FindPath(Vector2 start_pos, Vector2 goal_pos) {
    List<NavCell> open_list = new List<NavCell>();
    List<NavCell> closed_list = new List<NavCell>();
    NavCell start = new NavCell(WorldToGridCoord(start_pos));
    NavCell goal = new NavCell(WorldToGridCoord(goal_pos));
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

  void Update () {
		if (Input.GetMouseButtonUp(0)) {
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      RaycastHit2D hit = Physics2D.Raycast(mp, Vector2.zero);
      m_Selected = null;
      if (hit.collider != null) {
        Debug.Log("Found object");
        if (hit.collider.gameObject.GetComponent<NavAgentController>() != null) {
          Debug.Log("Object is a navagent");
          m_Selected = hit.collider.gameObject;
        }
      }
    }
    if (Input.GetMouseButtonUp(1) && m_Selected != null) {
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      m_Selected.GetComponent<NavAgentController>().SetDestination(mp);
      m_Selected.GetComponent<NavAgentController>().FindPath();
      m_Selected.GetComponent<NavAgentController>().Enabled(true);
    }
	}
}
