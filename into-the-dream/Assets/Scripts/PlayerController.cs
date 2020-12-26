using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float MAX_POS_Y = 7.0f;
    const float MIN_POS_Y = -7.0f;
    public float speed    = 1.0f;
    public GameObject frontObject;
    public GameObject backObject1, backObject2;

    void Update()
    {
        float Y_pos = y_position();

        //float y          = Mathf.Lerp(transform.position.y, Y_pos, Time.deltaTime / speed);
        Vector3 pos        = new Vector3(transform.position.x, Y_pos, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime / speed);

        if (transform.position.y > MAX_POS_Y)
        {
            transform.position = new Vector3(transform.position.x, MAX_POS_Y, transform.position.z);
        }

        if (transform.position.y < MIN_POS_Y)
        {
            transform.position = new Vector3(transform.position.x, MIN_POS_Y, transform.position.z);
        }
    }

    float y_position()
    {
        if      (MicController.noteNumber == 36) return ItdData.Scale.C3;
        else if (MicController.noteNumber == 37) return ItdData.Scale.Cs3;
        else if (MicController.noteNumber == 38) return ItdData.Scale.D3;
        else if (MicController.noteNumber == 39) return ItdData.Scale.Ds3;
        else if (MicController.noteNumber == 40) return ItdData.Scale.E3;
        else if (MicController.noteNumber == 41) return ItdData.Scale.F3;
        else if (MicController.noteNumber == 42) return ItdData.Scale.Fs3;
        else if (MicController.noteNumber == 43) return ItdData.Scale.G3;
        else if (MicController.noteNumber == 44) return ItdData.Scale.Gs3;
        else if (MicController.noteNumber == 45) return ItdData.Scale.A3;
        else if (MicController.noteNumber == 46) return ItdData.Scale.As3;
        else if (MicController.noteNumber == 47) return ItdData.Scale.B3;
        else if (MicController.noteNumber == 48) return ItdData.Scale.C4;
        else if (MicController.noteNumber == 49) return ItdData.Scale.Cs4;
        else if (MicController.noteNumber == 50) return ItdData.Scale.D4;
        else if (MicController.noteNumber == 51) return ItdData.Scale.Ds4;
        else if (MicController.noteNumber == 52) return ItdData.Scale.E4;
        else if (MicController.noteNumber == 53) return ItdData.Scale.F4;
        else if (MicController.noteNumber == 54) return ItdData.Scale.Fs4;
        else if (MicController.noteNumber == 55) return ItdData.Scale.G4;
        else if (MicController.noteNumber == 56) return ItdData.Scale.Gs4;
        else if (MicController.noteNumber == 57) return ItdData.Scale.A4;
        else if (MicController.noteNumber == 58) return ItdData.Scale.As4;
        else if (MicController.noteNumber == 59) return ItdData.Scale.B4;
        else if (MicController.noteNumber == 60) return ItdData.Scale.C5;
        else                                     return 0;
    }
}
