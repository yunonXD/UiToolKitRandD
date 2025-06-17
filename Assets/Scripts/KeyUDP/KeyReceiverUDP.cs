using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using System.Net;
using System;

/// @brief 1553 키보드 입력 받을 9020 포트 리시버
public class KeyReceiverUDP : MonoBehaviour{
    private const int listenPort = 9020;
    private Thread listenerThread;
    
    // GameManager 참조
    [SerializeField] private GameManager gameManager;

    private void Start(){
        Initialize();
    }

    private void OnDisable(){
        listenerThread?.Abort();
    }
    
    /// @brief UDP 초기화 및 실행
    public void Initialize(){
        listenerThread = new Thread(StartListener);
        listenerThread.IsBackground = true;
        listenerThread.Start();
    }

    /// @brief 데이터 받아올 UDP 리스너
    private void StartListener() {
        using (UdpClient listener = new UdpClient(listenPort)) {
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);
            Debug.Log("Listening for UDP broadcasts on port " + listenPort);

            try {
                while (true) {
                    byte[] bytes = listener.Receive(ref groupEP);
                    ProcessKeyData(bytes);
                }
            }
            catch (SocketException e) {
                Console.WriteLine(e);
            }
        }
    }

    /// @brief 키 데이터 처리
    /// @detail data가 make_str 또는 break_str 형식의 데이터이므로 해당 데이터를 비교하여 키 이벤트를 처리
    /// @param[in] data 받아온 바이트(데이터)
    private void ProcessKeyData(byte[] data) {
    
        if (data.Length > 0){

            byte[] fixedData = new byte[8];
            Array.Copy(data, fixedData, Math.Min(data.Length, 8));

            //Debug.Log($"[UDP Key] Raw Bytes: {BitConverter.ToString(data)}");

            foreach (var keyTable in KeyTables.KeyTableDictionary.Values)
            {
                if (CompareByteArrays(fixedData, keyTable.make_str))
                {
                    SendKeyDown(keyTable.os_vk_key);

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        gameManager.OnKeyReceived(keyTable.os_vk_key, true);
                    });

                    return;
                }
                else if (CompareByteArrays(fixedData, keyTable.break_str))
                {
                    SendKeyUp(keyTable.os_vk_key);
                    return;
                }

            }
        }
    }


        /// @brief Array 컴페어 메서드
        private bool CompareByteArrays(byte[] array1, byte[] array2) {
            for (int i = 0; i < 8; i++) {
                if (array1[i] != array2[i]) {
                    return false;
                }
            }
            return true;
        }

        /// @brief 가상키 인풋 (매크로 방식)
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;

        /// @brief 가상키 Down
        private static void SendKeyDown(byte keyCode) { keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY | 0, 0); }

        /// @brief 가상키 Up
        private static void SendKeyUp(byte keyCode) { keybd_event(keyCode, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); }

}