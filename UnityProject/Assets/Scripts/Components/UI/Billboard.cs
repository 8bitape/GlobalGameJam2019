using UnityEngine;

public class Billboard : MonoBehaviour
{
    private void Update()
    {
        this.transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);
    }
}
