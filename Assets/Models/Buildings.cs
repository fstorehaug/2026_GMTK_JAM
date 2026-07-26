using System.Collections;
using UnityEngine;

public class Buildings : BackgroundManager
{
    [SerializeField] private float _speedAll = 1f;
    [SerializeField] private float _speedDownMult = 1.6f;

    [SerializeField] private float _building1MoveDistance = 7.5f;
    [SerializeField] private float _building2MoveDistance = 3f;
    [SerializeField] private float _building3MoveDistance = 5f;
    [SerializeField] private float _building4MoveDistance = 4.5f;
    [SerializeField] private float _building5MoveDistance = 1.7f;

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


    public override void BeginAnimations()
    {
        StartCoroutine("MoveUp", _speedAll);
    }

    private IEnumerator MoveUp(float speed)
    {
        float elapsedTime = 0f;
        float progress = 0f;
        Vector3 tmpPosition;
        tmpPosition = building1.transform.position;
        float animTime = 3.1f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building1.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * _building1MoveDistance, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;
        tmpPosition = building2.transform.position;
        animTime = 2.5f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building2.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * _building2MoveDistance, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;
        tmpPosition = building3.transform.position;
        animTime = 2.5f; 

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building3.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * _building3MoveDistance, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;
        tmpPosition = building4.transform.position;
        animTime = 2.2f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building4.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * _building4MoveDistance, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.0f);

        elapsedTime = 0f;
        tmpPosition = building5.transform.position;
        animTime = 1.3f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building5.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * _building5MoveDistance, tmpPosition.z);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        speed = speed * _speedDownMult;

        eye1.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam1.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat2.SetActive(true);

        elapsedTime = 0f;
        tmpPosition = building2.transform.position;
        animTime = 1.25f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building2.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -_building2MoveDistance, tmpPosition.z);
            yield return null;
        }

        eye2.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat5.SetActive(true);

        elapsedTime = 0f;
        tmpPosition = building5.transform.position;
        animTime = 0.75f;

        while (elapsedTime <animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building5.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -_building5MoveDistance, tmpPosition.z);
            yield return null;
        }

        eye3.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        splat3.SetActive(true);

        elapsedTime = 0f;
        tmpPosition = building3.transform.position;
        animTime = 1.25f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building3.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -_building3MoveDistance, tmpPosition.z);
            yield return null;
        }

        eye4.StartCoroutine("Blink", 0.3f);      
        yield return new WaitForSeconds(0.2f);
        laserbeam4.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        splat4.SetActive(true);

        elapsedTime = 0f;
        tmpPosition = building4.transform.position;
        animTime = 1.1f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building4.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -_building4MoveDistance, tmpPosition.z);
            yield return null;
        }

        eye5.StartCoroutine("Blink", 0.3f);
        yield return new WaitForSeconds(0.2f);
        laserbeam5.SetActive(true);


        yield return new WaitForSeconds(0.5f);
        splat1.SetActive(true);

        elapsedTime = 0f;
        tmpPosition = building1.transform.position;
        animTime = 2f;

        while (elapsedTime < animTime)
        {
            elapsedTime += Time.deltaTime * speed;
            progress = elapsedTime / animTime;
            
            building1.transform.position = new Vector3(tmpPosition.x, tmpPosition.y + progress * -_building1MoveDistance, tmpPosition.z);
            yield return null;
        }
        
    }
}
