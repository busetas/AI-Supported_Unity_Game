using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class NPCChatManager : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField questionInput;
    public TMP_Text npcAnswerText;

    [Header("NPC")]
    public NPCTTS npcTTS;

    [Header("API")]
    private string apiKey = ""; // API KEY

    private string santaPrompt = @"
Sen Noel Baba’sın.
Neşeli, sıcak, samimi ve bilge bir karakterin var.
Bazen “Ho ho ho!” gibi ifadeler kullanırsın.
Çocuklara hitap eder gibi konuşursun ama akıllı ve öğreticisin.
Her zaman Noel Baba rolünde kalırsın.
Asla yapay zekâ olduğunu söylemezsin.
Rolünden asla çıkmazsın.
Cevapların her zaman Türkçe olur.
Eğer bir sorunun cevabını bilmiyorsan,
bunu eğlenceli bir şekilde söylersin (örneğin: “Bunu elflerime sorayım”).
Cevapların pozitif, eğlenceli ve masalsı olsun.
Asla korkutucu, karanlık, kaba veya sert konuşmazsın.
Her zaman Noel Baba gibi cevap verirsin.
Cevapların en fazla 2 cümle olsun, kısa, net ve anlaşılır olsun, gereksiz uzun hikâyeler anlatma.
";

    void Start()
    {
    }

    public void AskQuestion()
    {
        if (string.IsNullOrEmpty(questionInput.text))
        {
            npcAnswerText.text = "Noel Baba: Ho ho ho! Önce bana bir soru sor";
            return;
        }

        npcAnswerText.text = "Noel Baba düşünüyor...";
        StartCoroutine(GetTurkishAnswer(questionInput.text));
        questionInput.text = "";
    }

    IEnumerator GetTurkishAnswer(string question)
    {
        string url =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key="
            + apiKey;

        string jsonData =
        @"{
            ""contents"": [{
                ""role"": ""user"",
                ""parts"": [{
                    ""text"": """ + santaPrompt + @"\nOyuncunun sorusu: " + question + @"""
                }]
            }]
        }";

        UnityWebRequest request = CreatePostRequest(url, jsonData);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API HATASI: " + request.error);
            Debug.LogError("RESPONSE: " + request.downloadHandler.text);

            npcAnswerText.text = "Noel Baba: Elfler biraz karışıklık çıkardı, tekrar dener misin?";
            yield break;
        }

        string turkishAnswer = ExtractText(request.downloadHandler.text);
        npcAnswerText.text = "Noel Baba: " + turkishAnswer;

        Debug.Log("🇹🇷 TR: " + turkishAnswer);

        StartCoroutine(GetEnglishVersion(turkishAnswer));
    }

    IEnumerator GetEnglishVersion(string turkishText)
    {
        string url =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key="
            + apiKey;

        string translatePrompt =
        "Translate the following text into natural, friendly spoken English. " +
        "Do not explain anything, only give the translation:\n\n" +
        turkishText;

        string jsonData =
        @"{
            ""contents"": [{
                ""role"": ""user"",
                ""parts"": [{
                    ""text"": """ + translatePrompt + @"""
                }]
            }]
        }";

        UnityWebRequest request = CreatePostRequest(url, jsonData);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("İngilizce çeviri alınamadı");
            yield break;
        }

        string englishAnswer = ExtractText(request.downloadHandler.text);
        Debug.Log("🇬🇧 EN: " + englishAnswer);

        if (npcTTS != null)
        {
            npcTTS.Speak(englishAnswer);
        }
        else
        {
            Debug.LogError("npcTTS bağlı değil!");
        }
    }
    UnityWebRequest CreatePostRequest(string url, string json)
    {
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        return request;
    }
    string ExtractText(string response)
    {
        string marker = "\"text\": \"";
        int start = response.IndexOf(marker) + marker.Length;
        int end = response.IndexOf("\"", start);
        return response.Substring(start, end - start);
    }

    void Update()
    {
        
    }
}
