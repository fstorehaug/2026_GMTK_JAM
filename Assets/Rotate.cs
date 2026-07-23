using System;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private Direction direction;

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Direction.Forward:
                transform.Rotate(Vector3.forward, speed * Time.deltaTime);
                break;
            case Direction.Right:
                transform.Rotate(Vector3.right, speed * Time.deltaTime);
                break;
            case Direction.Up:
                transform.Rotate(Vector3.up, speed * Time.deltaTime);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}

public enum Direction
{
    Forward, Right, Up
}