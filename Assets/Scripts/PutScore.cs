using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PutScore : MonoBehaviour
{
    public GameObject holder;
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        holder = GameObject.Find("Holder");
        score.text = holder.GetComponent<Holder>().scoreCopy.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
