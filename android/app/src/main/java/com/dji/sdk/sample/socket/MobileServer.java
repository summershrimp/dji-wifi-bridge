package com.dji.sdk.sample.socket;

import android.util.Log;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.net.Socket;

import dji.common.error.DJIError;
import dji.common.util.DJICommonCallbacks;
import dji.sdk.base.DJIBaseProduct;
import dji.sdk.products.DJIAircraft;
import dji.sdk.sdkmanager.DJISDKManager;

/**
 * Created by Summer on 9/8/16.
 */
public class MobileServer {
    private Thread acceptThread;
    private static MobileServer _instance;

    private ServerSocket serverSocket;
    private int port = 19995;
    private boolean ok = false;

    private MobileServer() {
    }

    public int getPort(){
        return port;
    }
    public void start(){
        while(!ok && port < 22233){
            try {
                serverSocket = new ServerSocket(19995);
                acceptThread = new AcceptThread(serverSocket);
                ok = true;
            } catch(IOException e) {
                ++port;
            }
        }
        acceptThread.start();
    }

    public void stop(){
        if(ok && serverSocket != null){
            try {
                serverSocket.close();
            } catch(IOException e) {
                //ignore
            } finally {
                ok = false;
            }
        }
    }


    public static MobileServer getInstance() {
        if(_instance == null) {
            _instance = new MobileServer();
        }
        return _instance;
    }


    class AcceptThread extends Thread {
        private ServerSocket serverSocket;
        public AcceptThread(ServerSocket serverSocket) {
            super();
            this.serverSocket = serverSocket;
        }

        @Override
        public void run() {
            Socket socket;
            while(true) {
                if(!ok) {
                    break;
                }
                try {
                    socket = serverSocket.accept();
                    new TaskThread(socket).start();
                } catch (IOException e) {
                    Log.d("socketexcept", e.toString());
                }

            }
        }
    }

    class TaskThread extends Thread {
        private Socket socket;
        TaskThread(Socket socket) {
            super();
            this.socket = socket;
        }

        BufferedReader socketInput;
        BufferedWriter socketOutput;

        @Override
        public void run() {
            try {
                socketInput = new BufferedReader(new InputStreamReader(socket.getInputStream()));
                socketOutput = new BufferedWriter(new OutputStreamWriter(socket.getOutputStream()));
            } catch (IOException e) {
                Log.d("ioexcpet", e.toString());
            }
            while(true) {
                try{
                    String line = socketInput.readLine();
                    Log.d("socket_recv", line);
                    ActionParser.parse(line, socketOutput);
                } catch (IOException e) {
                    try {
                        socket.close();
                    }catch(IOException e2){
                        Log.d("socket_exception", e.toString());
                    }
                    break;
                } catch (NullPointerException e) {
                    Log.d("socket_exception", e.toString());
                }
            }
        }
    }

}
