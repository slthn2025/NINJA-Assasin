using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    Transform cam;
    Vector3 camStartPos;
    float distance;

    GameObject[] backgorunds;
    Material[] mat;
    float[] backSpeed;

    float farthesBack;
    [Range(0.01f,0.05f)]
    public float parallaxSpeed;
    
    void Start()
    {
        cam = Camera.main.transform;
        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat =  new Material [backCount];
        backSpeed = new float [backCount];
        backgorunds = new GameObject[backCount];

        for(int i = 0; i < backCount; i ++ ){
            backgorunds[i]=transform.GetChild(i).gameObject;
            mat[i] = backgorunds[i].GetComponent<Renderer>().material; 
        }   
        BackSpeedCalculate(backCount);  

    }


 
    void BackSpeedCalculate(int backCount)
    {
        for(int i = 0; i < backCount; i++){
            if ((backgorunds[i].transform.position.z - cam.position.z) > farthesBack)
            {
                farthesBack = backgorunds[i].transform.position.z - cam.position.z;                
            }
        }
        for(int i = 0; i < backCount; i++){
            backSpeed[i] = 1 - (backgorunds[i].transform.position.z-cam.position.z)/farthesBack; 
        }
    }

     private void LateUpdate() {
        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y,0);
        for(int i = 0; i < backgorunds.Length; i++){
            float speed = backSpeed[i] * parallaxSpeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distance,0)* speed);
        }
    }

}
