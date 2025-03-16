using System.Collections;
using TMPro;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _showTime = 0.2f;

    private Coroutine coroutine;

    private void Awake()
    {
        gameObject.SetActive(false);
        _inventory.OnInventoryFull += () => SetText(MessagesData.INVENORY_FULL);
    }

    public void SetText(string text)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            gameObject.SetActive(false);
        }

        _text.text = text;
        gameObject.SetActive(true);
        coroutine = StartCoroutine(Show());
    }

    private IEnumerator Show()
    {
        yield return new WaitForSeconds(_showTime);

        gameObject.SetActive(false);
    }
}

public static class MessagesData
{
    public static readonly string INVENORY_FULL = "There is no space in the inventory";
    public static readonly string WIN = "Congratulations!\nYou have opened the door and completed the level!";

    public static string NO_REQUIRE_INVENTORY(string id)
    {
        return $"Oh, it looks like you don't have the {id}. You need to find it.";
    }

    public static string NO_REQUIRE_HAND(string idRequired, string idTake)
    {
        return $"Are you stupid? Try opening it with a {idRequired}, not...\nWhat is it? {idTake}? Are you serious?";
    }
}