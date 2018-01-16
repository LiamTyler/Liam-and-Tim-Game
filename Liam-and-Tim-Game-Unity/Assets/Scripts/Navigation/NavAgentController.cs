using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavAgentController : MonoBehaviour {
  public NavigationController m_navController;
  private bool m_enabled;
  public Stack<NavCell> m_path;
  public float m_speed;
  private Rigidbody2D m_rb;
  public float m_goalRadius;

  public void Enabled(bool b) { m_enabled = b; }
  public bool Enabled() { return m_enabled; }
  public void SetPath(Stack<NavCell> p) { m_path = p; }

	// Use this for initialization
	void Start () {
		m_path = null;
    m_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
    if (m_enabled) {
      Vector2 player_pos = transform.position;
      Vector2 target_pos = m_navController.GridCoordToWorld(m_path.Peek().Pos());
      Vector2 dir = (target_pos - player_pos);
      if (dir.magnitude < m_goalRadius) {
        m_path.Pop();
        if (m_path.Count == 0) {
          m_enabled = false;
          m_rb.velocity = new Vector2(0,0);
          return;
        }
        target_pos = m_navController.GridCoordToWorld(m_path.Peek().Pos());
        dir = (target_pos - player_pos);
      }
      dir.Normalize();
      m_rb.velocity = new Vector2(0,0);
      m_rb.velocity += m_speed * Time.fixedDeltaTime* dir;
    }
	}
}
