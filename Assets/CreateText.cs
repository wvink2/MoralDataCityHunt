using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class CreateText : MonoBehaviour
{
    public GameObject ChoiceIdentifier1;
    public GameObject ChoiceIdentifier2;
    private TextMeshProUGUI textMesh;


    // Start is called before the first frame update
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void GenerateText()
    {
        CheckChoices();

    }

    public void SetOpaqueText()
    {
        textMesh.text = "Voor u eigen veiligheid maken wij zelf de keuze om informatie met u niet te delen, wij weten het beter";
    }

    private void CheckChoices()
    {
        bool choice1 = ChoiceIdentifier1.activeSelf;
        bool choice2 = ChoiceIdentifier2.activeSelf;

        string keuze1 = choice1 ? "24/7 cameratoezicht voor maximale veiligheid" : "Beperkte cameratoezicht";
        string keuze2 = choice2 ? "Inzetten van slimme camera’s" : "Geen of beperkte inzet van slimme camera’s";

        textMesh.text = $"Om u zo goed mogelijk te informeren staat het op dit bord, u heeft afgelopen vragen gekozen voor { keuze1} en { keuze2}";
    }
}
