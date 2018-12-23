package com.dji.sdk.sample.socket;

import android.util.Log;

import java.io.BufferedWriter;
import java.io.IOException;
import java.util.Date;
import java.util.List;

import dji.common.camera.DJICameraSettingsDef;
import dji.common.error.DJIError;
import dji.common.flightcontroller.DJIVirtualStickFlightControlData;
import dji.common.gimbal.DJIGimbalAngleRotation;
import dji.common.gimbal.DJIGimbalRotateAngleMode;
import dji.common.gimbal.DJIGimbalRotateDirection;
import dji.common.gimbal.DJIGimbalSpeedRotation;
import dji.common.util.DJICommonCallbacks;
import dji.sdk.flightcontroller.DJIFlightController;
import dji.sdk.products.DJIAircraft;
import dji.sdk.sdkmanager.DJISDKManager;

/**
 * Created by Summer on 9/8/16.
 */
public class ActionParser {

    private static long lastGimbalActionTime = System.currentTimeMillis();
    private static long lastRCActionTime = System.currentTimeMillis();

    private static void outString(final BufferedWriter out, String msg) {
        try {
            out.write(msg);
            out.newLine();
        } catch (IOException e){
            //ignore;
        }
    }

    private static void handleDJIError(final BufferedWriter out, DJIError djiError) {
        if(null != djiError) {
            Log.d("DJIError", djiError.getDescription());
            outString(out, djiError.getDescription());
        }
    }
    public static void parse(String line, final BufferedWriter out) {
        DJISDKManager sdkManager = DJISDKManager.getInstance();
        DJIAircraft aircraft;
        if(sdkManager.getDJIProduct() instanceof DJIAircraft) {
            aircraft = (DJIAircraft) sdkManager.getDJIProduct();
        } else {
            outString(out, "No Aircraft Connected!");
            return;
        }

        String[] commands = line.split(" ");
        if(commands[0].equals("takephoto")){
            aircraft.getCamera().startShootPhoto(DJICameraSettingsDef.CameraShootPhotoMode.Single, new DJICommonCallbacks.DJICompletionCallback() {
                @Override
                public void onResult(DJIError djiError) {
                    handleDJIError(out, djiError);
                }
            });
        }else if(commands[0].equals("takeoff")){
            aircraft.getFlightController().takeOff(new DJICommonCallbacks.DJICompletionCallback() {
                @Override
                public void onResult(DJIError djiError) {
                    handleDJIError(out, djiError);
                }
            });
        } else if(commands[0].equals("landing")){
            aircraft.getFlightController().autoLanding(new DJICommonCallbacks.DJICompletionCallback() {
                @Override
                public void onResult(DJIError djiError) {
                    if(djiError != null) {
                        handleDJIError(out, djiError);
                    }
                }
            });
        } else if(commands[0].equals("gimbal")) {
            if (commands[1].equals("reset")) {
                DJIGimbalAngleRotation zero = new DJIGimbalAngleRotation(true, 0f, DJIGimbalRotateDirection.Clockwise);
                aircraft.getGimbal().rotateGimbalByAngle(DJIGimbalRotateAngleMode.AbsoluteAngle, zero, zero, zero, new DJICommonCallbacks.DJICompletionCallback() {
                    @Override
                    public void onResult(DJIError djiError) {
                        handleDJIError(out, djiError);
                    }
                });
            } else {
                if(System.currentTimeMillis() - lastGimbalActionTime < 50) {
                    return ;
                }
                try {
                    Float pitch = Float.valueOf(commands[1]);
                    Float roll = Float.valueOf(commands[2]);
                    Float yaw = Float.valueOf(commands[3]);

                    DJIGimbalSpeedRotation mPitch = new DJIGimbalSpeedRotation(Math.abs(pitch), pitch >= 0 ? DJIGimbalRotateDirection.Clockwise : DJIGimbalRotateDirection.CounterClockwise);
                    DJIGimbalSpeedRotation mRoll = new DJIGimbalSpeedRotation(Math.abs(roll), roll >= 0 ? DJIGimbalRotateDirection.Clockwise : DJIGimbalRotateDirection.CounterClockwise);
                    DJIGimbalSpeedRotation mYaw = new DJIGimbalSpeedRotation(Math.abs(yaw), yaw >= 0 ? DJIGimbalRotateDirection.Clockwise : DJIGimbalRotateDirection.CounterClockwise);
                    aircraft.getGimbal().rotateGimbalBySpeed(mPitch, mRoll, mYaw, new DJICommonCallbacks.DJICompletionCallback() {
                        @Override
                        public void onResult(DJIError djiError) {
                            handleDJIError(out, djiError);
                        }
                    });
                    lastGimbalActionTime = System.currentTimeMillis();
                } catch (NumberFormatException e) {
                    outString(out, e.getMessage());
                }
            }
        } else if(commands[0].equals("rc")) {
            DJIFlightController flightController;
            flightController = aircraft.getFlightController();
            if(commands[1].equals("enable")){
                flightController.enableVirtualStickControlMode(new DJICommonCallbacks.DJICompletionCallback() {
                    @Override
                    public void onResult(DJIError djiError) {
                        handleDJIError(out, djiError);
                    }
                });
            } else if (commands[1].equals("disable")) {
                flightController.disableVirtualStickControlMode(new DJICommonCallbacks.DJICompletionCallback() {
                    @Override
                    public void onResult(DJIError djiError) {
                        handleDJIError(out, djiError);
                    }
                });
            } else {
                //rc Roll Pitch Yaw Throttle
                try {
                    Float roll = Float.valueOf(commands[1]);
                    Float pitch = Float.valueOf(commands[2]);
                    Float yaw = Float.valueOf(commands[3]);
                    Float throttle = Float.valueOf(commands[4]);
                    DJIVirtualStickFlightControlData controlData = new DJIVirtualStickFlightControlData(pitch, roll, yaw, throttle);
                    flightController.sendVirtualStickFlightControlData(controlData, new DJICommonCallbacks.DJICompletionCallback() {
                        @Override
                        public void onResult(DJIError djiError) {
                            handleDJIError(out, djiError);
                        }
                    });
                } catch (NumberFormatException e) {
                    outString(out, e.getMessage());
                }
            }
        }
    }
}
