using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LightMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 lightPosition;
    Renderer renderer;
    Renderer renderer1;
    Renderer renderer2;
    Renderer renderer3;
    Renderer renderer4;
    Renderer renderer5;
    Renderer renderer6;
    Renderer renderer7;
    void Start()
    {
        renderer = GameObject.Find("left_wall").GetComponent<Renderer>();
        renderer1 = GameObject.Find("sphere").GetComponent<Renderer>();
        renderer2 = GameObject.Find("torus").GetComponent<Renderer>();
        renderer3 = GameObject.Find("torus2").GetComponent<Renderer>();
        renderer4 = GameObject.Find("center_wall").GetComponent<Renderer>();
        renderer5 = GameObject.Find("right_wall").GetComponent<Renderer>();
        renderer6 = GameObject.Find("ceiling").GetComponent<Renderer>();
        renderer7 = GameObject.Find("floor").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lightPosition = transform.position;

        Material material = renderer.material;
        material.SetVector("pointLightPosition", lightPosition);

        Material material1 = renderer1.material;
        material1.SetVector("pointLightPosition", lightPosition);

        Material material2 = renderer2.material;
        material2.SetVector("pointLightPosition", lightPosition);

        Material material3 = renderer3.material;
        material3.SetVector("pointLightPosition", lightPosition);

        Material material4 = renderer4.material;
        material4.SetVector("pointLightPosition", lightPosition);

        Material material5 = renderer5.material;
        material5.SetVector("pointLightPosition", lightPosition);

        Material material6 = renderer6.material;
        material6.SetVector("pointLightPosition", lightPosition);

        Material material7 = renderer7.material;
        material7.SetVector("pointLightPosition", lightPosition);
    }
}
