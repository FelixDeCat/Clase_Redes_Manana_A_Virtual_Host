using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionItemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_name;
    [SerializeField] TextMeshProUGUI txt_players;
    [SerializeField] Button btn_join;

    public void SetSessionItem(string _name, string _playerC)
    {
        txt_name.text = _name;
        txt_players.text = _playerC;
    }

    public void SubscribeCallback_OnClick(Action<string> _joinAction)
    {
        btn_join.onClick.AddListener(() => _joinAction.Invoke(txt_name.text));
    }

    public void Clear()
    {
        txt_name.text = String.Empty;
        txt_players.text = String.Empty;
    }
}
