using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarNode : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI date;
    public GameObject checkmark;
    public GameObject red;
    public GameObject select;
    public GameObject root;
    public Button btn;
    public int day;
    public DateTime counterDayOfMonth;
}
