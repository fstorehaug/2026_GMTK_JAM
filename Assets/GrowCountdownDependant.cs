using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class GrowCountdownDependant : MonoBehaviour
{
    [SerializeField] private CountDown countDown;
    [SerializeField] private float maxScale = 2;
    [SerializeField] private float changeRate = .25f;

    void Update()
    {
        //transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (1 + countDown.ExponentialScale()*maxScale), changeRate * Time.deltaTime);
    }

  
}
