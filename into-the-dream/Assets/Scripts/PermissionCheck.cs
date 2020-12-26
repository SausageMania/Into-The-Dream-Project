using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class PermissionCheck : MonoBehaviour
{
    // 게임이 처음 실행됐을 때 / 퍼미션 허용or거절 후 게임으로 복귀했을 때 호출됨
    void OnApplicationFocus(bool status)
    {
        if (status)
        {
#if PLATFORM_ANDROID
            // 퍼미션이 허용된 상태면 PlayScene으로 이동
            if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {    
                Debug.Log("Permission이 허용되어 MainScene으로 이동");
                SceneManager.LoadScene("MainScene");
            }
#endif
        }
    }

    // OK 버튼을 클릭하면 Permission 요청(Android)
    public void RequestPermission()
    {
#if PLATFORM_ANDROID
        Debug.Log("OK 버튼 클릭");
        Permission.RequestUserPermission(Permission.Microphone);
#endif
    }
}
