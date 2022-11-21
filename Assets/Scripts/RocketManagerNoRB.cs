using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RocketManagerNoRB : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtMaxSpeed;
    [SerializeField] private TextMeshProUGUI _txtSpeed;
    [SerializeField] private TextMeshProUGUI _txtElevation;
    [SerializeField] private TextMeshProUGUI _txtFuel;
    [SerializeField] private TextMeshProUGUI _txtResult;
    [SerializeField] private Button _btnOK;

    private int level = 0;
    private int[] fuelLevels = {15, 10, 5};
    private float speed = 0;
    private float accel = 5;
    private int maxSpeed = 5;
    private float fuel;
    private bool collided = false;
    private float position;
    private float fuelSpeed = 20;
    private bool win = true;
    
    private double previousSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _btnOK.onClick.AddListener(MainMenu);
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 0)
        {
            if (previousSpeed > maxSpeed)
            {
                win = false;
                EndLevel("YOU LOSE");
            }
            else
            {
                if(level < 2)
                {
                    EndLevel("Next level"); 
                }
                else
                {
                    EndLevel("YOU WIN"); 
                }
                
            }
            
        }

        if (!collided)
        { 
            speed -= (accel * Time.deltaTime);
            _txtSpeed.text = "Speed: " +  (-speed).ToString("F1");
            previousSpeed = -speed;
            if (Input.GetKey(KeyCode.Space) && fuel > 0)
            {
                speed += fuelSpeed * Time.deltaTime;
                //.velocity += Vector3.up * 50 * Time.deltaTime;
                fuel -= (Time.deltaTime * 5);
            }
        }
        
        position += speed * Time.deltaTime;
        transform.position = new Vector3(0, position, 0);
        _txtMaxSpeed.text = "Max speed: " + maxSpeed;
        _txtFuel.text = "Fuel: " + fuel.ToString("F1");
        _txtElevation.text = "Elevation: " + (transform.position.y - 2.548388f).ToString("F1");
    }
    
    private void EndLevel(string message)
    {
        speed = 0;
        collided = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _txtResult.text = message;
        _txtResult.gameObject.SetActive(true);
        _btnOK.gameObject.SetActive(true);
        //PAUSAR EL JUEGO
        Time.timeScale = 0;
    }

    private void MainMenu()
    {
        if (win && level < 2)
        { 
            level++;
            Restart();
        }
        else
        {
            #if UNITY_EDITOR
                if(EditorApplication.isPlaying) 
                {
                    UnityEditor.EditorApplication.isPlaying = false;
                }
            #else
                Application.Quit();
            #endif
        }
        //SceneManager.LoadScene("MainMenu");
    }

    private void Restart()
    {
        transform.position = new Vector3(0, 22, 0);
        fuel = fuelLevels[level];
        _txtResult.gameObject.SetActive(false);
        _btnOK.gameObject.SetActive(false);
        position = transform.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        collided = false;
    }
}
