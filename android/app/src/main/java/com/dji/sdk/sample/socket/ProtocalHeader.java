package com.dji.sdk.sample.socket;

import java.io.Serializable;

/**
 * Created by Summer on 9/8/16.
 */
public class ProtocalHeader implements Serializable{

    private static final long serialVersionUID = 1L;

    public final static int ACTION_ACK = 0;
    public final static int ACTION_UNLOCK = 1;
    public final static int ACTION_RCDATA = 2;
    public final static int ACTION_GIMBAL = 3;
    public final static int ACTION_RETURNHOME = 4;


    int magic = 0x19941023;
    byte action;
    byte size;
}
