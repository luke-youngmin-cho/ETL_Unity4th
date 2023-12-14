using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoginUI : MonoBehaviour
{
    [SerializeField] TMP_InputField _id;
    [SerializeField] TMP_InputField _pw;
    [SerializeField] Button _login;

    private void Awake()
    {
        _login.onClick.AddListener(() =>
        {
            if (PhotonManager.instance.TrySetUserID(_id.text, _pw.text))
            {
                SceneManager.LoadScene("Lobby");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                // todo -> Pop up alert window that id or pw is wrong.
            }
        });
    }
}
