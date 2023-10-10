using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    #region Skills

    public DashSkill dash { get; private set; }
    public CloneSkill clone { get; private set; }
    public SwordSkill sword { get; private set; }
    public BlackholeSkill blackhole { get; private set; }
    public CrystalSkill crystalSkill { get; private set; }

    #endregion

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackhole = GetComponent<BlackholeSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public bool IsJoystickConnected()
    {
        // Get the current joystick names
        string[] currentJoystickNames = Input.GetJoystickNames();

        // Compare the length of the arrays to detect changes in connected devices
        if (currentJoystickNames.Length > 0 && currentJoystickNames[0] != "")
        {
            // A new joystick was connected
            Debug.Log("Joystick connected");
            return true;
        }

            // A joystick was disconnected
            Debug.Log("Joystick disconnected");
            return false;
    }
}
