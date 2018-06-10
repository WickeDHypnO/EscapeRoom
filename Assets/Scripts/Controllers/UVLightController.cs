using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UVLightColor
{
    BLUE,
    RED,
    YELLOW,
    WHITE,
    END
}

public class UVLightController : MonoBehaviour
{
    [SerializeField] private UVLightColor m_LightColor;

    public UVLightColor GetLightColor()
    {
        return m_LightColor;
    }

}
