using Unity.Netcode;
using UnityEngine;
using Newtonsoft.Json;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System;

public class VoiceChatSystem : NetworkBehaviour
{
    private int FREQUENCY = 44100 / 4; //Default 44100
    private int length = 1;
    AudioClip recordedClip;
    private float startTime;
    [SerializeField] private bool isRecording;

    private void Start()
    {
        //baþlangýç fonksiyonu yazarsak diye
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            if (Input.GetKey(KeyCode.V) && !isRecording)
            {
                isRecording = true;
                StartRecording();
            }

            if (Input.GetKeyUp(KeyCode.V) && isRecording)
            {
                isRecording = false;

                Microphone.End(null);
                // Kayýt iþlemini durdurun
                EndRecording();
            }

            if (!Microphone.IsRecording(null) && isRecording)
            {
                isRecording = false;
                // Kayýt iþlemini durdurun
                EndRecording();
            }
        }

    }

    private void EndRecording()
    {
        float recordingTime = Time.time - startTime;

        byte[] soundByte = GetBytesFromAudioClip(recordedClip);

        if (soundByte != null)
        {
            //byte[] compressedData = Compress(soundByte);
            byte[] compressedData = soundByte;
            SendServerRPC(compressedData, recordedClip.channels);
        }
    }

    private void StartRecording()
    {
        recordedClip = Microphone.Start(null, false, length, FREQUENCY);

        // Kayýt için bir sayaç baþlatýn
        startTime = Time.time;
    }

    //RPC

    //Oyuncunun kendini duyma kodu gelicek görevli kisi: Ben Resul


    [ClientRpc]
    public void SendClientRPC(byte[] ba, int chan)
    {
        if (!IsOwner)
        {
            ReciveData(ba, chan);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void SendServerRPC(byte[] ba, int chan)
    {
        //ReciveData(ba, chan);
        SendClientRPC(ba, chan);
    }

    void ReciveData(byte[] ba, int chan)
    {
        //byte[] decompressedData = Decompress(ba);
        byte[] decompressedData = ba;
        AudioClip a = CreateAudioClipFromBytes(decompressedData, FREQUENCY, chan);

        if (a == null) return;

        GetComponent<AudioSource>().clip = a;
        GetComponent<AudioSource>().Play();

    }


    //RPC





    public static AudioClip CreateAudioClipFromBytes(byte[] bytes, int frequency, int channels)
    {
        float[] samples = new float[bytes.Length / sizeof(float)];
        try
        {
            Buffer.BlockCopy(bytes, 0, samples, 0, (int)bytes.Length);
        }
        catch (Exception)
        {
            GameManager.instance.SendMessageToChat("AudioClip Convert error", 1, true, true);
            print("Audio clip e çevirme Datada hata var");
            return null;
            throw;
        }
        

        AudioClip audioClip = AudioClip.Create("AudioClip", samples.Length, channels, frequency, false);
        audioClip.SetData(samples, 0);

        return audioClip;
    }


    public static byte[] GetBytesFromAudioClip(AudioClip audioClip)
    {
        float[] samples = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(samples, 0);

        byte[] bytes = new byte[samples.Length * sizeof(float)];

        try
        {
            Buffer.BlockCopy(samples, 0, bytes, 0, (int)bytes.Length);
        }
        catch (Exception)
        {
            GameManager.instance.SendMessageToChat("Byte Conver error", 1, true, true);
            print("Byte e çevirme Datada hata var");
            return null;
            throw;
        }
        

        return bytes;
    }


    public static byte[] Compress(byte[] data)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
            {
                gzipStream.Write(data, 0, data.Length);
            }

            return memoryStream.ToArray();
        }
    }

    public static byte[] Decompress(byte[] data)
    {
        using (var memoryStream = new MemoryStream(data))
        {
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                var decompressedData = new byte[data.Length * 2];
                var bytesRead = gzipStream.Read(decompressedData, 0, decompressedData.Length);

                return decompressedData.Take(bytesRead).ToArray();
            }
        }
    }
}
