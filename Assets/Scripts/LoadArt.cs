using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LoadArt : MonoBehaviour
{
    [SerializeField] private int _width = 150;
    [SerializeField] private int _height = 150;

    [SerializeField] private PlayerCards _playerCards;

    private void Start()
    {
        var _url = $"https://picsum.photos/{_width}/{_height}";
        foreach (var card in _playerCards.Cards)
        {
            StartCoroutine(DownloadImage(card, _url));
        }       
    }

    private IEnumerator DownloadImage(Card card, string url)
    {
        var request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        { 
            Debug.Log(request.error); 
        }
        else
        {          
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var sprite = Sprite.Create(texture, new Rect(0, 0, _width, _height), new Vector2(0, 0));            
            card.UpdateIconImage(sprite);               
        }            
    }

}
