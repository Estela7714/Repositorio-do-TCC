using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectFixo : MonoBehaviour
{
    [SerializeField] private float larguraAlvo = 16f;
    [SerializeField] private float alturaAlvo = 9f;

    private Camera cam;
    private int ultimaLargura;
    private int ultimaAltura;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        AjustarViewport();
    }

    private void Update()
    {
        if (Screen.width != ultimaLargura || Screen.height != ultimaAltura)
            AjustarViewport();
    }

    private void AjustarViewport()
    {
        ultimaLargura = Screen.width;
        ultimaAltura = Screen.height;

        float targetAspect = larguraAlvo / alturaAlvo;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}