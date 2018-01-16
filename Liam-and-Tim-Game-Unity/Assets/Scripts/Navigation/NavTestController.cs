using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTestController : MonoBehaviour {
  public class PathCell {
    public NavCell cell;
    public GameObject square;
  }
  public NavigationController m_navController;
  public GameObject m_startSquare;
  public GameObject m_finishSquare;
  public GameObject m_wallSquare;
  public GameObject m_walkSquare;

  private GameObject m_Start;
  private GameObject m_Finish;

  private Stack<PathCell> m_Path;

  private void ClearPath() {
    while (m_Path.Count != 0) {
      PathCell c = m_Path.Pop();
      if (c.square != null)
        Destroy(c.square);
    }
  }

	// Use this for initialization
	void Start () {
		m_Path = new Stack<PathCell>();
    m_navController.GenerateMap();
    for (int r = 0; r < m_navController.m_navGrid.m_height; r++) {
      for (int c = 0; c < m_navController.m_navGrid.m_width; c++) {
        GridCell cell = m_navController.m_navGrid.GetGridCell(r, c);
        if (cell.IsWall()) {
          Instantiate(m_wallSquare, m_navController.GridCoordToWorld(new Vector2(r, c)), Quaternion.identity);
        }
      }
    }
    m_Start = Instantiate(m_startSquare, m_navController.m_Start, Quaternion.identity);
    m_Finish = Instantiate(m_finishSquare, m_navController.m_Finish, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0)) {
      ClearPath();
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      mp = m_navController.WorldToGridCoord(mp);
      mp = m_navController.GridCoordToWorld(mp);
      m_Start.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
    if (Input.GetMouseButtonUp(1)) {
      ClearPath();
      Vector2 mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Debug.Log(mp);
      mp = m_navController.WorldToGridCoord(mp);
      Debug.Log(mp);
      mp = m_navController.GridCoordToWorld(mp);
      Debug.Log(mp);
      m_Finish.transform.position = new Vector3(mp.x, mp.y, -.1f);
    }
    if (Input.GetKeyUp(KeyCode.Space)) {
      NavCell start = new NavCell(m_navController.WorldToGridCoord(m_Start.transform.position));
      NavCell goal = new NavCell(m_navController.WorldToGridCoord(m_Finish.transform.position));
      Stack<NavCell> path = m_navController.FindPath(start, goal);
      if (path == null) {
        Debug.Log("No path found");
      } else {
        Debug.Log("Path found");
        NavCell[] path_arr = path.ToArray();
        m_Path.Push(new PathCell { cell = path_arr[path_arr.Length - 1], square = null });
        for (int i = path_arr.Length - 2; i >= 0; --i) {
          GameObject sq = Instantiate(m_walkSquare, m_navController.GridCoordToWorld(path_arr[i].Pos()), Quaternion.identity);
          m_Path.Push(new PathCell { cell = path_arr[i], square = sq });
        }
      }
    }
	}
}
