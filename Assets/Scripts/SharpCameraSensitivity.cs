using UnityEngine;
using Cinemachine;

/*
 * Script by user Eudaimonium
 * https://forum.unity.com/threads/free-look-camera-and-mouse-responsiveness.642886/
 * https://pastebin.com/ryGiyMhb
 */
public class SharpCameraSensitivity : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private float aimingSensitivity = 125f;
    [Range(1.5f, 2f)]
    [SerializeField] private float yCorrection = 2f;

    private float m_xAxisValue;
    private float m_yAxisValue;

    private void Awake()
    {
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
        m_xAxisValue = freeLookCamera.m_XAxis.Value;
        m_yAxisValue = freeLookCamera.m_YAxis.Value;
    }

    private void Update()
    {
        float mouseX = freeLookCamera.m_XAxis.m_InvertInput ? -1f : 1f * Input.GetAxis("Mouse X") * aimingSensitivity * Time.deltaTime;
        float mouseY = freeLookCamera.m_YAxis.m_InvertInput ? 1f : -1f * Input.GetAxis("Mouse Y") * aimingSensitivity * Time.deltaTime;

        // Y Correction
        mouseY /= 360f;
        mouseY *= yCorrection;

        m_xAxisValue += mouseX;
        m_yAxisValue = Mathf.Clamp01(m_yAxisValue - mouseY);

        freeLookCamera.m_XAxis.Value = m_xAxisValue;
        freeLookCamera.m_YAxis.Value = m_yAxisValue;
    }
}
