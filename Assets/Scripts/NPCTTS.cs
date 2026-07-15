using System.Diagnostics;
using UnityEngine;
using System;

public class NPCTTS : MonoBehaviour
{
    public int rate = 0;
    public int volume = 100;

    public void Speak(string text)
    {
        UnityEngine.Debug.Log("NPC Speak çaðrýldý: " + text);

        if (string.IsNullOrWhiteSpace(text)) return;

        text = text.Trim();

        text = text
            .Replace("ð", "g").Replace("Ð", "G")
            .Replace("þ", "sh").Replace("Þ", "Sh")
            .Replace("ç", "ch").Replace("Ç", "Ch")
            .Replace("ö", "o").Replace("Ö", "O")
            .Replace("ü", "u").Replace("Ü", "U")
            .Replace("ý", "i").Replace("Ý", "I");

        text = text.Replace("…", ". ");
        text = text.Replace("?", "? ");
        text = text.Replace("!", "! ");
        text = text.Replace("\n", " ");
        while (text.Contains("  ")) text = text.Replace("  ", " ");

        if (text.Length > 180)
            text = text.Substring(0, 180);

        text = text.Replace("\"", "'");

        string cmd =
            "Add-Type -AssemblyName System.Speech; " +
            "$s = New-Object System.Speech.Synthesis.SpeechSynthesizer; " +
            $"$s.Rate = {rate}; " +
            $"$s.Volume = {volume}; " +
            $"$s.Speak(\\\"{text}\\\");";

        Process.Start(new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"" + cmd + "\"",
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }

}
