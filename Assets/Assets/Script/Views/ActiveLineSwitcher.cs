using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveLineSwitcher : MonoBehaviour {

    [SerializeField]
    Material m_normalMat;

    [SerializeField]
    Material m_activeMat;


    private Material m_material;
    public void SetActive()
    {
        this.gameObject.GetComponent<Renderer>().material = m_activeMat;
    }
    public void SetInactive()
    {
        this.gameObject.GetComponent<Renderer>().material = m_normalMat;
    }
}
