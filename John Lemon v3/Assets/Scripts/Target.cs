using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    public int numHits = 3;
    public Renderer targetRenderer;

    private float dmgEndTime = -1;

    private Material targetMat;
    private Color defaultMatColor;
    private Color damageColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        // reference this gameobject's material component directly
        // because for SOME FUCKING REASON referencing the actual material and then changing it changes it in real life 
        targetRenderer = GetComponent<Renderer>();

        targetMat = targetRenderer.material;
        defaultMatColor = targetMat.color;
    }

    // Update is called once per frame
    void Update()
    {
        // if done taking damage, revert to regular model color
        if (Time.time >= dmgEndTime && targetMat.color != defaultMatColor)
        {
            targetMat.color = defaultMatColor;
        }
    }

    // make target take damage, destroy if the minimum number of shots has hit
    public void GetShot(int damage)
    {
        numHits--;

        // make target red for feedback
        targetMat.color = damageColor;
        dmgEndTime = Time.time + 0.25f;

        if (numHits <= 0)
        {
            print("Calling RemoveTarget() from Target.");
            GameManager.sGameManager.RemoveTarget(); // tell gameobject there is one less target
            gameObject.SetActive(false); // destroying causes issues. dont do that 
        }
    }
}
