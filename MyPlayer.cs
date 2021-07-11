using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MyPlayer : MonoBehaviourPun
{
    public float Movespeed = 3.5f;
    public float Turnspeed = 120f;
    public TextMesh Caption = null;

    private void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).name == "Caption")
            {
                Caption = this.transform.GetChild(i).gameObject.GetComponent<TextMesh>();
                Caption.text = string.Format("Bulldog{0}", photonView.ViewID);
            }
        }
    }
    private void Update()
    {
        if (photonView.IsMine == true)
        {
            Controls();
        }         
    }
    private void Controls()
    {
        float vert = Input.GetAxis("Vertical");
        float horz = Input.GetAxis("Horizontal");
        this.transform.Translate(Vector3.forward * vert * Movespeed * Time.deltaTime);
        this.transform.localRotation *= Quaternion.AngleAxis(horz * Turnspeed * Time.deltaTime, Vector3.up);
    }
}
