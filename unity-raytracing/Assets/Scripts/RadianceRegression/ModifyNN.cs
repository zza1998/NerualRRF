using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyNN : MonoBehaviour
{
    Renderer renderer3;
    Renderer renderer1;
    Renderer renderer2;
    float next = 1.0f;
    float next_roughness = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        renderer1 = GameObject.Find("sphere").GetComponent<Renderer>();
        renderer2 = GameObject.Find("torus").GetComponent<Renderer>();
        renderer3 = GameObject.Find("torus2").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeValue() {
        Material material = renderer3.material;
        material.SetFloat("collectFlag", next);
        material.SetFloat("roughness", next_roughness);

        Material material1 = renderer1.material;
        material1.SetFloat("collectFlag", next);
        material1.SetFloat("roughness", next_roughness);

        Material material2 = renderer2.material;
        material2.SetFloat("collectFlag", next);
        material2.SetFloat("roughness", next_roughness);
        next = (float)(next == 1.0 ? 7.0:1.0);
        next_roughness = (float)(next_roughness == 1.0 ? 0.5 : 1.0);
    }
}
