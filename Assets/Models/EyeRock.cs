using System.Collections;
using UnityEngine;

public class EyeRock : MonoBehaviour
{
    private SkinnedMeshRenderer eye;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    [SerializeField] private float _minInterval = 1f;
    [SerializeField] private float _maxInterval = 5f;
    [SerializeField] private float _blinkTime = 0.5f;

    private bool _isBlinking = false;

    private Coroutine actionCoroutine;

    void Start()
    {
        eye = GetComponent<SkinnedMeshRenderer>();
    }
    private void OnEnable()
    {
        // Start the loop when the object is active
        //uncomment for random blinks
        //actionCoroutine = StartCoroutine(DoActionAtRandomIntervals());
    }

    private void OnDisable()
    {
        // Prevent background memory leaks or running loops
        if (actionCoroutine != null)
        {
            StopCoroutine(actionCoroutine);
        }
    }

    private IEnumerator DoActionAtRandomIntervals()
    {
        while (true)
        {
            // Calculate a brand new wait duration for every cycle
            float waitTime = Random.Range(_minInterval, _maxInterval);
            yield return new WaitForSeconds(waitTime);

            if(_isBlinking==false)
                StartCoroutine("Blink",_blinkTime);
        }
    }

private IEnumerator Blink(float animTime)
    {
        _isBlinking = true;

        
        float elapsedTime = 0f;
        float progress = 0f;
        /** blink down

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            eye.SetBlendShapeWeight(0, (progress)*100f);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        **/

        elapsedTime = 0f;
        progress = 0f;

        while (elapsedTime < animTime/2)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            eye.SetBlendShapeWeight(0, 100f-(elapsedTime/animTime) * 200f);
            yield return null;
        }

        _isBlinking =false;
    }

}
