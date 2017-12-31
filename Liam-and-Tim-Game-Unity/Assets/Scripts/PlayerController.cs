using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_MoveSpeed;
    public GameObject m_Bullet;

    // Use this for initialization
    void Start()
    {

    }

    // handle input
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            //  GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            GameObject bullet = Instantiate(
                m_Bullet,
                transform.position,
                Quaternion.identity);

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * 4;

            Destroy(bullet, 2.0f);
        }
    }
    void FixedUpdate()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        // transform.position += move * m_MoveSpeed;
        this.GetComponent<Rigidbody2D>().velocity = move * m_MoveSpeed;
    }
}
