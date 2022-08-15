using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class SendScore : MonoBehaviour
{
    public GameObject holder;
    public TMP_InputField name;
    public TextMeshProUGUI resposta;
    // Start is called before the first frame update
    void Start()
    {
        holder = GameObject.Find("Holder");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Enviar()
    {
        //name.text;
        Debug.Log("Enviou");

        resposta.enabled = true;

    }


}
