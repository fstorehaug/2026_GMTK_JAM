using UnityEngine;
using UnityEngine.InputSystem;

public class FireProjectile : MonoBehaviour
{
    // Only GameObjects with a Rigidbody can be assigned as the projectile.
    public Rigidbody projectile;
    // Speed of the projectile when fired.
    // This is a public variable so it can be adjusted in the Unity Editor.
    public float speed = 4;
    // Update is called once per frame
    // This method checks for input and fires a projectile if the attack action is pressed.
    public void shoot()
    {
            Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
            p.linearVelocity = transform.forward * speed;
        
    }
}