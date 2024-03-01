using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtons : MonoBehaviour
{
    // Define a delegate type that matches the signature of the function you want to point to
    public delegate void MyFunctionDelegate();

    // Declare a variable of the delegate type
    public MyFunctionDelegate functionPointer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        functionPointer();
        // Add your code here to execute when the button is clicked
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
