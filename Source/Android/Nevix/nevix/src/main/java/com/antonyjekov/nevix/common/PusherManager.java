package com.antonyjekov.nevix.common;

import android.util.Log;

import com.pubnub.api.Callback;
import com.pubnub.api.Pubnub;
import com.pubnub.api.PubnubError;

public class PusherManager {
    private final String SUBSCRIBE_KEY = "sub-c-c7a22dee-6f0c-11e3-9291-02ee2ddab7fe";
    private final String PUBLISH_KEY = "pub-c-2db685fb-40f0-4f91-a074-31ab9993d2d6";

    private Pubnub pusher;
    private String channel;
    private Callback callback;

    public PusherManager(String channelName) {
        this.channel = channelName;
        pusher = new Pubnub(PUBLISH_KEY, SUBSCRIBE_KEY, false);

        callback = new Callback() {
            public void successCallback(String channel, Object response) {
                Log.d("PUBNUB",response.toString());
            }
            public void errorCallback(String channel, PubnubError error) {
                Log.d("PUBNUB", error.toString());
            }
        };
    }

    public void pushCommand(String cmd)
    {
        pusher.publish(channel, cmd, callback );
    }
}
