using UnityEngine;


public class Player : MonoBehaviour
{
    //private Manager manager;
    private GameUIManager UIManager;

    private Gun gun;
    RaycastHit m_hit;
    Ray m_ray;

    // Start is called before the first frame update
    void Start()
    {
        UIManager = GameObject.Find("GameManager").GetComponent<GameUIManager>();

        gun = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        checkInputs();
    }

    private void checkInputs()
    {
        if (Input.anyKeyDown)
        {
            bool playerCanShoot = true; // bool for disable shooting spesific objects
            Transform enemy = null;
            Transform camTrans = Camera.main.transform;

            m_ray = new Ray(camTrans.position, camTrans.forward);

            var RaycastRes = Physics.Raycast(m_ray, out m_hit);
            Debug.DrawRay(camTrans.position, camTrans.forward * 10, RaycastRes ? Color.green : Color.red, 5f);

            if (RaycastRes)
            {
                // check if hit have rigidbody
                if (m_hit.rigidbody != null)
                {
                    // Identify hit object by his tag
                    switch (m_hit.transform.tag)
                    {
                        case "AmmoBox":  //hit AmmoBox - Reload
                            playerCanShoot = false;
                            gun.StartCoroutine("Reload"); // call reload in gun
                            break;

                        case "UIButton": //hit button, manager will handle it
                            playerCanShoot = false;
                            UIManager.handle_button(m_hit.transform.name);
                            break;

                        case "Enemy":   //hit enemy - save it for shooting
                            enemy = m_hit.transform; 
                            break;
                    }
                }
            }

            // if playerCanShoot is still true, call shoot in gun and pass enemy
            if (playerCanShoot) 
                StartCoroutine(gun.Shoot(enemy)); 
        }
    }

}
