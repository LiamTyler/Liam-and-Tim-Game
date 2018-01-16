using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgentController : MonoBehaviour {
  public NavigationController m_navController;
  public float m_speed;
  public float m_goalRadius;

  private List<NavCell> m_path;
  private int m_path_index;
  private Rigidbody2D m_rb;
  private bool m_enabled;
  private Vector2 m_destination;

  public GameObject m_walkSquare;
  public GameObject m_startSquare;
  public GameObject m_finishSquare;
  private Queue<GameObject> m_PathSquares;

  public void Enabled(bool b) { m_enabled = b; }
  public bool Enabled() { return m_enabled; }
  public void SetDestination(Vector2 d) { m_destination = d; }
  public Vector2 GetDestination() { return m_destination; }
  public List<NavCell> GetPath() { return m_path; }

	// Use this for initialization
	void Start () {
    m_PathSquares = new Queue<GameObject>();
		m_path = null;
    m_destination = new Vector2(0, 0);
    m_enabled = false;
    m_path_index = -1;
    m_rb = GetComponent<Rigidbody2D>();
	}

  public void FindPath() {
    if (m_path != null) {
      m_path = null;
      while (m_PathSquares.Count != 0) {
        Destroy(m_PathSquares.Dequeue());
      }
    }
    m_path = m_navController.FindPath(transform.position, m_destination);
    Debug.Log(m_path);
    if (m_path != null) {
      m_path_index = 0;
      for (int i = 0; i < m_path.Count; i++) {
        m_path[i].Pos(m_navController.GridCoordToWorld(m_path[i].Pos()));
      }
      Vector2 p = m_path[0].Pos();
      m_PathSquares.Enqueue(Instantiate(m_startSquare, new Vector3(p.x, p.y, -.1f), Quaternion.identity));
      for (int i = 1; i < m_path.Count - 1; i++) {
        p = m_path[i].Pos();
        m_PathSquares.Enqueue(Instantiate(m_walkSquare, new Vector3(p.x, p.y, -.1f), Quaternion.identity));
      }
      p = m_path[m_path.Count - 1].Pos();
      m_PathSquares.Enqueue(Instantiate(m_finishSquare, new Vector3(p.x, p.y, -.1f), Quaternion.identity));
    }
  }
	
	// Update is called once per frame
	void FixedUpdate () {
    if (m_enabled && m_path != null) {
      Vector2 player_pos = transform.position;
      Vector2 target_pos = m_path[m_path_index].Pos();
      Vector2 dir = (target_pos - player_pos);
      if (dir.magnitude < m_goalRadius) {
        m_path_index++;
        if (m_path_index == m_path.Count) {
          m_enabled = false;
          m_rb.velocity = new Vector2(0,0);
          return;
        }
        target_pos = m_path[m_path_index].Pos();
        dir = (target_pos - player_pos);
      }
      dir.Normalize();
      m_rb.velocity = new Vector2(0,0);
      m_rb.velocity += m_speed * Time.fixedDeltaTime* dir;
    }
	}
}
