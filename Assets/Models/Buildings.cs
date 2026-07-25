using System.Collections;
using UnityEngine;

public class Buildings : BackgroundManager
{

    [SerializeField] private float _minInterval = 1f;
    [SerializeField] private float _maxInterval = 5f;
    [SerializeField] private float _moveTime = 8f;

    [SerializeField] private GameObject building1;
    [SerializeField] private GameObject building2;
    [SerializeField] private GameObject building3;
    [SerializeField] private GameObject building4;
    [SerializeField] private GameObject building5;

    [SerializeField] private GameObject laserbeam1;
    [SerializeField] private GameObject laserbeam2;
    [SerializeField] private GameObject laserbeam3;
    [SerializeField] private GameObject laserbeam4;
    [SerializeField] private GameObject laserbeam5;

    [SerializeField] private GameObject splat1;
    [SerializeField] private GameObject splat2;
    [SerializeField] private GameObject splat3;
    [SerializeField] private GameObject splat4;
    [SerializeField] private GameObject splat5;

    [SerializeField] private EyeRock eye1;
    [SerializeField] private EyeRock eye2;
    [SerializeField] private EyeRock eye3;
    [SerializeField] private EyeRock eye4;
    [SerializeField] private EyeRock eye5;



    private bool _isMoving = false;

    private Coroutine actionCoroutine;

    public override void BeginAnimations()
    {
        StartCoroutine("MoveUp", _moveTime);
    }
    
    private IEnumerator DoActionAtRandomIntervals()
    {
        while (true)
        {
            // Calculate a brand new wait duration for every cycle
            float waitTime = Random.Range(_minInterval, _maxInterval);
            yield return new WaitForSeconds(waitTime);

            if (_isMoving == false)
                StartCoroutine("MoveUp", _moveTime);
        }
    }

    private IEnumerator MoveUp(float animTime)
    {
        _isMoving = true;
        float elapsedTime = 0f;
        float progress = 0f;
        Vector3 tmpPosition;

        while (elapsedTime < 3.1f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building1.transform.position;
            building1.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * 0.13f, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;

        while (elapsedTime < 2.5f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building2.transform.position;
            building2.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * 0.1f, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;

        while (elapsedTime < 2.5f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building3.transform.position;
            building3.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * 0.15f, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;

        while (elapsedTime < 2.2f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building4.transform.position;
            building4.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * 0.15f, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;

        while (elapsedTime < 1.4f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building5.transform.position;
            building5.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * 0.15f, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(2f);


        eye1.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat2.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < 1.25f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building2.transform.position;
            building2.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -0.7f, tmpPosition.z);
            yield return null;
        }

        eye2.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat5.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < 0.75f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building5.transform.position;
            building5.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -0.7f, tmpPosition.z);
            yield return null;
        }

        eye3.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat3.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < 1.25f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building3.transform.position;
            building3.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -0.6f, tmpPosition.z);
            yield return null;
        }

        eye4.StartCoroutine("Blink", 0.3f);      
        yield return new WaitForSeconds(0.2f);
        laserbeam4.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        splat4.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < 1.1f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building4.transform.position;
            building4.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -0.6f, tmpPosition.z);
            yield return null;
        }

        eye5.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam5.SetActive(true);


        yield return new WaitForSeconds(0.5f);
        splat1.SetActive(true);
        elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / animTime;
            tmpPosition = building1.transform.position;
            building1.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -0.6f, tmpPosition.z);
            yield return null;
        }

        
        

        elapsedTime = 0f;
        progress = 0f;
        
        _isMoving = false;
    }
}
