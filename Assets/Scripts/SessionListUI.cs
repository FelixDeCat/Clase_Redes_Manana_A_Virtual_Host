using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SessionListUI : MonoBehaviour
{
    [SerializeField] SessionItemUI model;
    [SerializeField] Transform parent;
    ObjectPool<SessionItemUI> pool;

    [SerializeField] RunnerHandler _runner;

    [SerializeField] List<SessionItemUI> currents = new List<SessionItemUI>();

    private void Start()
    {
        pool = new ObjectPool<SessionItemUI>
            (
                createFunc: () =>
                {
                    SessionItemUI ui = GameObject.Instantiate(model, parent);
                    ui.SubscribeCallback_OnClick(OnClickItemButton);
                    return ui;
                },
                actionOnGet: item =>
                {
                    currents.Add(item);
                },
                actionOnRelease: item =>
                {
                    currents.Remove(item);
                },
                actionOnDestroy: item =>
                {
                    Destroy(item.gameObject);
                }
            );
    }


    void OnClickItemButton(string sessionToConnect)
    {
        _runner.ClientConnectedToSession(sessionToConnect);
        transform.gameObject.SetActive(false);
    }

    public void OnListUpdate(List<SessionInfo> sessionList)
    {
        for (int i = 0; i < sessionList.Count; i++)
        {
            pool.Clear();
            var item = pool.Get();
            item.SetSessionItem(sessionList[i].Name, $"{sessionList[i].PlayerCount}/{sessionList[i].MaxPlayers}");
        }
    }

}
