using UnityEditor.Build;
using UnityEngine;

public class SetActiveTime : MonoBehaviour
{
    [SerializeField] private CountDown countDown;
    [SerializeField] private float activeAt;
    [SerializeField] private float deactivateAt;

    private bool animationdone = false;

    public GameObject gm;

    void Update()
    {
        if (animationdone)
            return;

        //if (countDown.TimeRemaining < activeAt)
        //{
        //    if (countDown.TimeRemaining < deactivateAt)
        //    {
        //        gm.SetActive(false);
        //        animationdone = false;
        //        return;
        //    }

        //    if (gm.activeSelf == false)
        //        gm.SetActive(true);
        //}

    }
}
