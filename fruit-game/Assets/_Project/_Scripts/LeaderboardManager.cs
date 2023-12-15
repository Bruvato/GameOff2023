using UnityEngine;
using TMPro;

// NOTE: Make sure to include the following namespace wherever you want to access Leaderboard Creator methods
using Dan.Main;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] _entryRankTextObjects;
        [SerializeField] private TMP_Text[] _entryNameTextObjects;
        [SerializeField] private TMP_Text[] _entryScoreTextObjects;
        [SerializeField] public TMP_InputField _usernameInputField;

        // Make changes to this section according to how you're storing the player's score:
        // ------------------------------------------------------------
        // [SerializeField] private GameManager _exampleGame;

        private int Score => GameManager.newScore;
        // ------------------------------------------------------------

        [SerializeField] private Transform entriesTransform;
        [SerializeField] private GameObject entry;

        private void Start()
        {
            for (int i = 0; i < 99; i++)
            {
                GameObject entryObject = Instantiate(entry, entriesTransform);
                _entryRankTextObjects[i] = entryObject.transform.GetChild(0).GetComponent<TMP_Text>();
                _entryNameTextObjects[i] = entryObject.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                _entryScoreTextObjects[i] = entryObject.transform.GetChild(2).GetComponent<TMP_Text>();

                _entryRankTextObjects[i].text = "";
                _entryNameTextObjects[i].text = "";
                _entryScoreTextObjects[i].text = "";


            }

            LoadEntries();
        }

        private void LoadEntries()
        {
            // Q: How do I reference my own leaderboard?
            // A: Leaderboards.<NameOfTheLeaderboard>

            Leaderboards.SuikaLeaderBoard.GetEntries(entries =>
            {
                // foreach (var t in _entryScoreTextObjects)
                //     t.text = "";

                var length = Mathf.Min(_entryScoreTextObjects.Length, entries.Length);
                for (int i = 0; i < length; i++)
                {
                    _entryRankTextObjects[i].text = $"{entries[i].Rank}. ";
                    _entryNameTextObjects[i].text = $"{entries[i].Username}";
                    _entryScoreTextObjects[i].text = $"{entries[i].Score}";
                }

            });
        }

        public void UploadEntry()
        {
            Leaderboards.SuikaLeaderBoard.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }
    }
}
