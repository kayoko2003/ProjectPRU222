using System.Collections.Generic;
using UnityEngine;

public class EvenManager : MonoBehaviour
{
    float currentEventCoolDdown = 0;

    public EventData[] events;

    [Tooltip("How long to wait before this becomes active")]
    public float firstTriggerDelay = 180f;

    [Tooltip("How long to wait between each event.")]
    public float triggerInterval = 30f;

    public static EvenManager instance;

    [System.Serializable]
    public class Event
    {
        public EventData data;
        public float duration, cooldown = 0;
    }

    List<Event> runningEvents = new List<Event>();

    PlayerStats[] allPlayers;

    void Start()
    {
        instance = this;

        currentEventCoolDdown = firstTriggerDelay > 0 ? firstTriggerDelay : triggerInterval;
        allPlayers = FindObjectsOfType<PlayerStats>();
    }

    void Update()
    {
        currentEventCoolDdown -= Time.deltaTime;
        if(currentEventCoolDdown <= 0)
        {
            EventData e = GetRandomEvent();
            if (e && e.CheckIfWillHappen(allPlayers[Random.Range(0, allPlayers.Length)]))
                runningEvents.Add(new Event
                {
                    data = e,
                    duration = e.duration
                });
            currentEventCoolDdown = triggerInterval;
        }

        List<Event> toRemove = new List<Event>();

        foreach (Event e in runningEvents)
        {
            e.duration -= Time.deltaTime;
            if(e.duration <= 0)
            {
                toRemove.Add(e);
                continue;
            }

            e.cooldown -= Time.deltaTime;
            if(e.cooldown <= 0)
            {
                e.data.Activate(allPlayers[Random.Range(0, allPlayers.Length)]);
                e.cooldown = e.data.GetSpawnInterval();
            }
        }

        foreach(Event e in toRemove) runningEvents.Remove(e);
    }
    public EventData GetRandomEvent()
    {
        if(events.Length <= 0) return null;

        List<EventData> possibleEvents = new List<EventData>(events);

        EventData result = possibleEvents[Random.Range(0, possibleEvents.Count)];
        while(!result.IsActive())
        {
            possibleEvents.Remove(result);
            if(possibleEvents.Count > 0)
                result = events[Random.Range(0, possibleEvents.Count)];
            else
                return null;
        }
        return result;
    }
}
