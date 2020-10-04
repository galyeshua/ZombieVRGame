using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] float m_maxBulletHit = 34f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Enemy>() != null)
        {
            // hit zombie
            collision.transform.GetComponent<Enemy>().hitByBullet(m_maxBulletHit);
            Destroy(this.gameObject, 0.7f);
        }
    }

}
