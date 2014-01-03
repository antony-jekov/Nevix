package com.antonyjekov.nevix.common;

import com.pubnub.api.Pubnub;

import org.json.JSONObject;

public class PusherManager {
    private final String SUBSCRIBE_KEY = "sub-c-c7a22dee-6f0c-11e3-9291-02ee2ddab7fe";
    private final String PUBLISH_KEY = "pub-c-2db685fb-40f0-4f91-a074-31ab9993d2d6";

    private Pubnub pusher;

    public PusherManager(String channelName) {
        pusher = new Pubnub(PUBLISH_KEY, SUBSCRIBE_KEY, false);
    }

    public void SendCommand(String cmd)
    {

        //pusher.publish();
    }
}
