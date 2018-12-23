package com.dji.sdk.sample.socket;

import java.io.BufferedOutputStream;
import java.io.IOException;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.HashSet;
import java.util.Set;

import dji.sdk.camera.DJICamera;

/**
 * Created by Summer on 9/9/16.
 */
public class VideoServer {

    static private VideoServer _instance;
    DJICamera camera;
    ServerSocket videoSocket;
    boolean ok = false;
    Set<Socket> clients = new HashSet<Socket>();
    private DJICamera.CameraReceivedVideoDataCallback mReceivedVideoDataCallback = null;

    public static VideoServer getInstance() {
        if(_instance == null)
            _instance = new VideoServer();
        return _instance;
    }

    public void sendData(byte[] bytes, int size) {
        Set<Socket> dropped = new HashSet<>();
        if(!ok)
            return;
        for(Socket s : clients) {
            try{
                s.getOutputStream().write(bytes, 0, size);
            } catch(IOException e) {
                e.printStackTrace();
                dropped.add(s);
            }
        }
        clients.removeAll(dropped);
    }

    public void start() {
        if(videoSocket != null)
            return ;
        try {
            videoSocket = new ServerSocket(19996);
            ok = true;
            new AcceptThread().start();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void stop() {
        if(!ok)
            return;
        try {
            videoSocket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        ok = false;
    }

    class AcceptThread extends Thread {
        @Override
        public void run()
        {
            while(ok) {
                try {
                    Socket socket = videoSocket.accept();
                    clients.add(socket);
                } catch (IOException e) {
                    try {
                        videoSocket.close();
                        clients.clear();
                    } catch (IOException ee){
                        e.printStackTrace();
                    }
                    ok = false;
                }
            }
        }
    }
}
