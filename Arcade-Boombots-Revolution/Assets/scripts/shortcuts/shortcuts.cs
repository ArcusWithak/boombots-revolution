using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class shortcuts : MonoBehaviour
{
    private bool boolreturn;

    ////wait background setup
    //wait varibles
    private bool sleepreset;
    public enum returnafterwait
    {
        empty,
        False,
        True
    }
    returnafterwait[] sleepreturn = new returnafterwait[1000];
    bool[] timeractive = new bool[1000];
    //creating active timers
    void Awake()
    {
        for (int I = 0; I < timeractive.Length; I++)
        {
            timeractive[I] = true;
        }
    }

    ////wait active code
    ///
    //setting timeractivity
    public void settingtimeractivity(int timerID, bool newactivity)
    {
        timeractive[timerID] = newactivity;
    }
    //wait restart and resize of the array
    public void SLeeprestart(int arraylength)
    {
        arraylength++;
        sleepreturn = new returnafterwait[arraylength];
        timeractive = new bool[arraylength];
        for (int I = 0; I < timeractive.Length; I++)
        {
            timeractive[I] = true;
        }
    }
    //restarting a counter if it is passed
    public void SleepReset(int timerID)
    {
        sleepreturn[timerID] = returnafterwait.empty;
    }

    //get sleep timer
    public bool ReturnSleep(int index)
    {
        bool value = false;
        if (sleepreturn[index] == returnafterwait.True)
        {
            value = true;
        }

        return value;
    }

    //the activet timer
    public bool Sleep(float time, bool autoreset, int timerID)
    {
        if (timeractive[timerID] == true)
        {
            if (timerID < sleepreturn.Length)
            {
                if (sleepreturn[timerID] == returnafterwait.empty)
                {
                    StartCoroutine(Waittimer(time, timerID));
                    sleepreturn[timerID] = returnafterwait.False;
                }
                else if (sleepreturn[timerID] == returnafterwait.True)
                {
                    if (autoreset == true)
                    {
                        SleepReset(timerID);
                    }
                    return true;
                }
                return false;
            }
            else
            {
                Debug.Log("timer :" + timerID.ToString() + " is inactive");
            }
            return false;
        }
        return false;
    }
    IEnumerator Waittimer(float time, int timerID)
    {
        yield return new WaitForSeconds(time);
        sleepreturn[timerID] = returnafterwait.True;
    }

    ////load scene
    ///
    //setting used varibles for levelloading process
    private bool usepresetlevelpath;
    private string presetpath;
    //loading level based on input string
    public void loadScene(string path)
    {
        if (usepresetlevelpath == false)
        {
            SceneManager.LoadScene(path);
        }
        else if (usepresetlevelpath == true)
        {
            SceneManager.LoadScene(presetpath + path + ".unity");
        }
    }
    //set a standaard path to follow or reset it if input is null
    public void loadScenesetpath(string path)
    {
        if (path != "")
        {
            presetpath = path;
            usepresetlevelpath = true;
        }
        else
        {
            usepresetlevelpath = false;
        }
    }

    ////store stuff in a array
    ///
    //store gameobjects  in to a array
    public GameObject[] storegameobjectinarray(GameObject fillfrom)
    {
        GameObject[] fillin = new GameObject[fillfrom.transform.childCount];
        for (int I = 0; I < fillfrom.transform.childCount; I++)
        {
            fillin[I] = fillfrom.transform.GetChild(I).gameObject;
        }
        return fillin;
    }

    ////active code for camera and player movement
    ///
    //movement function
    float rotationX;
    public void movement3D(GameObject user, float speed)
    {
        if (Input.GetKey(KeyCode.D))
        {
            user.transform.Translate(speed * Time.deltaTime, 0, 0, Space.Self);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            user.transform.Translate(-speed * Time.deltaTime, 0, 0, Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            user.transform.Translate(0, 0, speed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            user.transform.Translate(0, 0, -speed * Time.deltaTime, Space.Self);
        }
    }
    //cameramovement function
    public void cameramovement3D(GameObject user, bool Xaxis, float sensativiteit, float Maxv, float Minv)
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //mouse routation check
            if (Xaxis == true)
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensativiteit, 0);
            }
            else if (Xaxis == false)
            {
                rotationX -= Input.GetAxis("Mouse Y") * sensativiteit;
                rotationX = Mathf.Clamp(rotationX, Minv, Maxv);
                float rotationY = transform.localEulerAngles.y;
                user.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            }
        }
    }

    ////UI updatecode code
    ///
    //cooldowns
    public float Cooldown(float cooldown, Image cooldown_image)
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldown_image.fillAmount -= Time.deltaTime / cooldown;
        }
        return cooldown;
    }
    //healthbar or other resource bar
    public float resourcebarFill(float resourcenumber, float resourcemax, Image resource_bar)
    {
        float resource;
        resource = (resourcenumber / resourcemax) * 100;
        resource = resource / 100;
        resource_bar.fillAmount = (resource - 1) * -1;
        return (resource / 100) / resourcemax;
    }

    public float resourcebarEmpty(float resourcenumber, float resourcemax, Image resource_bar)
    {
        float resource;
        resource = (resourcenumber / resourcemax) * 100;
        resource_bar.fillAmount = resource / 100;
        return (resource / 100) / resourcemax;
    }

    //textupdate
    public void updatetext(Text filltobox, string input)
    {
        filltobox.text = input;
    }

    ////interactions
    ///
    //interactions varibels
    private float timer;
    private bool Once;
    //interaction script activation
    public bool interaction(Collider other, string othertag, float Fieldofinteract, SphereCollider Col, float interactiontime, Text interaction, Image interacttimeimage)
    {
        if (other.gameObject.tag == othertag)
        {
            //Debug.Log("intrange");

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < Fieldofinteract)
            {
                //Debug.Log("inview");
                RaycastHit Hit;
                Debug.DrawRay(transform.position, direction, Color.green, 0.1f);
                Physics.Raycast(transform.position, direction, out Hit, Col.radius);
                if (Hit.collider.gameObject.tag == othertag)
                {
                    //Debug.Log("insight");
                    if (interaction != null)
                    {
                        interaction.text = "press E to: ";
                    }
                    else
                    {
                        Debug.Log("missing interactiontext");
                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        if ((interaction != null) && (interacttimeimage != null))
                        {
                            if (Once == false)
                            {
                                interacttimeimage.fillAmount = 1;
                                Once = true;
                            }
                            timer += Time.deltaTime;
                            interacttimeimage.fillAmount -= Time.deltaTime / interactiontime;
                            if (timer > interactiontime)
                            {
                                interacttimeimage.fillAmount = 0;
                                Once = false;
                                return true;
                            }
                        }
                        else
                        {
                            Debug.Log("missing interactiontext and interactionimage");
                        }
                    }
                    else
                    {
                        timer = 0;
                        interacttimeimage.fillAmount = 0;
                        Once = false;
                        return false;
                    }
                }
            }
        }
        return false;
    }
    //interaction script reset
    public void interactionreset(bool use_image_and_text, Text interaction, Image interacttimeimage)
    {
        if (use_image_and_text == true)
        {
            interaction.text = "";
            interacttimeimage.fillAmount = 0;
        }
        Once = false;
    }
}
