using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public List<GameObject> backgroundImages = new List<GameObject>();

    public float BackgroundSpeed;

    private void Update()
    {
        for(int i = 0; i < backgroundImages.Count; i++)
        {
            backgroundImages[i].transform.localPosition += BackgroundSpeed * Time.deltaTime * Vector3.down;

            if (backgroundImages[i].transform.localPosition.y < -12.65f)
            {
                backgroundImages[i].transform.localPosition = new Vector3(backgroundImages[i].transform.localPosition.x, 15.36f,
                                                                                backgroundImages[i].transform.localPosition.z);
            }
        }
    }
}
