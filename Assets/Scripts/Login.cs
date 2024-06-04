using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour

{
    public InputField username;
    public InputField password;
    public GameObject[] canvas;   
    // Start is called before the first frame update
    void Start()
    {
        canvas[0].SetActive(true);
    }
    public void validate()
    {
       string uname = username.text;
       string pass = password.text;
        if(uname =="admin"&& pass == "123")
        {
            Debug.Log(" login succes");

        }else if(uname ==""|| pass == "")
        {
            Debug.Log("please enter username or password");
        }
        else
        {
            Debug.Log("please enter correct user name and password");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
