using System.Collections;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private const int MoveSpeed = 10;
    private const int ShrinkSpeed = 5;
    private const int RotationSpeed = 100;
    private const float ShrinkScale = 0.1f;
    public bool IsMoving { get; private set; }
    public bool IsSpinning { get; private set; }

    public void MoveToPoint(Vector3 destination)
    {
        if (IsMoving) return;
        IsMoving = true;
        StartCoroutine(MoveTo(destination));
    }

    public void Remove()
    {
        IsMoving = true;
        StartCoroutine(Shrink());
    }

    public void StartSpinning()
    {
        IsSpinning = true;
        StartCoroutine(Spin());
    }

    public void StopSpinning()
    {
        StopAllCoroutines();
        transform.rotation = new Quaternion();
        IsSpinning = false;
    }

    private IEnumerator Shrink()
    {
        while (transform.localScale.x > ShrinkScale)
        {
            transform.localScale -= Vector3.one*Time.deltaTime*ShrinkSpeed;
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator MoveTo(Vector3 destination)
    {
        while (destination != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, MoveSpeed*Time.deltaTime);
            yield return null;
        }
        IsMoving = false;
    }

    private IEnumerator Spin()
    {
        while (IsSpinning)
        {
            transform.Rotate(Vector3.forward*Time.deltaTime*RotationSpeed);
            yield return null;
        }
    }
}