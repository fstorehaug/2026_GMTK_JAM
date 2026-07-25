using UnityEngine;

public class ColorCountDown : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private CountDown countDown;

    [SerializeField] private Color targetColor;
    [SerializeField] private float changeRate = .25f;

    private Material runtiMaterial;

    private Color initialColor; 

    void Start()
    {
        runtiMaterial = meshRenderer.material;
        initialColor = runtiMaterial.color;
    }

    void Update()
    {
        //float progress = countDown.ExponentialScale();
        //Color targetProgressColor = Color.Lerp(initialColor, targetColor, progress);
        //runtiMaterial.color = Color.Lerp(runtiMaterial.color, targetProgressColor, changeRate * Time.deltaTime);
    }
}
