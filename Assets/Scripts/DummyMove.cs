using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DummyMove : MonoBehaviour
{
    PhotonView view;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * 5;

            transform.localScale = Vector3.one * (Mathf.Sin(Time.time) * .3f + .6f);
        }
        
    }
}
