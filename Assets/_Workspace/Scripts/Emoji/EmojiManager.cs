using UnityEngine;

public class EmojiManager : MonoBehaviour
{
    [SerializeField] private EmojiPrefab[] _emojiPrefabs;

    private int _currentEmojiPrefab;
    private int _lastEmojiPrefab;
    private void OnEnable()
    {
        EmojiBehaviour.onEmoji += SendEmoji; 
    }
    private void OnDisable()
    {
        EmojiBehaviour.onEmoji -= SendEmoji;
    }
    private void SendEmoji(string text, Sprite sprite)
    {
        EmojiPrefab emojiPrefab = _emojiPrefabs[_currentEmojiPrefab];
        if (!emojiPrefab.isActive)
            emojiPrefab.Render(text, sprite); 

        if (_currentEmojiPrefab < _emojiPrefabs.Length - 1)
            _currentEmojiPrefab++;
        else _currentEmojiPrefab = 0; 
    }
}
