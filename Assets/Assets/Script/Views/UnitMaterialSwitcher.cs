using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMaterialSwitcher : MonoBehaviour {

    [SerializeField]
    Material m_PlayerMat;

    [SerializeField]
    Material m_OpponentMat;

    [SerializeField]
    Renderer m_MaterialToSwitch;

    public void SetMaterial(bool IsPlayer = true)
    {
        Material[] mat = m_MaterialToSwitch.materials;
        mat[0] = IsPlayer ? m_PlayerMat : m_OpponentMat;
        m_MaterialToSwitch.materials = mat;
    }
}
